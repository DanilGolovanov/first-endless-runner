using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerups : MonoBehaviour
{
    public bool doublePoints;
    public bool safeMode;
    public float powerupLength;

    private PowerupManager _powerupManager;

    public Sprite[] powerupSprites;

    // Start is called before the first frame update
    void Start()
    {
        _powerupManager = FindObjectOfType<PowerupManager>();
    }

    private void Awake()
    {
        //pick random powerup
        int powerupSelector = Random.Range(0, 2);
        //activate powerup
        switch (powerupSelector)
        {
            case 0: 
                doublePoints = true;
                break;
            case 1:
                safeMode = true;
                break;
        }
        GetComponent<SpriteRenderer>().sprite = powerupSprites[powerupSelector];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            _powerupManager.ActivatePowerup(doublePoints, safeMode, powerupLength);
        }
        gameObject.SetActive(false);
    }
}
