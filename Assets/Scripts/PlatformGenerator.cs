using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    #region Variables

    public GameObject platform;
    public Transform generationPoint;
    private float distanceBetween;

    public float minDistanceBetween;
    public float maxDistanceBetween;

    private int platformSelector;
    private float[] platformWidths;

    public ObjectPooler[] platformPools;

    private float minHeight;
    public Transform maxHeightPoint;
    private float maxHeight;
    public float maxHeightChange;
    private float heightChange;

    private CoinGenerator _coinGenerator;
    public float randomCoinThreshold;

    public float randomSpikeThreshold;
    public ObjectPooler spikePool;

    public float powerupHeight;
    public ObjectPooler powerupPool;
    public float randomPowerupThreshold;

    #endregion

    #region Default Methods

    // Start is called before the first frame update
    void Start()
    {
        //get platform widths from each of available platform prefabs
        platformWidths = new float[platformPools.Length];
        for (int i = 0; i < platformPools.Length; i++)
        {
            platformWidths[i] = platformPools[i].pooledObject.GetComponent<BoxCollider2D>().size.x;
        }

        //min and max position of the platform on y-axis
        minHeight = transform.position.y;
        maxHeight = maxHeightPoint.position.y;

        _coinGenerator = FindObjectOfType<CoinGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        //if player is behind the generation point
        if (transform.position.x < generationPoint.position.x)
        {
            
            CreateRandomPlatform();           
            SpawnCoinsRandomly();
            SpawnSpikesRandomly();
            if (Random.Range(0f, 100f) < randomPowerupThreshold)
            {
                GameObject newPowerup = powerupPool.GetPooledObject();
                newPowerup.transform.position = transform.position + new Vector3(distanceBetween / 2f, Random.Range(1f, powerupHeight), 0f);
                newPowerup.SetActive(true);
            }
            MovePlatformGenerator();                 
        }
    }

    #endregion

    #region Custom Methods

    private void MovePlatformGenerator()
    {
        //avoid overlapping platforms (move platform generator to the end of the platform)
        transform.position = new Vector3(transform.position.x + (platformWidths[platformSelector] / 2), transform.position.y, transform.position.z);
    }

    private void SpawnSpikesRandomly()
    {
        //randomly spawn spikes on top of the platform
        if (Random.Range(0f, 100f) < randomSpikeThreshold)
        {
            GameObject newSpike = spikePool.GetPooledObject();
            //pick random position of spikes on x-axis on the platform
            float spikeXPosition = Random.Range(-platformWidths[platformSelector] / 2f + 1f, platformWidths[platformSelector] / 2f - 1f);
            //move spike's position half of the platform height up (platform height is 1f)
            Vector3 spikePositionChange = new Vector3(spikeXPosition, 0.5f, 0f);
            newSpike.transform.position = transform.position + spikePositionChange;
            newSpike.transform.rotation = transform.rotation;
            newSpike.SetActive(true);
            _coinGenerator.SpawnCoins(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z));
        }
    }

    private void SpawnCoinsRandomly()
    {
        //randomly spawn coins above the platform
        if (Random.Range(0f, 100f) < randomCoinThreshold)
        {
            _coinGenerator.SpawnCoins(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z));
        }
    }

    private void CreateRandomPlatform()
    {
        //pick random platform size from the array of prefabs
        platformSelector = Random.Range(0, platformPools.Length);
        //pick random position without overlapping
        PickRandomPlatformPosition(platformSelector);
        //create new platform 
        GameObject newPlatform = platformPools[platformSelector].GetPooledObject();
        newPlatform.transform.position = transform.position;
        newPlatform.transform.rotation = transform.rotation;
        newPlatform.SetActive(true);
    }

    private void PickRandomPlatformPosition(int platformSelector)
    {
        //pick random distance between platforms
        distanceBetween = Random.Range(minDistanceBetween, maxDistanceBetween);
        //change current position of the platform generator by random value
        heightChange = transform.position.y + Random.Range(-maxHeightChange, maxHeightChange);
        //force height of the platform to be within screen height
        heightChange = Mathf.Clamp(heightChange, minHeight, maxHeight);
        //set position for a new platform
        transform.position = new Vector3(transform.position.x + (platformWidths[platformSelector] / 2) + distanceBetween, heightChange, transform.position.z);
    }

    #endregion
}
