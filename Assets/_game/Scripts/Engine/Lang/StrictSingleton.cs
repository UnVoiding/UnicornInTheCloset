using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RomenoCompany
{
    public abstract class StrictSingleton<T> : SerializedMonoBehaviour where T : StrictSingleton<T>
    {
        private static T instance;
        private static bool initialized = false;

        // you should create instance yourself when using StrictSingleton
        // StrictSingleton is just a hollow shell of a regular Singleton
        // it provides only a fancy way of accessing the Singleton's object
        // using familiar Instance method but you control how and when it is initialized.
        // Alternatively you can use other Init methods below
        // which will create the instance for you. 
        public static void InitInstance(T inst)
        {
            if (initialized) return;

            instance = inst;
            instance.Setup();
            instance.name = $"[{typeof(T).Name}]";

            initialized = true;
            
            Debug.Log($"StrictSingleton {typeof(T).Name} was initialized");
        }

        public static void InitInstanceFromEmptyGameObject()
        {
            if (initialized) return;

            var go = new GameObject();
            InitInstance(go.AddComponent<T>());
        }

        public static void InitInstanceFromExistingGameObject()
        {
            if (initialized) return;

            InitInstance(FindObjectOfType<T>());
        }

        public static void InitInstanceFromPrefab(T prefab)
        {
            if (initialized) return;

            InitInstance(Instantiate(prefab));
        }

        public static void InitInstanceFromResources()
        {
            // TODO
            throw new NotImplementedException();
        }

        protected abstract void Setup();

        public static T Instance
        {
            get
            {
                if (!initialized)
                {
                    Debug.LogError($"{typeof(T).FullName}.Instance called before Init. You should call Init manually for StrictSingleton then access Instance");
                    return null;
                }
                else
                {
                    return instance;
                }
            }
        }

        public static void ResetInstance()
        {
            instance = null;
            initialized = false;
        }

        private void OnDestroy()
        {
            Debug.Log($"StrictSingleton {typeof(T).FullName} was destroyed");

            ResetInstance();
        }
    }
}
