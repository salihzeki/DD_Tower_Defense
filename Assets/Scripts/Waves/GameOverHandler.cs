using System.Collections;
using System.Collections.Generic;
using DapperDino.TD.Combat;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DapperDino.TD.Waves
{
    public class GameOverHandler : MonoBehaviour
    {
        [SerializeField] GameObject playerWinPanel = null;
        [SerializeField] GameObject playerLosePanel = null;

        private int nextLevelIndex;

        public const string HighestLevelIndex = "HighestLevelIndex";

        private void OnEnable()
        {
            WaveHandler.OnPlayerWin += HandlePlayerWin;
            HealthSystem.OnPlayerLose += HandlePlayerLose;
        }

        private void OnDisable()
        {
            WaveHandler.OnPlayerWin -= HandlePlayerWin;
            HealthSystem.OnPlayerLose -= HandlePlayerLose;
        }

        private void HandlePlayerWin()
        {
            playerWinPanel.SetActive(true);
            string activeSceneName = SceneManager.GetActiveScene().name;
            string levelIndex = activeSceneName.Split('_')[2];
            int levelIndexValue = int.Parse(levelIndex);
            if(PlayerPrefs.GetInt(HighestLevelIndex, 0) < levelIndexValue)
            {
                PlayerPrefs.SetInt(HighestLevelIndex, levelIndexValue);
            }
            
            nextLevelIndex = levelIndexValue + 1;
        }

         private void HandlePlayerLose()
        {
            Time.timeScale = 0f; //Pause the game.
            playerLosePanel.SetActive(true);
        }

        public void Retry()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Time.timeScale = 1f;
        }

        public void GoToMenu()
        {
            SceneManager.LoadScene("Scene_Menu");

            Time.timeScale = 1f;
        }

        public void GoToNext()
        {
            if (Application.CanStreamedLevelBeLoaded($"Scene_Level_{nextLevelIndex}"))
            {
                SceneManager.LoadScene($"Scene_Level_{nextLevelIndex}");
            }
            else
            {
                SceneManager.LoadScene("Scene_Menu");
            }
        }
    }

}
