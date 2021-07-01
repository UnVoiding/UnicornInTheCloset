using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Animations;

namespace RomenoCompany.Utility
{
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public class RelativeParentSizeScaler : MonoBehaviour
    {
        public bool lockDimensions;
        public bool preserveAspect;
        [                                       SerializeField, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")]
        private bool oldDimensionsSaved;
        [                                       SerializeField, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")]
        private Vector2 lockedDimensions;
        [                                       SerializeField, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")]
        private Vector2 parentDimensions;
        
        private DrivenRectTransformTracker m_Tracker;
        
        protected void OnDisable()
        {
            m_Tracker.Clear();
        }      
        
        protected void Update()
        {
            var t = transform as RectTransform;
            
            m_Tracker.Clear();
            
            if (oldDimensionsSaved != lockDimensions)
            {
                if (lockDimensions)
                {
                    var parentRT = t.parent as RectTransform;
                    if (parentRT == null)
                    {
                        parentDimensions = Vector2.zero;
                        return;
                    }

                    parentDimensions = parentRT.rect.size;
                    lockedDimensions = t.rect.size;
                }
            }

            oldDimensionsSaved = lockDimensions;
            
            if (lockDimensions)
            {
                var parentRT = t.parent as RectTransform;
                if (parentRT == null)
                {
                    return;
                }
                
                Vector2 newParentSize = parentRT.rect.size; 

                if (!preserveAspect)
                {
                    t.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newParentSize.x / parentDimensions.x * lockedDimensions.x);
                    t.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newParentSize.y / parentDimensions.y * lockedDimensions.y);
                }
                else
                {
                    float xByParentX = newParentSize.x / parentDimensions.x * lockedDimensions.x;
                    float xByAspect = newParentSize.y / parentDimensions.y * lockedDimensions.x;
                    t.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Max(xByParentX, xByAspect));

                    float yByParentY = newParentSize.y / parentDimensions.y * lockedDimensions.y;
                    float yByAspect = newParentSize.x / parentDimensions.x * lockedDimensions.y;
                    t.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Max(yByParentY, yByAspect));

                    t.anchorMin = new Vector2(0.5f, 0.5f);
                    t.anchorMax = new Vector2(0.5f, 0.5f);
                }
                m_Tracker.Add(this, t, DrivenTransformProperties.SizeDelta
                                       // | DrivenTransformProperties.AnchoredPosition
                                       | DrivenTransformProperties.Anchors);
            }
        }
    }
}