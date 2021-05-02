using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RomenoCompany
{
    public class Message : MonoBehaviour
    {
        [                                              SerializeField, FoldoutGroup("References")]
        public RectTransform rectTransform;
        [                                              SerializeField, FoldoutGroup("References")]
        public TMP_Text text;
        [                                              SerializeField, FoldoutGroup("References")]
        public Image image;


        public void SetText(string text)
        {
            this.text.text = text;
        }

        public void SetImage(Sprite sprite)
        {
            image.sprite = sprite;
        }
    }
}
