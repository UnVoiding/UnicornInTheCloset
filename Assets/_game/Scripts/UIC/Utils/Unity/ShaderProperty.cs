using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RomenoCompany
{
    public static class ShaderProperty
    {
        public static readonly int mainTex = Shader.PropertyToID("_MainTex");
        public static readonly int color = Shader.PropertyToID("_Color");
    }    
}
