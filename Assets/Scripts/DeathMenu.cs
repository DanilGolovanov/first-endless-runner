using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public string mainMenuScene;
    public GameManager gameManager;

    // Update is called once per frame
    void Update()
    {
        //restart the game by pressing "return"
        if (gameObject.activeInHierarchy && Input.GetKeyDown(KeyCode.Return))
        {
            gameManager.ResetGame();
        }
    }

    public void RestartGame()
    {
        //reset all values (speed, distance covered, score etc.)
        gameManager.ResetGame();
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
}
