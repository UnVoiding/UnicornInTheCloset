using System;
using System.Collections.Generic;
using UnityEngine;

namespace RomenoCompany
{
    public class Pool : StrictSingleton<Pool>
    {
        private readonly Dictionary<GameObject, List<IPoolObject>> _pool = new Dictionary<GameObject, List<IPoolObject>>();
        
        protected override void Setup()
        {
            // DontDestroyOnLoad(gameObject);
        }

        public void InitPoolItem(GameObject prefab, int count)
        {
            if (!_pool.ContainsKey(prefab))
            {
                _pool.Add(prefab, new List<IPoolObject>());
            }

            for (int i = 0; i < count; ++i)
            {
                var instance = Instantiate(prefab);
                var poolObject = instance.GetComponent<IPoolObject>() ?? instance.AddComponent<SimplePoolObject>();
                poolObject.Prefab = prefab;
                Return(poolObject);
            }
        }

        public GameObject Get(GameObject prefab)
        {
            if (!_pool.ContainsKey(prefab))
            {
                InitPoolItem(prefab, 1);
            }

            var list = _pool[prefab];
            if (list.Count == 0)
            {
                InitPoolItem(prefab, 1);
            }

            var poolItem = list[0];
            list.RemoveAt(0);
            GameObject go = poolItem.transform.gameObject;
            go.SetActive(true);
            return go;
        }

        public GameObject Get(GameObject prefab, Transform newParent)
        {
            var result = Get(prefab);
            result.transform.SetParent(newParent);
            return result;
        }

        public GameObject Get(GameObject prefab, Transform newParent, Vector3 position, Quaternion rotation)
        {
            var result = Get(prefab, newParent);
            result.transform.position = position;
            result.transform.rotation = rotation;
            return result;
        }

        public void Return(IPoolObject poolObject)
        {
            if (_pool.ContainsKey(poolObject.Prefab))
            {
                var list = _pool[poolObject.Prefab];
                poolObject.transform.SetParent(this.transform);
                poolObject.transform.gameObject.SetActive(false);
                list.Add(poolObject);
                return;
            }
            
            // Debug.LogError($"There are no prefab {poolObject.Prefab.name} in Pool!" );
        }
    }
}
