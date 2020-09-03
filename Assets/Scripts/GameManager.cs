using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform platformGenerator;
    private Vector3 platformStartPoint;

    public PlayerController player;
    private Vector3 playerStartPoint;

    private ObjectDestroyer[] platforms;

    private ScoreManager _scoreManager;
    public DeathMenu deathMenu;
    public bool powerupReset;

    // Start is called before the first frame update
    void Start()
    {
        platformStartPoint = platformGenerator.position;
        playerStartPoint = player.transform.position;
        _scoreManager = FindObjectOfType<ScoreManager>();
    }

    public void RestartGame()
    {
        //stop scoring points when player dies
        _scoreManager.scoreIncreasing = false;
        //when player dies make player invisible
        player.gameObject.SetActive(false);
        //activate death screen when player dies
        deathMenu.gameObject.SetActive(true);
    }

    public void ResetGame()
    {
        //deactivate death screen
        deathMenu.gameObject.SetActive(false);
        //destroy all platforms that were created previously before starting new game
        platforms = FindObjectsOfType<ObjectDestroyer>();
        for (int i = 0; i < platforms.Length; i++)
        {
            platforms[i].gameObject.SetActive(false);
        }
        //reset positions of player
        player.transform.position = playerStartPoint;
        //reset position of platform generator
        platformGenerator.position = platformStartPoint;
        //make player visible again
        player.gameObject.SetActive(true);
        //reset the score
        _scoreManager.scoreCount = 0;
        _scoreManager.scoreIncreasing = true;
        //reset powerups
        powerupReset = true;
    } 

}
