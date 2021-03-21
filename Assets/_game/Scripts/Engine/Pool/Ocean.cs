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
        }

        public void Allocate(GameObject prefab, int size)
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
        
        public IDroplet Get(GameObject prefab)
        {
            var pool = GetPool(prefab);
            var droplet = GetDroplet(pool, prefab);
            droplet.OnGetFromPool();
            return droplet;
        }

        public void Return(IDroplet poolObject)
        {
            poolObject.OnReturnToPool();

            var pool = GetPool(poolObject.Prefab);
            pool.Add(poolObject);
        }

        private List<IDroplet> GetPool(GameObject prefab)
        {
            if (!ocean.ContainsKey(prefab))
            {
                ocean[prefab] = new List<IDroplet>();
            }

            return ocean[prefab];
        }

        private void CreateDroplet(List<IDroplet> pool, GameObject prefab)
        {
            var instance = Instantiate(prefab, transform);
            var poolObject = instance.GetComponent<IDroplet>();
            poolObject.Prefab = prefab;
            
            pool.Add(poolObject);
            
            poolObject.OnCreate();
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