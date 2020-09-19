using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EndlessRunner.ObjectManagement;

namespace EndlessRunner.Generators
{
    public class CoinGenerator : MonoBehaviour
    {
        [SerializeField, Tooltip("GameObject with attached ObjectPooler script with coin object as the parameter.")]
        private ObjectPooler _coinPool;

        [SerializeField, Tooltip("Distance between coins generated on one platform.")]
        private float distanceBetweenCoins;

        /// <summary>
        /// Generate 3 coins on the platform in specified position.
        /// </summary>
        /// <param name="startPosition">Position of the middle coin.</param>
        public void SpawnCoins(Vector3 startPosition)
        {
            GenerateCoin(startPosition);
            GenerateCoin(new Vector3(startPosition.x - distanceBetweenCoins, startPosition.y, startPosition.z));
            GenerateCoin(new Vector3(startPosition.x + distanceBetweenCoins, startPosition.y, startPosition.z));
        }

        /// <summary>
        /// Generate individual coin.
        /// </summary>
        /// <param name="position">Position where the coin will be generated.</param>
        private void GenerateCoin(Vector3 position)
        {
            GameObject coin = _coinPool.GetPooledObject();
            coin.transform.position = position;
            coin.SetActive(true);
        }
    }
}
