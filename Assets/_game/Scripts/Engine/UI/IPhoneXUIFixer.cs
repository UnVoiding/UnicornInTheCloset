using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

namespace RomenoCompany
{
    public class IPhoneXUIFixer : MonoBehaviour
    {
        [SerializeField] private RectTransform[] _safeAreasTransform;
	
        private Canvas _canvasComponent;

        private void Start()
        {
            foreach (var safeAreaRect in _safeAreasTransform)
            {
                if(safeAreaRect == null) return;
     
                _canvasComponent = safeAreaRect.GetComponentInParent<Canvas>();

                var safeArea = UnityEngine.Screen.safeArea;

                var anchorMin = safeArea.position;
                Vector2 size = safeArea.size;

                float browHeight = (float)UnityEngine.Screen.height - size.y;

#if UNITY_IOS
            if (browHeight > 0.1f)
            {
                size = new Vector2(size.x, size.y + 42.0f);
            }
#endif

                var anchorMax = safeArea.position + size;
                var pixelRect = _canvasComponent.pixelRect;
                anchorMin.x /= pixelRect.width;
                anchorMin.y /= pixelRect.height;
                anchorMax.x /= pixelRect.width;
                anchorMax.y /= pixelRect.height;

                safeAreaRect.anchorMin = anchorMin;
                safeAreaRect.anchorMax = anchorMax;
            }
        }
    }
}
