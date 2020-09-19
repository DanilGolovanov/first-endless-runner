using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessRunner.ObjectManagement
{
    public class ObjectDestroyer : MonoBehaviour
    {
        [SerializeField, Tooltip("PlatformDestructionPoint object from the scene.")]
        private GameObject _platformDestructionPoint;

        private void Start()
        {
            _platformDestructionPoint = GameObject.Find("PlatformDestructionPoint");
        }

        private void Update()
        {
            // destroy object if platform destruction point passed it (means destruction point is on the right)
            if (transform.position.x < _platformDestructionPoint.transform.position.x)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
