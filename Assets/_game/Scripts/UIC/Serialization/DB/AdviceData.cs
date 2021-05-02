using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RomenoCompany
{
    [Serializable]
    public class AdviceData
    {
        public int id;
        [TextArea(3, 15)] public string caption;          // only for lawyer advices
        [TextArea(3, 15)] public string text;
        public bool showOnAdviceScreen = true;
        public CompanionData.ItemID companionId;
    }
}
