using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EndlessRunner.Managers;

namespace EndlessRunner.Collectables
{
    public class Powerups : MonoBehaviour
    {
        #region Variables

        // variables to indicate which powerup was picked up
        private bool isDoublePoints;
        private bool isSafeMode;
        private bool isHighJump;

        [SerializeField, Tooltip("How many seconds powerup will be applicable for.")]
        public float powerupLength;

        private PowerupManager _powerupManager;

        // array of sprites that will be used for powerups
        // NOTE: atm there is only one sprite, array is added to scale the feature later on
        public Sprite[] powerupSprites;

        #endregion

        private void Start()
        {
            // get powerup manager
            _powerupManager = FindObjectOfType<PowerupManager>();
        }

        private void Awake()
        {
            CreateRandomPowerup();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // if there is a collision between player and powerup
            if (collision.name == "Player")
            {
                // deactivate active powerups if any
                _powerupManager.DeactivatePowerups();
                // apply picked up powerup
                _powerupManager.ActivatePowerup(isDoublePoints, isSafeMode, isHighJump, powerupLength);
            }
            // delete powerup from the scene
            gameObject.SetActive(false);
        }

        private void CreateRandomPowerup()
        {
            // pick random powerup
            int powerupSelector = Random.Range(0, 3);
            // activate picked powerup
            switch (powerupSelector)
            {
                case 0:
                    isDoublePoints = true;
                    break;
                case 1:
                    isHighJump = true;
                    break;
                case 2:
                    isSafeMode = true;
                    break;
            }
            // attach corresponding sprite to the powerup prefab
            GetComponent<SpriteRenderer>().sprite = powerupSprites[0];
        }
    }
}
