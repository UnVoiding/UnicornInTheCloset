using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RomenoCompany
{
    [Serializable]
    public class CompanionData
    {
        public enum ItemID
        {
            MAIN = 0,
            FRIEND = 1,
            EX_SCHOOLMATE = 2,
            LESBIAN = 3,
            BAD_PRIEST = 4,
            PSYCHOLOGIST_MAN = 5,
            GOOD_PRIEST = 6,
            CAT = 7,
            HOT = 8,
            HOMOPHOBIC_WOMAN = 9,
            DEVIL = 10,
            ACTIVIST_MAN = 11,
            FRIEND_GAY = 12,
            GOVERNMENT_AGENT = 13,
            TEENAGER = 14,
            HIDDING = 15,
            ACTIVIST_WOMAN = 16,
            ACCEPTING_PARENT = 17,
            PSYCHOTERAPIST_WOMAN = 18,
            LAWYER = 19,
            TRANSGENDER = 20,
            CATFISH = 21,
            PARENTS = 22,
            UNICORN = 23,
            NONE = 1000,
        }

        public ItemID id;
        public int internalId;
        public bool enabled;
        public bool openByDefault;
        public string name;
        public string code;
        public string klass;
        public string gameItem;
        public string skill;
        public string vulnerability;
        public string goal;
        [TextArea(1, 5)] public string description;

        public Sprite fullHeightImage;
        public Sprite mainScreenImage;
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
