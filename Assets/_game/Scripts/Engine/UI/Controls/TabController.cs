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
                tab.Link(tabToggle);
                tabToggle.Link(tab, toggleGroup);
                onTabInit(i, tab, tabToggle);
                
                tabs.Add(tab);
                tabToggles.Add(tabToggle);
            }
        }
        
        public void InitPrecreatedTabs()
        {
            for (int i = 0; i < tabs.Count; i++)
            {
                var tab = tabs[i];
                var tabToggle = tabToggles[i];
                
                tab.Link(tabToggle);
                tabToggle.Link(tab, toggleGroup);
            }
        }
    }
}