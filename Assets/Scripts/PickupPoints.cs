using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupPoints : MonoBehaviour
{
    public int scoreToGive;
    private ScoreManager _scoreManager;
    private AudioSource _coinSound;

    // Start is called before the first frame update
    void Start()
    {
        _scoreManager = FindObjectOfType<ScoreManager>();
        _coinSound = GameObject.Find("CoinSound").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            _scoreManager.AddScore(scoreToGive);
            gameObject.SetActive(false);
            //make sure to play sound on every coin when on high speed (if there are multiple coins in a row)
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
