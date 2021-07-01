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
        public Image toggleImage;
        [                                                           FoldoutGroup("References")] 
        public Image contentImage;
        [                                                           FoldoutGroup("References")] 
        public TMP_Text spoilerText;
        [                                                           FoldoutGroup("References")] 
        public TMP_Text captionText;
        
        [                                                           FoldoutGroup("Settings")] 
        public bool isOpen = false;
        [                                                           FoldoutGroup("Settings")] 
        public Color openedColor = Color.white;
        [                                                           FoldoutGroup("Settings")] 
        public Color closedColor = Color.white;
        [                                                           FoldoutGroup("Settings")] 
        public event Action onSwitchOnOff;

        
        public virtual void Init()
        {
            isOpen = false;
            openSpoiler.gameObject.SetActive(true);
            closeSpoiler.gameObject.SetActive(false);
            contentRoot.gameObject.SetActive(true);
            contentRoot.ignoreLayout = true;

            spoilerText.fontSize = LayoutManager.Instance.esw;
            spoilerText.margin = LayoutManager.Instance.defaultMargins;

            captionText.fontSize = LayoutManager.Instance.esw;
            captionText.margin = LayoutManager.Instance.defaultMargins;

            openCloseBtn.onClick.AddListener(Switch);
            
            UpdateColor();
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

            UpdateColor();
            
            onSwitchOnOff?.Invoke();
        }

        private void UpdateColor()
        {
            toggleImage.color = isOpen ? openedColor : closedColor;
            contentImage.color = isOpen ? openedColor : closedColor;
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