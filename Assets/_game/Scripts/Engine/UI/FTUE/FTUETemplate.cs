using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RomenoCompany
{
    public enum FTUEType
    {
        NONE = 0,
        COMPANION_SELECTION1 = 10,
        COMPANION_SELECTION2 = 20,
        PROFILE_SCREEN = 30,
        PROFILE_SCREEN_ADVICES = 40,
        PROFILE_SCREEN_LAWYER_ADVICES = 50,
        PROFILE_SCREEN_ITEMS = 60,
        PROFILE_SCREEN_ITEM_INFO = 70,
        UNLOCKED_COMPANIONS = 80,
        CHAT_SCREEN_CHOOSE_ANSWER = 90,
        COMPANION_SELECTION_INFO_TAB = 100,
        DEFAULT = 1000,
    }

    [Serializable]
    public class FTUETemplate
    {
        public FTUEType fTUEType = FTUEType.DEFAULT;
        
        public Color overlayColor = new Color(0.0f, 0.0f, 0.0f, 0.588f);
        
        public bool showTap = true;
        [ShowIf("showTap")]
        public Vector2 tapOffset;
        [ShowIf("showTap")]
        public Vector3 circlesScale = Vector3.one;
        [ShowIf("showTap")]
        public Vector2 fingerOffset;
        
        public bool showTooltip = true;
        [ShowIf("showTooltip")]
        [TextArea(1, 5)] public string tooltipText;
        [ShowIf("showTooltip")]
        public Vector2 tooltipOffset;
    }
}