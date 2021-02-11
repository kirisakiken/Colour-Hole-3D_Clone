using System;
using BezmicanZehir.Core.Managers;
using UnityEngine;
using UnityEngine.Events;

namespace BezmicanZehir.Core
{
    public class KillerZone : MonoBehaviour
    {
        [SerializeField] private GameMaster gameMaster;
        [SerializeField] private PlayerInput playerInput;

        public UnityEvent OnNormalObstacleDestroy;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("KillerObstacle")) // Death Event
            {
                // Play haptic feedback
                HapticFeedback.Vibrate(150);
                playerInput.currentState = 4;
                playerInput.OnDeath?.Invoke();
                Destroy(other.gameObject);
            }
            else if (other.CompareTag("NormalObstacle"))
            {
                // Play haptic feedback
                HapticFeedback.Vibrate(10);
                gameMaster.currentNormalObstacleCount--;
                OnNormalObstacleDestroy?.Invoke();
                Destroy(other.gameObject);
                if (gameMaster.currentNormalObstacleCount == 0)
                {
                    gameMaster.currentStageIsCleared = true;
                }
            }
            else if (other.CompareTag("MidObstacle"))
            {
                // Play haptic feedback
                HapticFeedback.Vibrate(1);
                Destroy(other.gameObject);
            }
        }
    }
}
