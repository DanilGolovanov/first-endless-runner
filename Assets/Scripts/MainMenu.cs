using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string gameScene;

    public void StartGame()
    {
        //Open game scene from main menu
        SceneManager.LoadScene(gameScene);
    }

    public void QuitGame()
    {
        //Quit game in unity editor
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif

        //Quit game in the build
        Application.Quit();
    }
}
