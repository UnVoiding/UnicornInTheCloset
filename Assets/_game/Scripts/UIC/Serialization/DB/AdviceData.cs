using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RomenoCompany
{
    [Serializable]
    public class AdviceData
    {
        public int id;
        public string caption;
        [TextArea(2, 15)] public string text;
    }
}
