using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RomenoCompany
{
    public class Message : MonoBehaviour, IDroplet
    {
        [                                              SerializeField, FoldoutGroup("References")]
        public RectTransform rectTransform;
        [                                              SerializeField, FoldoutGroup("References")]
        public TMP_Text text;
        [                                              SerializeField, FoldoutGroup("References")]
        public Image image;

        public GameObject Prefab { get; set; }
        public GameObject GameObject => gameObject;

        private static readonly Vector3 pooledObjectsPos; 


        public void SetText(string text)
        {
            this.text.text = text;
        }

        public void SetImage(Sprite sprite)
        {
            image.sprite = sprite;
            var f = image.GetComponent<AspectRatioFitter>();
            if (f != null)
            {
                f.aspectRatio = (float)image.sprite.texture.width / (float)image.sprite.texture.height;
            }
        }

        public void OnCreate()
        {
            gameObject.SetActive(false);
            // var l = gameObject.GetComponent<LayoutElement>();
            // l.ignoreLayout = true;
        }

        public void OnGetFromPool()
        {
            var t = gameObject.transform;
            // var l = gameObject.GetComponent<LayoutElement>();
            
            // first set parent then enable
            // t.SetParent(UIManager.Instance.canvasRectTransform, false);
            // t.localScale = Vector3.one;
            // t.localPosition = Vector3.zero;
            // t.anchoredPosition = Vector2.zero;
            // t.rect.Set(0, 0, 1, 1);
            t.SetParent(UIManager.Instance.ChatWidget.allMessageRoot, false);
            t.localScale = Vector3.one;
            t.localPosition = Vector3.zero;
            gameObject.SetActive(true);
            // l.ignoreLayout = false;
        }

        public void OnReturnToPool()
        {
            // first disable then move
            gameObject.SetActive(false);
            gameObject.transform.SetParent(Ocean.Instance.transform);
        }
    }
}
