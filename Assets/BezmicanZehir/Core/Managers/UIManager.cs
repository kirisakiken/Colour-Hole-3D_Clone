using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BezmicanZehir.Core.Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameMaster gameMaster;
        
        [SerializeField] private Slider stageOneProgress;
        [SerializeField] private Slider stageTwoProgress;
        private Slider _activeSlider;

        private void Start()
        {
            InitializeProgressSlider(stageOneProgress);
        }

        public void LoadSceneAtIndex(int index)
        {
            SceneManager.LoadScene(index);
        }

        public void LoadNextScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void RestartScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void EnableGroup(GameObject go)
        {
            go.SetActive(true);
        }

        public void DisableGroup(GameObject go)
        {
            go.SetActive(false);
        }

        public void UpdateSlider()
        {
            _activeSlider.value = _activeSlider.maxValue - gameMaster.currentNormalObstacleCount;
        }

        public void InitializeProgressSlider(Slider slider)
        {
            slider.minValue = 0;
            slider.maxValue = gameMaster.currentNormalObstacleCount;
            _activeSlider = slider;
        }
    }
}
