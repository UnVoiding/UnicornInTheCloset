using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RomenoCompany
{
    public class LayoutManager : StrictSingleton<LayoutManager>
    {
        [           Header("Layout Manager"), NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")] 
        public float screenWidth;
        // elementary font size - a unit of font measurement dependant on screen width
        [                                     NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")] 
        public float esw;
        [                                     NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")] 
        public Vector4 defaultMargins; // left, top, right, bottom

        protected override void Setup()
        {
            screenWidth = UIManager.Instance.canvasRectTransform.rect.size.x;
            esw = screenWidth / 22f;
            defaultMargins = new Vector4(esw, 0.5f * esw, esw, 0.5f * esw);
        }
    }
}