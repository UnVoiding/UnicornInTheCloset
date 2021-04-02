using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RomenoCompany
{
    [RequireComponent(typeof(Toggle))]
    public class TabToggle : MonoBehaviour
    {
        [                                                           FoldoutGroup("References")] 
        public Tab tab;
        [                                           SerializeField, FoldoutGroup("References")] 
        protected TMP_Text titleText;
        [                                           SerializeField, FoldoutGroup("References")] 
        protected Image notification;

        [                                           NonSerialized, ShowInInspector, ReadOnly, FoldoutGroup("Runtime")] 
        public Toggle toggle;

        
        public void Link(Tab tab, ToggleGroup toggleGroup)
        {
            this.tab = tab;

            toggle.onValueChanged.AddListener(ActivateTab);
            toggle.group = toggleGroup;
            toggle.isOn = false;

            if (notification != null) notification.gameObject.SetActive(false);
        }
        
        public void ActivateTab(bool value)
        {
            tab.Activate(value);
        }

        public void ShowNotification(bool value)
        {
            notification.gameObject.SetActive(value);
        }
        
        public void SetColor(Color color)
        {
            titleText.color = color;
        }
    }
}