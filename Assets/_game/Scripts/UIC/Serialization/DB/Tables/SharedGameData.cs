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
        public float ftueToolTipShowTime = 0.3f;
        public float ftueToolTipHideTime = 0.2f;
        public AnimationCurve ftueToolTipShowCurve = AnimationCurve.Linear(0, 0, 1, 1);
        public AnimationCurve ftueToolTipHideCurve = AnimationCurve.Linear(0, 0, 1, 1);
    }
}
