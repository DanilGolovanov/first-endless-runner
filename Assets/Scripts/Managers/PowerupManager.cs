using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EndlessRunner.Generators;
using EndlessRunner.Controllers;
using EndlessRunner.ObjectManagement;

namespace EndlessRunner.Managers
{
    public class PowerupManager : MonoBehaviour
    {
        #region Variables

        // variables indicating which powerup is active
        private bool doublePoints;
        private bool safeMode;
        private bool highJump;

        // variable indicating if there are any powerup that was activated
        private bool powerupActive;
        // keep track of time during which powerup is affecting player
        private float powerupLengthCounter;

        private ScoreManager _scoreManager;
        private PlatformGenerator _platformGenerator;
        private GameManager _gameManager;
        private PlayerController _player;

        // if multiple double points powerups are active at the same time
        // this variable holds last default value
        // e.g. currentDefaultPointsPerSecond is 2, if player picked 2 powerups it becomes 4 
        private float currentDefaultPointsPerSecond;
        // store initial values and use it to reset powerup
        private float absoluteDefaultPointsPerSecond;
        private float defaultSpikeThreshold;
        private float defaultJumpForce;
        private float defaultJumpTime;

        // all spikes in the scene
        private ObjectDestroyer[] _spikes;

        [SerializeField, Tooltip("Audio file with powerup sound.")]
        private AudioSource _powerupSound;

        [SerializeField, Tooltip("Text gameObject where the powerup will be shown.")]
        private Text _powerupText;
        private Color32 textColor;

        #endregion

        #region Default Methods
        private void Start()
        {
            // get intances of following classes
            _scoreManager = FindObjectOfType<ScoreManager>();
            _platformGenerator = FindObjectOfType<PlatformGenerator>();
            _gameManager = FindObjectOfType<GameManager>();
            _player = FindObjectOfType<PlayerController>();

            // get initial (default) values
            currentDefaultPointsPerSecond = _scoreManager.pointsPerSecond;
            absoluteDefaultPointsPerSecond = _scoreManager.pointsPerSecond;
            defaultSpikeThreshold = _platformGenerator.randomSpikeThreshold;
            defaultJumpForce = _player.jumpForce;
            defaultJumpTime = _player.jumpTime;
        }

        private void Update()
        {
            ResetPowerupsIfPlayerDied();

            if (powerupActive)
            {
                // reduce time counter each frame
                powerupLengthCounter -= Time.deltaTime;

                // check which powerups to activate
                UseActivePowerup();

                // check the time counter and deactivate powerups if it's negative
                if (powerupLengthCounter <= 0)
                {
                    DeactivatePowerups();
                }
            }
        }
        #endregion

        #region Custom Methods
        /// <summary>
        /// Check which powerup need to be activated and activate this powerup.
        /// </summary>
        private void UseActivePowerup()
        {
            if (doublePoints)
            {
                ActivateDoublePoints();
            }
            if (safeMode)
            {
                ActivateSafeMode();
            }
            if (highJump)
            {
                ActivateHighJump();
            }
        }

        /// <summary>
        /// Make him jump higher and longer by changing values related to jump stats of the player.
        /// </summary>
        private void ActivateHighJump()
        {
            _player.jumpForce = 15;
            _player.jumpTime = 0.6f;
            textColor = new Color32(68, 156, 70, 255);
            UpdatePowerupText("High Jump");
        }

        /// <summary>
        /// Set % chance of spikes generation (randomSpikeThreshold) to 0.
        /// </summary>
        private void ActivateSafeMode()
        {
            _platformGenerator.randomSpikeThreshold = 0f;
            textColor = new Color32(68, 113, 183, 255);
            UpdatePowerupText("Safe Mode");
        }

        /// <summary>
        /// Change value of pointPerSecond in score manager 
        /// and double points when player collects coins.
        /// </summary>
        private void ActivateDoublePoints()
        {
            _scoreManager.pointsPerSecond = currentDefaultPointsPerSecond * 2.5f;
            _scoreManager.doublePointsIsActive = true;
            textColor = new Color32(233, 233, 72, 255);
            UpdatePowerupText("x2 Points");
        }

        /// <summary>
        /// Update powerup text when powerup is picked up.
        /// </summary>
        /// <param name="powerupName">Name of the powerup that was picked up.</param>
        private void UpdatePowerupText(string powerupName)
        {
            _powerupText.color = textColor;
            _powerupText.gameObject.SetActive(true);
            _powerupText.text = Mathf.Round(powerupLengthCounter) + "s " + powerupName;
        }

        /// <summary>
        /// Reset powerup time counter and all current effects of powerups when player dies.
        /// </summary>
        private void ResetPowerupsIfPlayerDied()
        {
            if (_gameManager.powerupReset == true)
            {
                powerupLengthCounter = 0;
                DeactivatePowerups();
                _scoreManager.pointsPerSecond = absoluteDefaultPointsPerSecond;
                _gameManager.powerupReset = false;
            }
        }

        /// <summary>
        /// Reset all powerup related values to defaults.
        /// </summary>
        public void DeactivatePowerups()
        {
            _scoreManager.pointsPerSecond = absoluteDefaultPointsPerSecond;
            _scoreManager.doublePointsIsActive = false;
            _player.jumpForce = defaultJumpForce;
            _player.jumpTime = defaultJumpTime;
            _platformGenerator.randomSpikeThreshold = defaultSpikeThreshold;
            powerupActive = false;
            _powerupText.gameObject.SetActive(false);
        }

        /// <summary>
        /// Activate picked up powerup by changing default values related to this powerup.
        /// </summary>
        /// <param name="isDoublePoints">Variable indicating if double points powerup is active.</param>
        /// <param name="isSafeMode">Variable indicating if safe mode powerup is active.</param>
        /// <param name="isHighJump">Variable indicating if high jump powerup is active.</param>
        /// <param name="time">How long the powerup will be active.</param>
        public void ActivatePowerup(bool isDoublePoints, bool isSafeMode, bool isHighJump, float time)
        {
            // check which powerup was picked up
            doublePoints = isDoublePoints;
            safeMode = isSafeMode;
            highJump = isHighJump;
            // how long the powerup will last
            powerupLengthCounter = time;

            // change default settings when the powerup is picked up
            currentDefaultPointsPerSecond = _scoreManager.pointsPerSecond;
            defaultSpikeThreshold = _platformGenerator.randomSpikeThreshold;

            DestroyActiveSpikes(safeMode);

            powerupActive = true;

              _powerupSound.Play();
        }

        /// <summary>
        /// Destroy all spikes that are present in the scene when player picks up safe mode powerup.
        /// </summary>
        /// <param name="safeMode">Variable indicating if safe mode powerup is active.</param>
        private void DestroyActiveSpikes(bool safeMode)
        {
            if (safeMode)
            {
                _spikes = FindObjectsOfType<ObjectDestroyer>();
                for (int i = 0; i < _spikes.Length; i++)
                {
                    if (_spikes[i].gameObject.name.Contains("Spikes"))
                    {
                        _spikes[i].gameObject.SetActive(false);
                    }
                }
            }
        }
        #endregion
    }
}
