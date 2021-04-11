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
        public TMP_FontAsset font;
    }
}

