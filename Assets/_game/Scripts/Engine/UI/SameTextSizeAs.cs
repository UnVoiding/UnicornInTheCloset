using System;
using TMPro;
using UnityEngine;

namespace RomenoCompany
{
    [ExecuteAlways]
    [RequireComponent(typeof(TMP_Text))]
    public class SameTextSizeAs : MonoBehaviour
    {
        public TMP_Text sameAsText;
        private TMP_Text thisText;

        public void Start()
        {
            Debug.LogError("SameTextSizeAs: Start called");
        }

        private void OnEnable()
        {
            Debug.LogError("SameTextSizeAs: OnEnable called");
            thisText = GetComponent<TMP_Text>();

            TMPro_EventManager.TEXT_CHANGED_EVENT.Add(UpdateFontSize);
        }

        private void OnDisable()
        {
            Debug.LogError("SameTextSizeAs: OnDisable called");
            TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(UpdateFontSize);
        }

        public void OnDestroy()
        {
            Debug.LogError("SameTextSizeAs: OnDestroy called");
        }

        public void UpdateFontSize(UnityEngine.Object obj)
        {
            if (sameAsText != null && obj == sameAsText)
            {
                Debug.LogError("SameTextSizeAs: UpdateFontSize called");

                thisText.fontSize = sameAsText.fontSize;
                thisText.ForceMeshUpdate();
            }

            if (thisText == obj)
            {
                thisText.enableAutoSizing = false;
            }
        }
    }
}