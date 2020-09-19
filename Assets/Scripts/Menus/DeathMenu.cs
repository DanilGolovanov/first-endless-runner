using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using EndlessRunner.Managers;

namespace EndlessRunner.Menus
{
    public class DeathMenu : MonoBehaviour
    {
        [SerializeField, Tooltip("'MainMenuScene'.")]
        private string mainMenuScene;
        [SerializeField, Tooltip("GameManager gameObject from the scene.")]
        private GameManager _gameManager;

        private void Update()
        {
            // restart the game by pressing "enter"
            if (gameObject.activeInHierarchy && Input.GetKeyDown(KeyCode.Return))
            {
                _gameManager.ResetGame();
            }
        }

        public void RestartGame()
        {
            _gameManager.ResetGame();
        }

        public void QuitToMainMenu()
        {
            SceneManager.LoadScene(mainMenuScene);
        }
    }
}
