using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EndlessRunner.Managers;

namespace EndlessRunner.Collectables
{
    public class Coins : MonoBehaviour
    {
        [SerializeField, Tooltip("How many points to give when player gets the coin.")]
        private int scoreToGive;
        private ScoreManager _scoreManager;
        private AudioSource _coinSound;

        private void Start()
        {
            // get score manager
            _scoreManager = FindObjectOfType<ScoreManager>();
            // get coin sound
            _coinSound = GameObject.Find("CoinSound").GetComponent<AudioSource>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // if player collides with the coin
            if (collision.gameObject.name == "Player")
            {
                // add given number of points to the score at the top left corner of the game UI
                _scoreManager.AddScoreForCoins(scoreToGive);
                // delete coin, with which player collided, from the scene
                gameObject.SetActive(false);

                PlayCoinSound();
            }
        }

        /// <summary>
        ///  Play coin sound and make sure to play sound on every coin when player is on high speed 
        ///  (if there are multiple coins in a row usually by default some sounds may be skipped because the previous one didn't finish to play).
        /// </summary>
        private void PlayCoinSound()
        {
            if (_coinSound.isPlaying)
            {
                _coinSound.Stop();
                _coinSound.Play();
            }
            else
            {
                _coinSound.Play();
            }
        }
    }
}
