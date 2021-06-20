using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace RomenoCompany
{
    public class UnlockedCompanion : MonoBehaviour, IDroplet
    {
        [FormerlySerializedAs("name")] [                                       FoldoutGroup("References")]
        public TMP_Text nameText;
        [                                       FoldoutGroup("References")]
        public Image image;
        
        [              NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")]
        public Transform parent;
        
        public GameObject Prefab { get; set; }
        public GameObject GameObject => gameObject;
        
        public void OnCreate()
        {
            gameObject.SetActive(false);
            parent = UIManager.Instance.GetWidget<CompanionUnlockWidget>().contentRoot.transform;
        }

        public void OnGetFromPool()
        {
            // first set parent then enable
            var t = gameObject.transform;
            
            t.SetParent(parent, false);
            t.localScale = Vector3.one;
            t.localPosition = Vector3.zero;
            gameObject.SetActive(true);
        }

        public void OnReturnToPool()
        {
            // first disable then move
            gameObject.SetActive(false);
            gameObject.transform.SetParent(Ocean.Instance.transform);
        }
    }
}
