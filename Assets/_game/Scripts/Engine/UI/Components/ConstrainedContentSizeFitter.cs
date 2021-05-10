using System;
using System.Collections.Generic;
using RomenoCompany;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
    [AddComponentMenu("Layout/Constrained Content Size Fitter", 142)]
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    /// <summary>
    /// Resizes a RectTransform to fit the size of its content.
    /// </summary>
    /// <remarks>
    /// The ContentSizeFitter can be used on GameObjects that have one or more ILayoutElement components, such as Text, Image, HorizontalLayoutGroup, VerticalLayoutGroup, and GridLayoutGroup.
    /// </remarks>
    public class ConstrainedContentSizeFitter : UIBehaviour, ILayoutSelfController
    {
        /// <summary>
        /// The size fit modes avaliable to use.
        /// </summary>
        public enum FitMode
        {
            /// <summary>
            /// Don't perform any resizing.
            /// </summary>
            Unconstrained,
            /// <summary>
            /// Resize to the minimum size of the content.
            /// </summary>
            MinSize,
            /// <summary>
            /// Resize to the preferred size of the content.
            /// </summary>
            PreferredSize
        }

        [SerializeField] protected FitMode m_HorizontalFit = FitMode.Unconstrained;

        /// <summary>
        /// The fit mode to use to determine the width.
        /// </summary>
        public FitMode horizontalFit { get { return m_HorizontalFit; } set { if (SetStruct(ref m_HorizontalFit, value)) SetDirty(); } }

        [SerializeField] protected FitMode m_VerticalFit = FitMode.Unconstrained;
        [FormerlySerializedAs("m_VerticalConstrain")] [SerializeField] protected bool m_VerticalConstrainToParentSize = false;
        [FormerlySerializedAs("m_HorizontalConstrain")] [SerializeField] protected bool m_HorizontalConstrainToParentSize = false;
        [SerializeField] protected bool m_VerticalConstrainToCanvasSize = false;
        [ShowIf("m_VerticalConstrainToCanvasSize"), SerializeField] protected float m_VerticalCanvasSizeFraction = 0.5f;
        [SerializeField] protected bool m_VerticalConstrainToConstant = false;
        [ShowIf("m_VerticalConstrainToConstant"), SerializeField] protected float m_VerticalConstant = 200f;

        private RectTransform rootCanvasRectTransform;
        private RectTransform RootCanvasRectTransform
        {
            get
            {
                if (rootCanvasRectTransform == null)
                {
                    var q = GetComponentInParent<Canvas>();
                    if (q == null)
                    {
                        return null;
                    }
                    else
                    {
                        rootCanvasRectTransform = q.rootCanvas.GetComponent<RectTransform>();
                    }
                }

                return rootCanvasRectTransform;
            }
        }

        /// <summary>
        /// The fit mode to use to determine the height.
        /// </summary>
        public FitMode verticalFit { get { return m_VerticalFit; } set { if (SetStruct(ref m_VerticalFit, value)) SetDirty(); } }

        [System.NonSerialized] private RectTransform m_Rect;
        private RectTransform rectTransform
        {
            get
            {
                if (m_Rect == null)
                    m_Rect = GetComponent<RectTransform>();
                return m_Rect;
            }
        }

        private DrivenRectTransformTracker m_Tracker;

        protected ConstrainedContentSizeFitter()
        {}

        protected override void OnEnable()
        {
            base.OnEnable();
            SetDirty();
        }

        protected override void OnDisable()
        {
            m_Tracker.Clear();
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
            base.OnDisable();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            SetDirty();
        }
        
        public static bool SetStruct<T>(ref T currentValue, T newValue) where T : struct
        {
            if (EqualityComparer<T>.Default.Equals(currentValue, newValue))
                return false;

            currentValue = newValue;
            return true;
        }

        private void HandleSelfFittingAlongAxis(int axis)
        {
            FitMode fitting = (axis == 0 ? horizontalFit : verticalFit);
            if (fitting == FitMode.Unconstrained)
            {
                // Keep a reference to the tracked transform, but don't control its properties:
                m_Tracker.Add(this, rectTransform, DrivenTransformProperties.None);
                return;
            }

            m_Tracker.Add(this, rectTransform, (axis == 0 ? DrivenTransformProperties.SizeDeltaX : DrivenTransformProperties.SizeDeltaY));

            // Set size to min or preferred size
            if (fitting == FitMode.MinSize)
                rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis)axis, LayoutUtility.GetMinSize(m_Rect, axis));
            else
            {
                float toBeSize = LayoutUtility.GetPreferredSize(m_Rect, axis);
                var p = m_Rect.parent as RectTransform;
                if (!ReferenceEquals(p, null))
                {
                    if (axis == 0 && m_HorizontalConstrainToParentSize)
                    {
                        toBeSize = Mathf.Clamp(toBeSize, 0, p.rect.size.x);
                    }

                    if (axis == 1)
                    {
                        if (m_VerticalConstrainToParentSize)
                        {
                            toBeSize = Mathf.Clamp(toBeSize, 0, p.rect.size.y);
                        }

                        if (m_VerticalConstrainToCanvasSize)
                        {
                            float constrt = RootCanvasRectTransform.rect.size.y *
                                            m_VerticalCanvasSizeFraction;
                            if (m_VerticalConstrainToConstant)
                            {
                                constrt += m_VerticalConstant;
                            }
                            
                            toBeSize = Mathf.Clamp(toBeSize, 0, constrt);
                        }
                    }
                }
                rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis)axis, toBeSize);
            }
        }

        public void Test()
        {
            
        }

        /// <summary>
        /// Calculate and apply the horizontal component of the size to the RectTransform
        /// </summary>
        public virtual void SetLayoutHorizontal()
        {
            m_Tracker.Clear();
            HandleSelfFittingAlongAxis(0);
        }

        /// <summary>
        /// Calculate and apply the vertical component of the size to the RectTransform
        /// </summary>
        public virtual void SetLayoutVertical()
        {
            HandleSelfFittingAlongAxis(1);
        }

        protected void SetDirty()
        {
            if (!IsActive())
                return;

            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        }

    #if UNITY_EDITOR
        protected override void OnValidate()
        {
            SetDirty();
        }

    #endif
    }
}
