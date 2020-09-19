using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessRunner.Menus
{
    public class MainMenuCamera : MonoBehaviour
    {
        [SerializeField, Tooltip("MainMenuBackground object with PlayerController script attached.")]
        private MainMenuBackground _player;

        private Vector3 lastPlayerPosition;
        private float distanceToMove;

        private void Start()
        {
            // get player
            _player = FindObjectOfType<MainMenuBackground>();
            // get initial player position
            lastPlayerPosition = _player.transform.position;
        }

        private void Update()
        {
            MoveCameraWhenPlayerMoves();
        }

        /// <summary>
        /// Move camera on x-axis at the same speed as player runs.
        /// </summary>
        private void MoveCameraWhenPlayerMoves()
        {
            distanceToMove = _player.transform.position.x - lastPlayerPosition.x;
            transform.position = new Vector3(transform.position.x + distanceToMove, transform.position.y, transform.position.z);
            lastPlayerPosition = _player.transform.position;
        }
    }
}
