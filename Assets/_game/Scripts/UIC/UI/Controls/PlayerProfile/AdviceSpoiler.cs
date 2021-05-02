using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RomenoCompany
{
    public class AdviceSpoiler : Spoiler
    {
        [                 Header("AdviceSpoiler"), NonSerialized, ShowInInspector, ReadOnly, FoldoutGroup("Runtime")] 
        public AdviceState adviceState;
        
        public override void Switch()
        {
            base.Switch();

            var crrt = contentRoot.GetComponent<RectTransform>();

            Debug.LogError($"AdviceSpoiler: advice name {captionText.text} opened {isOpen} height {crrt.rect.height}");
        }

    }
}

