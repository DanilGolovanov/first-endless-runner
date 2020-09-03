using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    public Text highScoreText;
    public float scoreCount;
    public float highScoreCount;
    public float pointsPerSecond;
    public bool scoreIncreasing;
    public bool doublePointsIsActive;

    // Start is called before the first frame update
    void Start()
    {
        //get the high score if player played the game before
        if (PlayerPrefs.HasKey("HighScore"))
        {
            highScoreCount = PlayerPrefs.GetFloat("HighScore");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if player is alive keep adding points
        if (scoreIncreasing)
        {
            scoreCount += pointsPerSecond * Time.deltaTime;
        }    
        //set high score 
        if (scoreCount > highScoreCount)
        {
            highScoreCount = scoreCount;
            //save high score
            PlayerPrefs.SetFloat("HighScore", highScoreCount);
        }
        
        scoreText.text = "Score: " + Mathf.Round(scoreCount);
        highScoreText.text = "High Score: " + Mathf.Round(highScoreCount);
    }

    public void AddScore(int pointsToAdd)
    {
        if (doublePointsIsActive)
        {
            pointsToAdd *= 2;
        }
        scoreCount += pointsToAdd;
    }
}
