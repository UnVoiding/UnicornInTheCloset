using System;
using UnityEngine;

namespace RomenoCompany
{
    public class HPOverlayBaseObject : MonoBehaviour, IWorldUI, IDroplet
    {
        [NonSerialized] public GameObject prefab;
        public GameObject Prefab
        {
            get => prefab;
            set => prefab = value;
        }
        public GameObject GameObject => gameObject;
        
        public RectTransform rectTransform;
        
        private readonly Vector2 pooledObjectsPos = new Vector2(0, 10000); 
        
        public virtual void Process(Camera camera, HPOverlay parent)
        {

        }

        public void OnCreate()
        {
            rectTransform.position = pooledObjectsPos;
            // rectTransform.SetParent(UIManager.Instance.hpOverlay.transform);
            rectTransform.localScale = Vector3.one;
        }

        public virtual void OnGetFromPool()
        {
        }

        public virtual void OnReturnToPool()
        {
            // transform.SetParent(Ocean.Instance.transform);
            rectTransform.position = pooledObjectsPos;
        }
    }
}