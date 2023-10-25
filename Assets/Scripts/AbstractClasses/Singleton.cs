using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ariston
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    // Search for an existing instance in the scene
                    instance = FindObjectOfType<T>();

                    // If not found, create a new GameObject and attach the component
                    if (instance == null)
                    {
                        Debug.Log(typeof(T).ToString() + " instance is null, creating a new one");
                        GameObject singletonGO = new GameObject(typeof(T).Name);
                        instance = singletonGO.AddComponent<T>();
                    }
                }

                return instance;
            }
        }

        protected virtual void Awake()
        {
            instance = GetComponent<T>();
        }

        protected virtual void OnDestroy()
        {
            // Clear data or perform cleanup as needed here
        }


    }
}
