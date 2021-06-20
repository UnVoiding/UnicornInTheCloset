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
        UNLOCKED_COMPANIONS = 80, // UNUSED
        CHAT_SCREEN_CHOOSE_ANSWER = 90,
        COMPANION_SELECTION_INFO_TAB = 100,
        DEFAULT = 1000,
    }

    [Serializable]
    public class FTUETemplate
    {
        public FTUEType fTUEType = FTUEType.DEFAULT;
        
        public bool highlightObject = true;
        [ShowIf("highlightObject")]
        public FTUEHighlightSettings highlightSettings;

        public bool showHint = true;
        [ShowIf("showHint")]
        public FTUEHintSettings hintSettings;
        
        public bool showTooltip = true;
        [ShowIf("showTooltip")]
        public FTUETooltipSettings tooltipSettings;

        public FTUETemplate DeepClone()
        {
            return new FTUETemplate()
            {
                fTUEType = fTUEType,
                highlightSettings = highlightSettings.DeepClone(),
                hintSettings = hintSettings.DeepClone(),
                tooltipSettings = tooltipSettings.DeepClone(),
            };
        }
    }

    [Serializable]
    public class FTUEHintSettings
    {
        public Vector2 hintAbsoluteOffset;
        public Vector2 hintSizeRelativeOffset;
        
        public bool showFinger = true;
        [ShowIf("showFinger")]
        public Vector2 fingerOffset;
        [ShowIf("showFinger")]
        public Vector3 fingerRotation;
        [ShowIf("showFinger")]
        public float fingerBouncePunch = 1.0f;
        [ShowIf("showFinger")]
        public float fingerBounceDuration = 1.0f;
        [ShowIf("showFinger")]
        public float fingerBounceDelay = 0.5f;
        
        public bool showCircles = true;
        [ShowIf("showCircles")]
        public Vector3 circlesScale = Vector3.one;

        public FTUEHintSettings DeepClone()
        {
            return (FTUEHintSettings)MemberwiseClone();
        }
    }

    [Serializable]
    public class FTUETooltipSettings
    {
        public Vector2 tooltipAbsoluteOffset = new Vector2(200, -430);
        public Vector2 tooltipSizeRelativeOffset;

        [TextArea(1, 5)] public string tooltipText = "Нажмите сюда";

        public FTUETooltipSettings DeepClone()
        {
            return (FTUETooltipSettings)MemberwiseClone();
        }
    }
        
    [Serializable]
    public class FTUEHighlightSettings
    {
        public Color overlayColor = new Color(0.0f, 0.0f, 0.0f, 0.588f);
        
        public FTUEHighlightSettings DeepClone()
        {
            return (FTUEHighlightSettings)MemberwiseClone();
        }
    }
}