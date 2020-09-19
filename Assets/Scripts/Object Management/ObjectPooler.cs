using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessRunner.ObjectManagement
{
    public class ObjectPooler : MonoBehaviour
    {
        [Tooltip("Prefab which will be reused in the scene.")]
        public GameObject pooledObject;
        [SerializeField, Tooltip("How many copies of the object to be instantiated when starting the game.")]
        private int pooledAmount;

        // list of all objects of the given type present at a given moment in the scene
        private List<GameObject> _pooledObjects;

        private void Start()
        {
            _pooledObjects = new List<GameObject>();

            // instantiate provided number of objects 
            for (int i = 0; i < pooledAmount; i++)
            {
                GameObject obj = (GameObject)Instantiate(pooledObject);
                obj.SetActive(false);
                _pooledObjects.Add(obj);
            }
        }

        /// <summary>
        /// Look for inactive object from the pool of object present in the scene
        /// or create a new inactive object if there are no any.
        /// </summary>
        /// <returns>Either inactive object present in the scene or newly created inactive object.</returns>
        public GameObject GetPooledObject()
        {
            // return inactive object from the list of objects
            for (int i = 0; i < _pooledObjects.Count; i++)
            {
                if (!_pooledObjects[i].activeInHierarchy)
                {
                    return _pooledObjects[i];
                }
            }

            // create inactive object if there are no inactive objects
            GameObject obj = (GameObject)Instantiate(pooledObject);
            obj.SetActive(false);
            _pooledObjects.Add(obj);
            return obj;
        }
    }
}
