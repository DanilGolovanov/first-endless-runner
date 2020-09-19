using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EndlessRunner.Menus
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField, Tooltip("'GameScene'.")]
        private string gameScene;

        public void StartGame()
        {
            // open game scene from main menu
            SceneManager.LoadScene(gameScene);
        }

        public void QuitGame()
        {
            // quit game in unity editor
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif

            // quit game in the final build
            Application.Quit();
        }
    }
}
