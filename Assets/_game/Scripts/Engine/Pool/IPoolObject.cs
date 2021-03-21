using UnityEngine;

namespace RomenoCompany
{
    public interface IPoolObject
    {
        GameObject Prefab { get; set; }
        Transform transform { get; }
        void ReturnToPool();
    }

// pooled object
    public interface IDroplet
    {
        GameObject Prefab { get; set; }
        GameObject GameObject { get; }
        void OnCreate();
        void OnGetFromPool();
        void OnReturnToPool();
    }    
}
