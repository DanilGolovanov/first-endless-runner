using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinGenerator : MonoBehaviour
{
    public ObjectPooler coinPool;
    public float distanceBetweenCoins;

    //generate 3 coins on the platform 
    public void SpawnCoins(Vector3 startPosition)
    {
        GenerateCoin(startPosition);
        GenerateCoin(new Vector3(startPosition.x - distanceBetweenCoins, startPosition.y, startPosition.z));
        GenerateCoin(new Vector3(startPosition.x + distanceBetweenCoins, startPosition.y, startPosition.z));
    }

    //generate individual coins
    void GenerateCoin(Vector3 position)
    {
        GameObject coin = coinPool.GetPooledObject();
        coin.transform.position = position;
        coin.SetActive(true);
    }
}
