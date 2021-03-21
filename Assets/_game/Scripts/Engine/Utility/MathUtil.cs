using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RomenoCompany
{
    public static class MathUtil
    {
        public static Vector2 RadialToCartesian(float angle, float radius)
        {
            return new Vector2(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle));
        }
    
        public static float ClampAngle(float angle, float from = -180.0f, float to = 180.0f)
        {
            angle %= 360;
            return angle > 180 ? angle - 360 : angle;
        }
    }    
}
