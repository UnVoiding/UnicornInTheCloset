using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RomenoCompany
{
    [Serializable]
    public class PlayerItemData
    {
        public enum ItemID
        {
            SOULSTONE = 0,
            CUT_OFF_WINGS = 1,
            HELM = 2,
            BROKEN_TABLETS = 3,
            RORCHACH_STAIN = 4,
            HEART = 5,
            CAT_DARWIN = 6,
            HOMOPHOBE_INSIDE = 7,
            SIGN = 8,
            SHADOWPLAY = 9,
            TICKETS = 10,
            RAINBOW_FLAG = 11,
            GREY_FLAG = 12,
            GLOSS = 13,
            SACRIFICIAL_KNIFE = 14,
            SNOW_ORB = 15,
            LITTLE_FIRE = 16,
            ELIXIR = 17,
            BALL_OF_YARN = 18,
            BUDDHA_STATUETTE = 19,
            CODEX = 20,
            QUITE_VIOLA = 21,
            RUBIKS_CUDE = 22,
            NONE = 1000
        }
        
        public ItemID id; 
        public string name;
        public string description;
        public string textOnUsage;
        public string code;
        public Sprite image;
    }
}
