using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RomenoCompany
{
    public abstract class Singleton<T> : SerializedMonoBehaviour where T : Singleton<T>
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance != null)
                    {
                        _instance.Setup();
                    } 
                    else
                    {
                        var go = new GameObject($"[{typeof(T)}]");
                        _instance = go.AddComponent<T>();
                    }
                }
                return _instance;
            }
        }

        public static void ResetInstance()
        {
            _instance = null;
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
        
        protected void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                Setup();
            }
        }

        protected abstract void Setup();
    }    
}
