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
        public Transform openSpoiler;
        [                                                           FoldoutGroup("References")] 
        public Transform closeSpoiler;
        [                                                           FoldoutGroup("References")] 
        public Transform contentRoot;
        [                                                           FoldoutGroup("References")] 
        public TMP_Text spoilerText;
        [                                                           FoldoutGroup("References")] 
        public TMP_Text captionText;
        
        [                                                           FoldoutGroup("Settings")] 
        public bool isOpen = false;

        public void Init()
        {
            isOpen = false;
            openSpoiler.gameObject.SetActive(true);
            closeSpoiler.gameObject.SetActive(false);
            contentRoot.gameObject.SetActive(false);

            openCloseBtn.onClick.AddListener(Switch);
        }

        public void Switch()
        {
            isOpen = !isOpen;
            openSpoiler.gameObject.SetActive(!isOpen);
            closeSpoiler.gameObject.SetActive(isOpen);
            contentRoot.gameObject.SetActive(isOpen);
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