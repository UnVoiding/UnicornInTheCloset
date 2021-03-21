using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RomenoCompany
{
    [Serializable]
    public class PlayerItem
    {
        public int id; 
        public string name;
        public string description;
        public string textOnUsage;
        public string code;
        public Sprite image;
    }
}
