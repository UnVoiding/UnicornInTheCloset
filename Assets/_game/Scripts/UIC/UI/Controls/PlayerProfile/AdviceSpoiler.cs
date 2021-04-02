using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RomenoCompany
{
    public class AdviceSpoiler : Spoiler
    {
        [                 Header("AdviceSpoiler"), NonSerialized, ShowInInspector, ReadOnly, FoldoutGroup("Runtime")] 
        public AdviceState adviceState;
    }
}

