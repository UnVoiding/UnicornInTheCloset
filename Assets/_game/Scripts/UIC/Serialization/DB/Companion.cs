using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RomenoCompany
{
    [Serializable]
    public class Companion
    {
        public int id;
        public int internalId;
        public string name;
        public string code;
        [TextArea(2, 10)] public string characteristics;
        [TextArea(1, 5)] public string description;
        public List<CompanionEmotion> emotions;
    }
    
    [Serializable]
    public class CompanionEmotion
    {
        public string emotionName;
        public Sprite sprite;
    }
}
