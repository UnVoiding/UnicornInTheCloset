using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Video;

namespace RomenoCompany
{
    [CreateAssetMenu(
        fileName = "SharedGameData", 
        menuName = "UIC/ShredGameData", 
        order = 59)]
    public class SharedGameData : SerializedScriptableObject
    {
        // Snowflake
        public float snowflakeTimeToReadCompanionSymbol = 0.033f;
        public float snowflakeTimeToReadHeroSymbol = 0.015f;
        public float snowflakeMinAfterPassageWaitTime = 1.5f;
        public float snowflakeImageMessageWaitTime = 2f;
        public float snowflakeWaitTimeAfterHeroMessage = 1f;
        public float snowflakeDefaultWaitTimeAfterPassage = 1f;
        
        public float ftueToolTipShowTime = 0.3f;
        public float ftueToolTipHideTime = 0.2f;
        public AnimationCurve ftueToolTipShowCurve = AnimationCurve.Linear(0, 0, 1, 1);
        public AnimationCurve ftueToolTipHideCurve = AnimationCurve.Linear(0, 0, 1, 1);
    }
}
