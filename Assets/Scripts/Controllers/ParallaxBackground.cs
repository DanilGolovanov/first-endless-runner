using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessRunner.Controllers
{
    public class ParallaxBackground : MonoBehaviour
    {
        #region Variables

        // tranform property of camera to which the background is attached
        private Transform _cameraTransform;
        // length of individual sprite 
        private float spriteLength;
        // start position of the sprite
        private float startPosition;

        [SerializeField, Tooltip("How fast the background is moving (e.g. 1 means that element moves at the same speed as main camera")]
        private float parallaxEffect;

        #endregion

        #region Default Methods
        private void Awake()
        {
            // main camera
            _cameraTransform = Camera.main.transform;
        }

        private void Start()
        {
            // get size of the one of the child component's sprite
            spriteLength = GetComponentInChildren<SpriteRenderer>().bounds.size.x;
            // get the start position
            startPosition = transform.position.x;
        }

        private void LateUpdate()
        {
            // how far object moved relative to the camera
            float distanceRelativeToCamera = (_cameraTransform.position.x * (1 - parallaxEffect));

            CreateParallaxEffect();
            ImitateInfiniteBackground(distanceRelativeToCamera);
        }
        #endregion

        #region Custom Methods
        /// <summary>
        /// Move the background seamlessly for the player.
        /// </summary>
        /// <param name="distanceRelativeToCamera">Distance which the layer the layer travelled relative to the camera.</param>
        private void ImitateInfiniteBackground(float distanceRelativeToCamera)
        {
            // if layer moved to the RIGHT for amount almost equal to width of one of its components
            if (distanceRelativeToCamera > startPosition + spriteLength - 10)
            {
                //move the layer to the right by that amount
                startPosition += spriteLength;
            }
            // if object moved to the LEFT for amount almost equal to his own width
            else if (distanceRelativeToCamera < startPosition - spriteLength)
            {
                //move the layer to the right by that amount
                startPosition -= spriteLength;
            }
        }

        /// <summary>
        /// Move layer at specified speed relative to the camera.
        /// </summary>
        private void CreateParallaxEffect()
        {
            // how far object moved in the world space
            float distance = (_cameraTransform.position.x * parallaxEffect);
            // move the camera
            transform.position = new Vector3(startPosition + distance, transform.position.y, transform.position.z);
        }
        #endregion
    }
}
