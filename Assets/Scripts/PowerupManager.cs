using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    private bool doublePoints;
    private bool safeMode;
    private bool powerupActive;
    private float powerupLengthCounter;

    private ScoreManager _scoreManager;
    private PlatformGenerator _platformGenerator;
    private GameManager _gameManager;

    private float defaultPointsPerSecond = 5;
    private float defaultSpikeThreshold;

    private ObjectDestroyer[] spikes;

    // Start is called before the first frame update
    void Start()
    {
        _scoreManager = FindObjectOfType<ScoreManager>();
        _platformGenerator = FindObjectOfType<PlatformGenerator>();
        _gameManager = FindObjectOfType<GameManager >();
    }

    // Update is called once per frame
    void Update()
    {
        if (powerupActive)
        {
            powerupLengthCounter -= Time.deltaTime;

            if (_gameManager.powerupReset == true)
            {
                powerupLengthCounter = 0;
                _gameManager.powerupReset = false;
            }

            if (doublePoints)
            {
                _scoreManager.pointsPerSecond = defaultPointsPerSecond * 2.5f;
                _scoreManager.doublePointsIsActive = true;
            }
            if (safeMode)
            {
                _platformGenerator.randomSpikeThreshold = 0f;
            }
            if (powerupLengthCounter <= 0)
            {
                _scoreManager.pointsPerSecond = defaultPointsPerSecond;
                _scoreManager.doublePointsIsActive = false;
                _platformGenerator.randomSpikeThreshold = defaultSpikeThreshold;
                powerupActive = false;
            }
        }
    }

    public void ActivatePowerup(bool isDoublePoints, bool isSafeMode, float time)
    {
        //check which powerup was picked up
        doublePoints = isDoublePoints;
        safeMode = isSafeMode;
        //how long the powerup will last
        powerupLengthCounter = time;

        //change default settings when the powerup is picked up
        defaultPointsPerSecond = _scoreManager.pointsPerSecond;
        defaultSpikeThreshold = _platformGenerator.randomSpikeThreshold;

        //destroy all spikes that are present in the scene when player got the powerup
        if (safeMode)
        {
            spikes = FindObjectsOfType<ObjectDestroyer>();
            for (int i = 0; i < spikes.Length; i++)
            {
                if (spikes[i].gameObject.name.Contains("Spikes"))
                {
                    spikes[i].gameObject.SetActive(false);
                }             
            }
        }

        powerupActive = true;
    }
}
