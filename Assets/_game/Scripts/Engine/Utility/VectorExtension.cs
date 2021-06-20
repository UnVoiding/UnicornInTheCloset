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
            return new Vector3(-value.x, value.y, value.z);
        }
    }    
}
