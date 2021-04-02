using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RomenoCompany
{
    [Serializable]
    public class CompanionData
    {
        public int id;
        public int internalId;
        public string name;
        public string code;
        public string klass;
        public string gameItem;
        public string skill;
        public string vulnerability;
        public string goal;
        [TextArea(1, 5)] public string description;

        public Sprite fullLengthImage;
        public List<CompanionEmotion> emotions;
        
        public List<TextAsset> dialogueJsons;
        [NonSerialized]
        public string formattedCharacteristics;
    }
    
    [Serializable]
    public class CompanionEmotion
    {
        public string emotionName;
        public Sprite sprite;
    }
}
