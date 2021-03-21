using System;
using System.Collections.Generic;
using UnityEngine;

namespace RomenoCompany
{
    public static class CameraExtension
    {
        // get camera frustrum height at distance
        public static float GetFrustrumHeight(this Camera camera, float distance)
        {
            return 2.0f * distance * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        }

        // get camera frustrum width at distance
        public static float GetFrustrumWidth(this Camera camera, float distance)
        {
            return camera.GetFrustrumHeight(distance) * camera.aspect;
        }
    }
}


