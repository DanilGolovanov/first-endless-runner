using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using EndlessRunner.Managers;

namespace EndlessRunner.Menus
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField, Tooltip("'MainMenuScene'.")]
        private string mainMenuScene;
        [SerializeField, Tooltip("Pause menu button.")]
        private GameObject pauseButton;

        public void PauseGame()
        {
            Time.timeScale = 0f;
            gameObject.SetActive(true);
            pauseButton.SetActive(false);
        }

        public void ResumeGame()
        {
            Time.timeScale = 1f;
            gameObject.SetActive(false);
            pauseButton.SetActive(true);
        }

        public void RestartGame()
        {
            Time.timeScale = 1f;
            gameObject.SetActive(false);
            pauseButton.SetActive(true);
            FindObjectOfType<GameManager>().ResetGame();
        }

        public void QuitToMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(mainMenuScene);
        }
    }
}
