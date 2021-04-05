using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace RomenoCompany
{
    public class Tab : MonoBehaviour
    {
        [                                                           FoldoutGroup("References")] 
        public TabToggle tabToggle;
        [                                                           FoldoutGroup("References")] 
        public Transform contentRoot;

        [                                               NonSerialized] 
        public Action<bool> OnActivateTab;

        
        public void Link(TabToggle tabToggle)
        {
            this.tabToggle = tabToggle;
        }

        public void Activate(bool activate)
        {
            gameObject.SetActive(activate);

            OnActivateTab?.Invoke(activate);
        }

        public virtual void OnShow()
        {
            
        }
        
        public virtual void OnHide()
        {
            
        }
    }
}