using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RomenoCompany
{
    [ExecuteAlways]
    class RectTester : MonoBehaviour
    {
        // private Vector3[] corners;
        // private bool firstTime = true;
        // private RectTransform rt;
        // private string[] cornerNames = {"Bottom Left", "Top Left", "Top Right", "Bottom Right"};
        
        private void Update()
        {
            var rt = transform as RectTransform;
            Debug.LogError($"{rt.rect.size}");
        }
        
        //
        // private void Update()
        // {
        //     Debug.LogError($"===== {ReflectionUtility.GetCurrentMethodName()}");
        //     
        //     if (firstTime)
        //     {
        //         rt = GetComponent<RectTransform>();
        //         corners = new Vector3[4];
        //         firstTime = false;
        //     }
        //     
        //     Debug.LogError($"!!!!!!!!!!!!!!!!!!!!!!!! root: {gameObject.name}");
        //     PrintRTInfo(rt);
        //     
        //     int axis = 1;
        //     RectTransform current = rt; 
        //     
        //     while (current != null)
        //     {
        //         current = current.parent as RectTransform;
        //         if (current != null)
        //         {
        //             Debug.LogError($"!!!!!!!!!!!!!!!!!!!!!!!! parent: {current.gameObject.name}");
        //             PrintRTInfo(current);
        //         }
        //     }
        //     
        //     
        //     
        //     for (int i = 0; i < transform.childCount; i++)
        //     {
        //         RectTransform child = transform.GetChild(i) as RectTransform;
        //         if (child == null)
        //         {
        //             continue;
        //         }
        //         
        //         // child.pivot
        //         float childPreferredSize = LayoutUtility.GetPreferredSize(child, axis);
        //         Debug.LogError($"!!!!!! child {child.gameObject.name} size by axis {axis}: {childPreferredSize}");
        //     
        //     // for (int i = 0; i < transform.childCount; i++)
        //     // {
        //     //     RectTransform child = transform.GetChild(i) as RectTransform;
        //     //     if (child == null)
        //     //     {
        //     //         continue;
        //     //     }
        //     //     
        //     //     // child.pivot
        //     //     float childPreferredSize = LayoutUtility.GetPreferredSize(child, axis);
        //     //     Debug.LogError($"!!!!!! child {child.gameObject.name} size by axis {axis}: {childPreferredSize}");
        //         
        //         
        //         
        //         
        //         // Debug.LogError($"!!!!!! child {child.gameObject.name} size by axis {axis}: {childPreferredSize}");
        //         
        //         
        //         // flexible = LayoutUtility.GetFlexibleSize(child, axis);
        //         //
        //         // float min, preferred, flexible;
        //         // LayoutUtility.GetChildSizes(child, axis, controlSize, childForceExpandSize, out min, out preferred, out flexible);
        //     
        //         // if (useScale)
        //         // {
        //         //     float scaleFactor = child.localScale[axis];
        //         //     min *= scaleFactor;
        //         //     preferred *= scaleFactor;
        //         //     flexible *= scaleFactor;
        //         // }
        //         //
        //         // if (alongOtherAxis)
        //         // {
        //         //     totalMin = Mathf.Max(min + combinedPadding, totalMin);
        //         //     totalPreferred = Mathf.Max(preferred + combinedPadding, totalPreferred);
        //         //     totalFlexible = Mathf.Max(flexible, totalFlexible);
        //         // }
        //         // else
        //         // {
        //         //     totalMin += min + spacing;
        //         //     totalPreferred += preferred + spacing;
        //         //
        //         //     // Increment flexible size with element's flexible size.
        //         //     totalFlexible += flexible;
        //         // }
        //     }
        // }
        //
        // private void PrintRTInfo(RectTransform rectTrans)
        // {
        //     Debug.LogError($"!!!!!!! Pivot {rectTrans.pivot}");
        //     Debug.LogError($"!!!!!!! AnchoredPosition {rectTrans.anchoredPosition}");
        //     Debug.LogError($"!!!!!!! AnchorMax {rectTrans.anchorMax}");
        //     Debug.LogError($"!!!!!!! AnchorMin {rectTrans.anchorMin}");
        //     Debug.LogError($"!!!!!!! offsetMin {rectTrans.offsetMin}");
        //     Debug.LogError($"!!!!!!! OffsetMax {rectTrans.offsetMax}");
        //     Debug.LogError($"!!!!!!! Rect {rectTrans.rect}");
        //
        //     rectTrans.GetLocalCorners(corners);
        //     for (int i = 0; i < corners.Length; i++)
        //     {
        //         Debug.LogError($"!!!!!!! GetLocalCorners: {cornerNames[i]} {corners[i]}");
        //     }
        //
        //     rectTrans.GetWorldCorners(corners);
        //     for (int i = 0; i < corners.Length; i++)
        //     {
        //         Debug.LogError($"!!!!!!! GetWorldCorners: {cornerNames[i]} {corners[i]}");
        //     }
        // }
        //
        // protected virtual void OnRectTransformDimensionsChange()
        // {
        //     Debug.LogError($"===== {ReflectionUtility.GetCurrentMethodName()}");
        // }
        //
        // protected virtual void OnBeforeTransformParentChanged()
        // {
        //     Debug.LogError($"===== {ReflectionUtility.GetCurrentMethodName()}");
        // }
        //
        // protected virtual void OnTransformParentChanged()
        // {
        //     Debug.LogError($"===== {ReflectionUtility.GetCurrentMethodName()}");
        // }
        //
        // protected virtual void OnDidApplyAnimationProperties()
        // {
        //     Debug.LogError($"===== {ReflectionUtility.GetCurrentMethodName()}");
        //     
        // }
        //
        // protected virtual void OnCanvasGroupChanged()
        // {
        //     Debug.LogError($"===== {ReflectionUtility.GetCurrentMethodName()}");
        // }
        //
        // /// <summary>
        // /// Called when the state of the parent Canvas is changed.
        // /// </summary>
        // protected virtual void OnCanvasHierarchyChanged()
        // {
        //     Debug.LogError($"===== {ReflectionUtility.GetCurrentMethodName()}");
        // }
    }
}

