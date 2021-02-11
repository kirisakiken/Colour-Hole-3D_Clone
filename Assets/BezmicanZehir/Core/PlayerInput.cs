using System;
using System.Collections;
using System.Collections.Generic;
using BezmicanZehir.Core.Managers;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    public int currentState;
    
    [SerializeField] private Camera mainCamera;
    [SerializeField] [Range(0, 1)] [Tooltip("Camera speed while switching stages")] private float stageTransitionSpeed; // Camera movement speed when switching stages
    [SerializeField] private LayerMask groundMask;
    [SerializeField] [Tooltip("First Stage bounds (+/- x, +/- z)")] private Vector4 firstStageLimits;  // First Stage bounds (+/- x, +/- z)
    [SerializeField] [Tooltip("Second Stage bounds (+/- x, +/- z)")] private Vector4 secondStageLimits; // Second Stage bounds (+/- x, +/- z)
    public Vector4 zoneLimit; // Active Stage bounds (+/- x, +/- z)
    private bool _holdingHole;

    [SerializeField] private bool mobileInputEnable;
    [SerializeField] private float dragSpeed;
    private Touch _touch;
    private Vector3 _deltaVector;

    public UnityEvent OnLevelStarted;
    public UnityEvent OnTranslationStarted;
    public UnityEvent OnTranslationFinished;
    public UnityEvent OnLevelCleared;
    public UnityEvent OnDeath;
    
    private void Start()
    {
        _holdingHole = false;
        zoneLimit = firstStageLimits;
    }

    private void Update()
    {
        switch (currentState)
        {
            case 0: //Standby
                if (Input.touchCount > 0)
                {
                    OnLevelStarted?.Invoke();
                    currentState = 1;
                }
                break;
            case 1: //Playing
                _deltaVector = Input.touchCount > 0 ? DeltaSwerve() : Vector3.zero;
                break;
        }
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case 0: //Standby
                if (Input.GetMouseButtonDown(0))
                {
                    OnLevelStarted?.Invoke();
                    currentState = 1;
                }
                break;
            case 1: //Playing
                if (!mobileInputEnable) // Desktop input
                {
                    if (Input.GetMouseButton(0) && _holdingHole)
                    {
                        if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out var theHit, 50.0f,
                            groundMask) && theHit.transform.CompareTag("Selectable"))
                        {
                            transform.position = theHit.point;
                        }
                    }
                }
                else // Mobile & UnityRemote Input
                {
                    if (_deltaVector != Vector3.zero && mobileInputEnable)
                    {
                        transform.position += _deltaVector;
                        // Below is used to keep hole inside play ground
                        transform.position = new Vector3(Mathf.Clamp(transform.position.x, zoneLimit.x, zoneLimit.y),
                            transform.position.y, Mathf.Clamp(transform.position.z, zoneLimit.z, zoneLimit.w));
                    }
                }
                break;
            case 2: //Switching
                //----
                break;
            case 3: //End of level
                //----
                break;
            case 4: // Death
                //----
                break;
        }
        
    }

    private void OnMouseDown()
    {
        _holdingHole = true;
    }

    private void OnMouseUp()
    {
        _holdingHole = false;
    }
    
    /// <summary>
    /// This method is used to move camera to next stage when current stage has been cleared.
    /// </summary>
    /// <param name="stage"> Target stage. Since the game contains only two stages in each level, SecondStage.</param>
    /// <returns> Returns WaitForSeconds</returns>
    public IEnumerator MoveCameraToNextStage(Transform stage)
    {
        OnTranslationStarted?.Invoke();

        zoneLimit = secondStageLimits;
        
        if (stage == GameMaster.ActiveStage)
        {
            currentState = 3;
        }
        var holeDestination = new Vector3(0.0f, 0.0f, -3.0f);
        
        // Move hole to the current stage center
        do
        {
            transform.position = Vector3.MoveTowards(transform.position, holeDestination, 0.1f);
            yield return new WaitForSeconds(0.001f);
        } while (Vector3.Distance(transform.position, holeDestination) > 0.05f);

        transform.parent = mainCamera.transform;
        
        var destinationPoint = stage.localPosition + new Vector3(0.0f, 17.0f, -15.0f);
        // Move camera to the target stage
        do
        {
            mainCamera.transform.localPosition = Vector3.MoveTowards(mainCamera.transform.localPosition, destinationPoint, stageTransitionSpeed);
            yield return new WaitForSeconds(0.001f);
        } while (Vector3.Distance(mainCamera.transform.localPosition, destinationPoint) > 0.25f);
        mainCamera.transform.localPosition = destinationPoint;
        
        transform.parent = null;
        
        OnTranslationFinished?.Invoke();
    }

    private Vector3 DeltaSwerve()
    {
        if (Input.touchCount > 0)
        {
            _touch = Input.GetTouch(0);
            if (_touch.phase == TouchPhase.Moved)
            {
                var x = _touch.deltaPosition.x * dragSpeed;
                var z = _touch.deltaPosition.y * dragSpeed;

                return new Vector3(x, 0.0f, z);
            }
        }

        return Vector3.zero;
    }
}