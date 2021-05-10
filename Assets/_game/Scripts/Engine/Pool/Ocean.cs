using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace RomenoCompany
{
    // the idea is that naming as abstraction is more recognizable
    // after a little bit of learning curve
    // Ocean (fancy name for Pool Manager) -> Pool -> Droplet (fancy name for pooled object)
    public class Ocean : StrictSingleton<Ocean>
    {
        public Dictionary<GameObject, List<IDroplet>> ocean;
        
        protected override void Setup()
        {
            ocean = new Dictionary<GameObject, List<IDroplet>>();
            DontDestroyOnLoad(gameObject);
        }

        public List<IDroplet> CreatePool(GameObject prefab, int size = 5)
        {
            var pool = new List<IDroplet>(size);
            ocean[prefab] = pool;
            return pool;
        }

        public void PrecreateDroplets(GameObject prefab, int size)
        {
            var pool = GetPool(prefab);

            int toCreate = 0;
            if (pool.Count <= size)
            {
                toCreate = size - pool.Count; 
                for (int i = 0; i < toCreate; i++)
                {
                    CreateDroplet(pool, prefab);
                }
            }
        }
        
        public T Get<T>(T prefab) where T : class, IDroplet
        {
            var pfbGo = prefab.GameObject;
            var pool = GetPool(pfbGo);
            var droplet = GetDroplet(pool, pfbGo);
            droplet.OnGetFromPool();
            return droplet as T;
        }

        public void Return(IDroplet droplet)
        {
            droplet.OnReturnToPool();

            var pool = GetPool(droplet.Prefab);
            pool.Add(droplet);
        }

        private List<IDroplet> GetPool(GameObject prefab)
        {
            if (!ocean.ContainsKey(prefab))
            {
                CreatePool(prefab);
            }

            return ocean[prefab];
        }

        private void CreateDroplet(List<IDroplet> pool, GameObject prefab)
        {
            var instance = Instantiate(prefab, transform);
            var droplet = instance.GetComponent<IDroplet>();
            droplet.Prefab = prefab;
            
            pool.Add(droplet);
            
            droplet.OnCreate();
        }

        private IDroplet GetDroplet(List<IDroplet> pool, GameObject prefab)
        {
            if (pool.Count <= 0)
            {
                CreateDroplet(pool, prefab);
            }

            var droplet = pool[pool.Count - 1];
            
            pool.Remove(droplet);
            
            return droplet;
        }
    }
}