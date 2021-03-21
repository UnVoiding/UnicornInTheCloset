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
        [SerializeField] TMP_Text _titleText;
        [SerializeField] Image _notification;

        public Action<bool> OnActivateTab;

        [Header("Runtime data")]
        // [NonSerialized, ShowInInspector] SkinsTab _tab;
        [NonSerialized] public Toggle _toggle;
        
        public void Init(string title, ToggleGroup toggleGroup)
        {
            _toggle = GetComponent<Toggle>();
            _toggle.onValueChanged.AddListener(ActivateTab);
            _toggle.group = toggleGroup;

            // _tab = tab;

            _toggle.isOn = false;
            _notification.gameObject.SetActive(false);
        }

        public void ActivateTab(bool value)
        {
            // _tab.gameObject.SetActive(value);

            OnActivateTab?.Invoke(value);
        }

        public void ShowNotification(bool value)
        {
            _notification.gameObject.SetActive(value);
        }
        
        public void SetColor(Color color)
        {
            _titleText.color = color;
        }
    }
}