using UnityEngine;

namespace RomenoCompany
{
    public class SimplePoolObject : MonoBehaviour, IPoolObject
    {
        public GameObject Prefab { get; set; }
    
        public void ReturnToPool()
        {
            Pool.Instance.Return(this);
        }
    }    
}
