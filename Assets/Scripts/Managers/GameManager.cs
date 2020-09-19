using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EndlessRunner.ObjectManagement;
using EndlessRunner.Controllers;
using EndlessRunner.Menus;

namespace EndlessRunner.Managers
{
    public class GameManager : MonoBehaviour
    {
        #region Variables

        [SerializeField, Tooltip("Platform Generator gameObject.")]
        private Transform _platformGenerator;
        // initial position of platform generator
        private Vector3 platformStartPoint;
        // all platforms in the scene
        private ObjectDestroyer[] _platforms;

        [SerializeField, Tooltip("Player gameObject with attached PlayerController script.")]
        private PlayerController _player;
        // initial position of the player
        private Vector3 playerStartPoint;

        private ScoreManager _scoreManager;
        // initial value of how many points per second player scores
        private float defaultPointsPerSecond;

        [Tooltip("Death Menu gameObject with attached DeathMenu script.")]
        public DeathMenu deathMenu;

        [Tooltip("Variable indicating when to reset the powerup effects.")]
        public bool powerupReset;

        #endregion

        #region Default Methods
        private void Start()
        {
            // get initial values
            platformStartPoint = _platformGenerator.position;
            playerStartPoint = _player.transform.position;
            _scoreManager = FindObjectOfType<ScoreManager>();
            defaultPointsPerSecond = _scoreManager.pointsPerSecond;
        }
        #endregion

        #region Custom Methods
        /// <summary>
        /// Stop scoring points, make player invisible and activate death menu when player dies.
        /// </summary>
        public void FinishGame()
        {
            // stop scoring points when player dies
            _scoreManager.scoreIncreasing = false;
            // when player dies make player invisible
            _player.gameObject.SetActive(false);
            // activate death screen when player dies
            deathMenu.gameObject.SetActive(true);
        }

        /// <summary>
        /// Reset player stats and position of player and platform generator to initial values,
        /// destroy previously generated platforms and deactivate death menu.
        /// </summary>
        public void ResetGame()
        {
            // deactivate death screen
            deathMenu.gameObject.SetActive(false);
            // destroy all platforms that were created previously 
            _platforms = FindObjectsOfType<ObjectDestroyer>();
            for (int i = 0; i < _platforms.Length; i++)
            {
                _platforms[i].gameObject.SetActive(false);
            }
            // reset position of player
            _player.transform.position = playerStartPoint;
            // reset position of platform generator
            _platformGenerator.position = platformStartPoint;
            // make player visible again
            _player.gameObject.SetActive(true);
            // reset the score
            _scoreManager.scoreCount = 0;
            _scoreManager.scoreIncreasing = true;
            // reset points per second
            _scoreManager.pointsPerSecond = defaultPointsPerSecond;
            // reset powerups
            powerupReset = true;
        }
        #endregion
    }
}
