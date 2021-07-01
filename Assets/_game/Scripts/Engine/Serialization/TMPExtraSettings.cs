using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace RomenoCompany
{
    // TextMeshPro Styles
    [CreateAssetMenu(
        fileName = "TMPExtraSettings", 
        menuName = "RC/TMPExtraSettings", 
        order = 59)]
    public class TMPExtraSettings : SerializedScriptableObject
    {
        public Dictionary<string, TMPStyle> tmpStyles;
    }

    [Serializable]
    public class TMPStyle
    {
        // STATIC
        public TMP_FontAsset font;
        public float wordSpacing;
        
        // RUNTIME
        public bool useRelativeFontSize = false;
        [ShowIf("useRelativeFontSize")]
        public float relativeFontSize = 1.0f;
        
        public bool setMargin = false;
        [ShowIf("setMargin")]
        public bool useDefaultMargin = true;
        [ShowIf("setMargin")] [ShowIf("useDefaultMargin")]
        public float defaultMarginsMultiplier = 1.0f;
        [ShowIf("setMargin")] [HideIf("useDefaultMargin")]
        public Vector4 margin;

        public void ApplyStatic(TMP_Text textComp)
        {
            textComp.font = font;
            textComp.wordSpacing = wordSpacing;
        }
        
        public void ApplyRuntime(TMP_Text textComp)
        {
            if (useRelativeFontSize) textComp.fontSize = LayoutManager.Instance.esw * relativeFontSize;
            if (setMargin)
            {
                if (useDefaultMargin) textComp.margin = LayoutManager.Instance.defaultMargins * defaultMarginsMultiplier;
                else textComp.margin = margin;
            }
        }
    }
}

