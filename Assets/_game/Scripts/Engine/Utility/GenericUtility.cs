using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


namespace RomenoCompany
{
    public static class GenericUtility
    {
        public static Tween Invoke(Action function, float delay)
        {
            float t = 0;
            return DOTween.To(() => t, (x) => t = x, 1.0f, delay)
                .SetUpdate(UpdateType.Normal)
                .OnComplete(() =>
                {
                    function();
                });
        }
    }    
}

