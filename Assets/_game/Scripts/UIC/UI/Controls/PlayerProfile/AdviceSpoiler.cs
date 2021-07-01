using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RomenoCompany
{
    public class AdviceSpoiler : Spoiler
    {
        [                 Header("AdviceSpoiler"), NonSerialized, ShowInInspector, ReadOnly, FoldoutGroup("Runtime")] 
        public AdviceState adviceState;

        // public override void Init()
        // {
        //     base.Init();
        // }

        // public override void Switch()
        // {
        //     base.Switch();
        //
        //     var crrt = contentRoot.GetComponent<RectTransform>();
        // }

    }
}

