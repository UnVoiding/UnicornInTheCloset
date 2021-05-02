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
        [                          NonSerialized, ShowInInspector, ReadOnly, FoldoutGroup("Runtime")] 
        public TabController controller;

        [                                               NonSerialized] 
        public Action<bool> OnActivateTab;

        
        public virtual void Init(TabToggle tabToggle, TabController controller)
        {
            this.controller = controller;
            this.tabToggle = tabToggle;
        }

        public void Activate(bool activate)
        {
            gameObject.SetActive(activate);
            
            OnActivate(activate);
        }

        public virtual void OnShow()
        {
            
        }
        
        public virtual void OnHide()
        {
            
        }
        
        protected virtual void OnActivate(bool activate)
        {
            OnActivateTab?.Invoke(activate);
        }
    }
}