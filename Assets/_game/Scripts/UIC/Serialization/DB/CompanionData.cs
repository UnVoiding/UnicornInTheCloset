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
            NONE = 0,
            MAIN = 1,
            FRIEND = 2,
            EX_SCHOOLMATE = 3,
            LESBIAN = 4,
            BAD_PRIEST = 5,
            PSYCHOLOGIST_MAN = 6,
            GOOD_PRIEST = 7,
            CAT = 8,
            HOT = 9,
            HOMOPHOBIC_WOMAN = 10,
            DEVIL = 11,
            ACTIVIST_MAN = 12,
            FRIEND_GAY = 13,
            GOVERNMENT_AGENT = 14,
            TEENAGER = 15,
            HIDDING = 16,
            ACTIVIST_WOMAN = 17,
            ACCEPTING_PARENT = 18,
            PSYCHOTERAPIST_WOMAN = 19,
            LAWYER = 20,
            TRANSGENDER = 21,
            CATFISH = 22,
            PARENTS = 24,
            UNICORN = 25,
        }

        public ItemID id;
        public int internalId;
        public bool enabled;
        public bool openByDefault;
        public string name;
        public string shortName;
        public string code;
        public string klass;
        public string gameItem;
        public string skill;
        public string vulnerability;
        public string goal;
        [TextArea(1, 5)] public string description;

        public Message textMessagePfb;
        public Message imageMessagePfb;
        public Sprite fullHeightImage;
        public Sprite mainScreenImage;
        public List<CompanionEmotion> emotions;
        
        public List<TextAsset> dialogueJsons;
        [NonSerialized]
        public string formattedCharacteristics;

        public CompanionEmotion GetEmotion(string emotionName)
        {
            for (int i = 0; i < emotions.Count; i++)
            {
                if (emotions[i].emotionName == emotionName) return emotions[i];
            }

            return null;
        }
    }
    
    [Serializable]
    public class CompanionEmotion
    {
        public string emotionName;
        public Sprite sprite;
    }
}
