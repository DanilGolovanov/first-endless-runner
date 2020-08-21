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

    public ObjectPooler[] objectPools;

    private float minHeight;
    public Transform maxHeightPoint;
    private float maxHeight;
    public float maxHeightChange;
    private float heightChange;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        platformWidths = new float[objectPools.Length];
        for (int i = 0; i < objectPools.Length; i++)
        {
            platformWidths[i] = objectPools[i].pooledObject.GetComponent<BoxCollider2D>().size.x; 
        }

        minHeight = transform.position.y;
        maxHeight = maxHeightPoint.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < generationPoint.position.x)
        {
            //pick random distance between platforms
            distanceBetween = Random.Range(minDistanceBetween, maxDistanceBetween);

            //pick random platform size from the array of prefabs
            platformSelector = Random.Range(0, objectPools.Length);

            //change current position of the platform generator by random value
            heightChange = transform.position.y + Random.Range(-maxHeightChange, maxHeightChange);

            //force height of the platform to be within screen height
            heightChange = Mathf.Clamp(heightChange, minHeight, maxHeight);

            //set position for a new platform
            transform.position = new Vector3(transform.position.x + (platformWidths[platformSelector] / 2) + distanceBetween, heightChange, transform.position.z);

            //create new platform 
            GameObject newPlatform = objectPools[platformSelector].GetPooledObject();

            newPlatform.transform.position = transform.position;
            newPlatform.transform.rotation = transform.rotation;
            newPlatform.SetActive(true);

            //avoid overlapping platforms
            transform.position = new Vector3(transform.position.x + (platformWidths[platformSelector] / 2), transform.position.y, transform.position.z);
        }
    }
}
