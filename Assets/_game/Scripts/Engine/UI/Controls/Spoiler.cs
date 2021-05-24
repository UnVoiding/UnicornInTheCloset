using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RomenoCompany
{
    public class Spoiler : MonoBehaviour
    {
        [                                                           FoldoutGroup("References")] 
        public Button openCloseBtn;
        [                                                           FoldoutGroup("References")] 
        public RectTransform openSpoiler;
        [                                                           FoldoutGroup("References")] 
        public RectTransform closeSpoiler;
        [                                                           FoldoutGroup("References")] 
        public LayoutElement contentRoot;
        [                                                           FoldoutGroup("References")] 
        public TMP_Text spoilerText;
        [                                                           FoldoutGroup("References")] 
        public TMP_Text captionText;
        
        [                                                           FoldoutGroup("Settings")] 
        public bool isOpen = false;
        [                                                           FoldoutGroup("Settings")] 
        public event Action onSwitchOnOff;

        
        public void Init()
        {
            isOpen = false;
            openSpoiler.gameObject.SetActive(true);
            closeSpoiler.gameObject.SetActive(false);
            contentRoot.gameObject.SetActive(true);
            contentRoot.ignoreLayout = true;

            spoilerText.fontSize = LayoutManager.Instance.esw;
            captionText.fontSize = LayoutManager.Instance.esw;

            openCloseBtn.onClick.AddListener(Switch);
        }

        public virtual void Switch()
        {
            isOpen = !isOpen;
            openSpoiler.gameObject.SetActive(!isOpen);
            closeSpoiler.gameObject.SetActive(isOpen);
            contentRoot.ignoreLayout = !isOpen;
            // contentRoot.gameObject.SetActive(isOpen);
            //
            // contentRoot.ForceUpdateRectTransforms();
            
            onSwitchOnOff?.Invoke();
        }

        public void SetCaption(string text)
        {
            captionText.text = text;
        }

        public void SetSpoilerText(string text)
        {
            spoilerText.text = text;
        }
    }
}