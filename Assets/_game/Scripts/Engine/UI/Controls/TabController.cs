using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace RomenoCompany
{
    public class TabController : MonoBehaviour
    {
        [                                           SerializeField, FoldoutGroup("References")] 
        public GameObject tabTogglesRoot;
        [                                           SerializeField, FoldoutGroup("References")] 
        public GameObject tabsRoot;
        [                                           SerializeField, FoldoutGroup("References")] 
        public Tab tabPfb;
        [                                           SerializeField, FoldoutGroup("References")] 
        public TabToggle tabTogglePfb;
        [                                           SerializeField, FoldoutGroup("References")] 
        public ToggleGroup toggleGroup;
        [                                           SerializeField, FoldoutGroup("References")] 
        public List<Tab> tabs;
        [                                           SerializeField, FoldoutGroup("References")] 
        public List<TabToggle> tabToggles;
        
        
        public void CreateTabs(int tabCount, Action<int, Tab, TabToggle> onTabInit)
        {
            tabs = new List<Tab>(tabCount);
            tabToggles = new List<TabToggle>(tabCount);

            for (int i = 0; i < tabCount; i++)
            {
                var tab = Instantiate(tabPfb, tabsRoot.transform);
                var tabToggle = Instantiate(tabTogglePfb, tabsRoot.transform);
                tab.Init(tabToggle, this);
                tabToggle.Init(tab, this);
                onTabInit(i, tab, tabToggle);
                
                tabs.Add(tab);
                tabToggles.Add(tabToggle);
            }
            
            ActivateTab(1);
        }

        public void OnShow()
        {
            foreach (var t in tabs)
            {
                t.OnShow();
            }

            foreach (var tt in tabToggles)
            {
                tt.OnShow();
            }
        }

        public void OnHide()
        {
            foreach (var t in tabs)
            {
                t.OnHide();
            }

            foreach (var tt in tabToggles)
            {
                tt.OnHide();
            }
        }

        public void InitPrecreatedTabs()
        {
            toggleGroup.allowSwitchOff = false;
            
            for (int i = 0; i < tabs.Count; i++)
            {
                var tab = tabs[i];
                var tabToggle = tabToggles[i];
                
                tab.Init(tabToggle, this);
                tabToggle.Init(tab, this);
                tab.Activate(false);
            }

            for (int i = 0; i < tabs.Count; i++)
            {
                var tabToggle = tabToggles[i];

                tabToggle.toggle.group = toggleGroup;
            }

            ActivateTab(0);
        }

        public void ShowTab(int i, bool show)
        {
            tabToggles[i].gameObject.SetActive(show);
        }

        public void ActivateTab(int i)
        {
            tabToggles[i].toggle.isOn = true;
        }
    }
}