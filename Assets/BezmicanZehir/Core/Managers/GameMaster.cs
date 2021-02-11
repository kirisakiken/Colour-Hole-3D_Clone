using System.Linq;
using UnityEngine;

namespace BezmicanZehir.Core.Managers
{
    public class GameMaster : MonoBehaviour
    {
        private PlayerInput _playerInput;
        public int currentNormalObstacleCount;

        public Transform firstStage;
        public Transform secondStage;

        public static Transform ActiveStage;

        public bool currentStageIsCleared;
        
        private void Awake()
        {
            _playerInput = FindObjectOfType<PlayerInput>();
            currentNormalObstacleCount = GetNormalObstacleCount(firstStage);
            ActiveStage = firstStage;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                StageInitialization();
            }
            
            if (currentStageIsCleared)
            {
                if (ActiveStage == firstStage)
                {
                    _playerInput.currentState = 2; // Set currentState to switching
                    StartCoroutine(_playerInput.MoveCameraToNextStage(secondStage));
                    ActiveStage = secondStage;
                    currentStageIsCleared = false;
                }
                else if (ActiveStage == secondStage)
                {
                    _playerInput.currentState = 3;
                    currentStageIsCleared = false;
                    _playerInput.OnLevelCleared?.Invoke();
                }
            }
        }

        public void StageInitialization()
        {
            currentNormalObstacleCount = GetNormalObstacleCount(ActiveStage);
            _playerInput.currentState = 1;
        }

        /// <summary>
        /// This method used to get normal obstacle counts in given stage
        /// </summary>
        /// <param name="stage"> Current active stage </param>
        /// <returns> Normal obstacle count </returns>
        private int GetNormalObstacleCount(Transform stage)
        {
            return stage.GetComponentsInChildren<Obstacle>().Where(o => !o.isKiller).ToArray().Length;
        }

        public void SetSelectable(GameObject go)
        {
            go.tag = "Selectable";
        }
    }
}
