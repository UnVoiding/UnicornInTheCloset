using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RomenoCompany
{
    // [AddComponentMenu("Layout/Content Size Fitter", 141)]
    // [ExecuteAlways]
    // [RequireComponent(typeof(RectTransform))]
    // /// <summary>
    // /// Resizes a RectTransform to fit the size of its content and contents of its children.
    // /// </summary>
    // /// <remarks>
    // /// The ContentSizeFitter can be used on GameObjects that have one or more ILayoutElement components, such as Text, Image, HorizontalLayoutGroup, VerticalLayoutGroup, and GridLayoutGroup.
    // /// </remarks>
    // public class BetterContentSizeFitter : UIBehaviour, ILayoutSelfController
    // {
    //     /// <summary>
    //     /// The size fit modes avaliable to use.
    //     /// </summary>
    //     public enum FitMode
    //     {
    //         /// <summary>
    //         /// Don't perform any resizing.
    //         /// </summary>
    //         Unconstrained,
    //         /// <summary>
    //         /// Resize to the minimum size of the content.
    //         /// </summary>
    //         MinSize,
    //         /// <summary>
    //         /// Resize to the preferred size of the content.
    //         /// </summary>
    //         PreferredSize
    //     }
    //
    //     [SerializeField] protected FitMode m_HorizontalFit = FitMode.Unconstrained;
    //
    //     /// <summary>
    //     /// The fit mode to use to determine the width.
    //     /// </summary>
    //     public FitMode horizontalFit { get { return m_HorizontalFit; } set { if (SetStruct(ref m_HorizontalFit, value)) SetDirty(); } }
    //
    //     [SerializeField] protected FitMode m_VerticalFit = FitMode.Unconstrained;
    //
    //     /// <summary>
    //     /// The fit mode to use to determine the height.
    //     /// </summary>
    //     public FitMode verticalFit { get { return m_VerticalFit; } set { if (SetStruct(ref m_VerticalFit, value)) SetDirty(); } }
    //
    //     [System.NonSerialized] private RectTransform m_Rect;
    //     private RectTransform rectTransform
    //     {
    //         get
    //         {
    //             if (m_Rect == null)
    //                 m_Rect = GetComponent<RectTransform>();
    //             return m_Rect;
    //         }
    //     }
    //
    //     private DrivenRectTransformTracker m_Tracker;
    //
    //     protected BetterContentSizeFitter()
    //     {}
    //
    //     protected override void OnEnable()
    //     {
    //         base.OnEnable();
    //         SetDirty();
    //     }
    //
    //     protected override void OnDisable()
    //     {
    //         m_Tracker.Clear();
    //         LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
    //         base.OnDisable();
    //     }
    //
    //     protected override void OnRectTransformDimensionsChange()
    //     {
    //         SetDirty();
    //     }
    //
    //     private void HandleSelfFittingAlongAxis(int axis)
    //     {
    //         FitMode fitting = (axis == 0 ? horizontalFit : verticalFit);
    //         if (fitting == FitMode.Unconstrained)
    //         {
    //             // Keep a reference to the tracked transform, but don't control its properties:
    //             m_Tracker.Add(this, rectTransform, DrivenTransformProperties.None);
    //             return;
    //         }
    //
    //         m_Tracker.Add(this, rectTransform, (axis == 0 ? DrivenTransformProperties.SizeDeltaX : DrivenTransformProperties.SizeDeltaY));
    //
    //         float selfSizeAlongAxis = 0;
    //         // Set size to min or preferred size
    //         if (fitting == FitMode.MinSize)
    //         {
    //             selfSizeAlongAxis = LayoutUtility.GetMinSize(m_Rect, axis);
    //         }
    //         else
    //         {
    //             selfSizeAlongAxis = LayoutUtility.GetPreferredSize(m_Rect, axis);
    //             Vector2 childrenBoundingRect = GetEnvelopingChildrenPreferredSize(axis);
    //         }
    //         
    //         rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis)axis, selfSizeAlongAxis);
    //     }
    //
    //     private Vector2 GetEnvelopingChildrenPreferredSize(int axis)
    //     {
    //         for (int i = 0; i < transform.childCount; i++)
    //         {
    //             RectTransform child = transform.GetChild(i) as RectTransform;
    //             if (child == null)
    //             {
    //                 continue;
    //             }
    //             
    //             child.pivot
    //             float childPreferredSize = LayoutUtility.GetPreferredSize(child, axis);
    //             // flexible = LayoutUtility.GetFlexibleSize(child, axis);
    //             //
    //             // float min, preferred, flexible;
    //             // LayoutUtility.GetChildSizes(child, axis, controlSize, childForceExpandSize, out min, out preferred, out flexible);
    //
    //             if (useScale)
    //             {
    //                 float scaleFactor = child.localScale[axis];
    //                 min *= scaleFactor;
    //                 preferred *= scaleFactor;
    //                 flexible *= scaleFactor;
    //             }
    //
    //             if (alongOtherAxis)
    //             {
    //                 totalMin = Mathf.Max(min + combinedPadding, totalMin);
    //                 totalPreferred = Mathf.Max(preferred + combinedPadding, totalPreferred);
    //                 totalFlexible = Mathf.Max(flexible, totalFlexible);
    //             }
    //             else
    //             {
    //                 totalMin += min + spacing;
    //                 totalPreferred += preferred + spacing;
    //
    //                 // Increment flexible size with element's flexible size.
    //                 totalFlexible += flexible;
    //             }
    //         }
    //     }
    //
    //     /// <summary>
    //     /// Calculate and apply the horizontal component of the size to the RectTransform
    //     /// </summary>
    //     public virtual void SetLayoutHorizontal()
    //     {
    //         m_Tracker.Clear();
    //         HandleSelfFittingAlongAxis(0);
    //     }
    //
    //     /// <summary>
    //     /// Calculate and apply the vertical component of the size to the RectTransform
    //     /// </summary>
    //     public virtual void SetLayoutVertical()
    //     {
    //         HandleSelfFittingAlongAxis(1);
    //     }
    //
    //     protected void SetDirty()
    //     {
    //         if (!IsActive())
    //             return;
    //
    //         LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
    //     }
    //     
    //     public static bool SetStruct<T>(ref T currentValue, T newValue) where T : struct
    //     {
    //         if (EqualityComparer<T>.Default.Equals(currentValue, newValue))
    //             return false;
    //
    //         currentValue = newValue;
    //         return true;
    //     }
    //
    // #if UNITY_EDITOR
    //     protected override void OnValidate()
    //     {
    //         SetDirty();
    //     }
    //
    // #endif
    // }
}
