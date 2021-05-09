using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RomenoCompany
{
    [Serializable]
    public class FTUETemplate
    {
        public enum FTUEType
        {
            Default,
        }

        public FTUEType fTUEType = FTUEType.Default;
        public Color overlayColor = new Color(0.0f, 0.0f, 0.0f, 0.392f);
        public bool highlightGo = true;
        public bool showTooltip = true;
        [ShowIf("showTooltip")]
        public string tooltipText;
    }
}