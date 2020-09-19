using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EndlessRunner.Managers
{
    public class ScoreManager : MonoBehaviour
    {
        #region Variables

        [Header("Score")]
        [SerializeField, Tooltip("Text gameObject where the current score will be shown.")]
        private Text _scoreText;
        [Tooltip("The actual value of the current score.")]
        public float scoreCount;

        [Header("High Score")]
        [SerializeField, Tooltip("Text gameObject where the high score will be shown.")]
        private Text _highScoreText;
        [SerializeField, Tooltip("The actual value of the high score.")]
        private float highScoreCount;

        [Header("Score Management")]
        [Tooltip("How many points per second player scores.")]
        public float pointsPerSecond;
        [Tooltip("Indicates if score is need to be keep increasing (stops increasing when player dies).")]
        public bool scoreIncreasing;
        [Tooltip("Indicates if double points powerup is active.")]
        public bool doublePointsIsActive;

        #endregion

        #region Default Methods
        private void Start()
        {
            // get stored high score if player played the game before
            if (PlayerPrefs.HasKey("HighScore"))
            {
                highScoreCount = PlayerPrefs.GetFloat("HighScore");
            }
        }

        private void Update()
        {
            AddScore();
            SetHighScore();
            UpdateScore();
            UpdateHighScore();
        }
        #endregion

        #region Custom Methods
        /// <summary>
        /// Add points to the current score when player collects coin.
        /// If double points powerup is active the amount of points to be added is doubled as well.
        /// </summary>
        /// <param name="pointsToAdd">How many points to add when player collects 1 coin.</param>
        public void AddScoreForCoins(int pointsToAdd)
        {
            if (doublePointsIsActive)
            {
                pointsToAdd *= 2;
            }
            scoreCount += pointsToAdd;
        }

        /// <summary>
        /// Change high score if current score of the player is bigger than the previous high score.
        /// </summary>
        private void SetHighScore()
        {
            if (scoreCount > highScoreCount)
            {
                highScoreCount = scoreCount;
                // save high score
                PlayerPrefs.SetFloat("HighScore", highScoreCount);
            }
        }

        /// <summary>
        /// Keep adding points if player is alive.
        /// </summary>
        private void AddScore()
        {
            if (scoreIncreasing)
            {
                scoreCount += pointsPerSecond * Time.deltaTime;
            }
        }

        /// <summary>
        /// Update UI text element of the high score.
        /// </summary>
        private void UpdateHighScore()
        {
            _highScoreText.text = "High Score: " + Mathf.Round(highScoreCount);
        }

        /// <summary>
        /// Update UI text element of the current score.
        /// </summary>
        private void UpdateScore()
        {
            _scoreText.text = "Score: " + Mathf.Round(scoreCount);
        }
        #endregion
    }
}
