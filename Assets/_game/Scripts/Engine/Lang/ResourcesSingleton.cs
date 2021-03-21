using Sirenix.OdinInspector;
using UnityEngine;

namespace RomenoCompany
{
    public abstract class ResourcesSingleton<T> : SerializedMonoBehaviour where T : ResourcesSingleton<T>
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Instantiate(Resources.Load<T>(PrefabResourcePath));
                    _instance.gameObject.name = ObjectName;
                    if (_instance != null)
                    {
                        _instance.Setup();
                    } else
                    {
                       throw new UnityException($"service {typeof(T)} not found in resources");
                    }
                }
                return _instance;
            }
        }

        public static string ObjectName
        {
            get
            {
                var fullName = typeof(T).ToString().Split('.');
                return $"[{fullName[fullName.Length - 1]}]";
            }
        }

        public static string PrefabResourcePath
        {
            get
            {
                var fullName = typeof(T).ToString().Split('.');
                return $"Services/[{fullName[fullName.Length-1]}]";
            }
        }

        protected abstract void Setup();
    }
}