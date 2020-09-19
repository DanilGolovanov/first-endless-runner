using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EndlessRunner.ObjectManagement;

namespace EndlessRunner.Generators
{
    public class PlatformGenerator : MonoBehaviour
    {
        #region Variables

        [Header("Platforms")]
        [SerializeField, Tooltip("Point ahead of camera where the platforms are generated.")]
        private Transform _generationPoint;
        [SerializeField, Tooltip("Platform prefabs.")]
        private ObjectPooler[] _platformPools;

        // index to select random platform prefab from the array
        private int platformSelector;
        // array of different platform widths
        private float[] platformWidths;

        [Space]
        // distance between platforms
        private float distanceBetween;
        [SerializeField, Tooltip("Minimum distance between two consecutive platforms.")]
        private float minDistanceBetween;
        [SerializeField, Tooltip("Maximum distance between two consecutive platforms.")]
        private float maxDistanceBetween;

        // minimum height to which plaform generation point can go
        private float minHeight;
        [SerializeField, Tooltip("Maximum height point up to which plaform generation point can go.")]
        private Transform _maxHeightPoint;
        // position of the maxHeightPoint
        private float maxHeight;
        [SerializeField, Tooltip("Maximum height difference between two consecutive platforms.")]
        private float maxHeightChange;
        // actual height change
        private float heightChange;

        [Header("Coins")]
        private CoinGenerator _coinGenerator;
        [SerializeField, Tooltip("% chance of getting coins on the next platform.")]
        private float randomCoinThreshold;

        [Header("Spikes")]
        [Tooltip("% chance of getting spikes on the next platform.")]
        public float randomSpikeThreshold;
        [SerializeField, Tooltip("Spikes prefab.")]
        private ObjectPooler _spikePool;

        [Header("Powerups")]
        [SerializeField, Tooltip("Height above the platform where the powerup will be created.")]
        private float powerupHeight;
        [SerializeField, Tooltip("Powerup prefab.")]
        private ObjectPooler _powerupPool;
        [SerializeField, Tooltip("% chance of getting powerup on the next platform.")]
        private float randomPowerupThreshold;

        #endregion

        #region Default Methods
        private void Start()
        {
            platformWidths = GetPlatformWidths();

            // min and max position of the platform on y-axis
            minHeight = transform.position.y;
            maxHeight = _maxHeightPoint.position.y;

            // get coin generator
            _coinGenerator = FindObjectOfType<CoinGenerator>();
        }

        private void Update()
        {
            // if player is behind the generation point
            if (transform.position.x < _generationPoint.position.x)
            {
                CreateRandomPlatform();
                SpawnCoinsRandomly();
                SpawnSpikesRandomly();
                SpawnPowerupsRandomly();
                MovePlatformGenerator();
            }
        }
        #endregion

        #region Custom Methods
        /// <summary>
        /// Get platform widths from each of the available platform prefabs.
        /// </summary>
        private float[] GetPlatformWidths()
        {
            float[] platformWidths = new float[_platformPools.Length];
            for (int i = 0; i < _platformPools.Length; i++)
            {
                platformWidths[i] = _platformPools[i].pooledObject.GetComponent<BoxCollider2D>().size.x;
            }
            return platformWidths;
        }

        /// <summary>
        /// Move platform generator to the end of the last platform to avoid overlapping platforms.
        /// </summary>
        private void MovePlatformGenerator()
        {
            transform.position = new Vector3(transform.position.x + (platformWidths[platformSelector] / 2), transform.position.y, transform.position.z);
        }

        /// <summary>
        /// Create powerups in random locations using provided prefab and % chance of powerup generation to .
        /// </summary>
        private void SpawnPowerupsRandomly()
        {
            if (Random.Range(0f, 100f) < randomPowerupThreshold)
            {
                GameObject newPowerup = _powerupPool.GetPooledObject();
                // create powerup between platforms at random height
                newPowerup.transform.position = transform.position + new Vector3(distanceBetween / 2f, Random.Range(1f, powerupHeight), 0f);
                newPowerup.SetActive(true);
            }
        }

        /// <summary>
        /// Create spikes in random locations on top of the platform using provided prefab and % chance of spikes generation.
        /// </summary>
        private void SpawnSpikesRandomly()
        {
            if (Random.Range(0f, 100f) < randomSpikeThreshold)
            {
                GameObject newSpike = _spikePool.GetPooledObject();
                // pick random position of spikes on x-axis on the platform
                float spikeXPosition = Random.Range(-platformWidths[platformSelector] / 2f + 1f, platformWidths[platformSelector] / 2f - 1f);
                // move spike's position half of the platform height up (platform height is 1f)
                Vector3 spikePositionChange = new Vector3(spikeXPosition, 0.65f, 0f);
                newSpike.transform.position = transform.position + spikePositionChange;
                newSpike.transform.rotation = transform.rotation;
                newSpike.SetActive(true);
            }
        }

        /// <summary>
        /// Ranodomly choose on which platform to spawn coins 
        /// and create 3 coins above the platform using provided prefab and % chance of coin generation.
        /// </summary>
        private void SpawnCoinsRandomly()
        {
            if (Random.Range(0f, 100f) < randomCoinThreshold)
            {
                _coinGenerator.SpawnCoins(new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z));
            }
        }

        /// <summary>
        /// Create platform with a random width and in the random position.
        /// </summary>
        private void CreateRandomPlatform()
        {
            // pick random platform size from the array of prefabs
            platformSelector = Random.Range(0, _platformPools.Length);
            // pick random position without overlapping
            PickRandomPlatformPosition(platformSelector);
            // create new platform 
            GameObject newPlatform = _platformPools[platformSelector].GetPooledObject();
            newPlatform.transform.position = transform.position;
            newPlatform.transform.rotation = transform.rotation;
            newPlatform.SetActive(true);
        }

        /// <summary>
        /// Pick a random position for platform using provided "boundaries" variables (e.g. maxHeightChange).
        /// </summary>
        /// <param name="platformSelector">Random index to choose random platform from the array.</param>
        private void PickRandomPlatformPosition(int platformSelector)
        {
            // pick random distance between platforms
            distanceBetween = Random.Range(minDistanceBetween, maxDistanceBetween);
            // change current position of the platform generator by random value
            heightChange = transform.position.y + Random.Range(-maxHeightChange, maxHeightChange);
            // force height of the platform to be within screen height
            heightChange = Mathf.Clamp(heightChange, minHeight, maxHeight);
            // set position for a new platform
            transform.position = new Vector3(transform.position.x + (platformWidths[platformSelector] / 2) + distanceBetween, heightChange, transform.position.z);
        }
        #endregion
    }
}
