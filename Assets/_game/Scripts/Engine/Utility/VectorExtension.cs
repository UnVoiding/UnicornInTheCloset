using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

namespace RomenoCompany
{
    public static class VectorExtention
    {
        public static Vector3 InvertedX(this Vector3 value)
        {
            value.x = -value.x;
            value.y = value.y;
            value.z = value.z;
            return value;
        }
    }    
}
