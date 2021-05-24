using UnityEngine;

namespace RomenoCompany.Utility
{
    public static class RectUtility
    {
        public static Rect Combine(this Rect rect1, Rect rect2)
        {
            float xMin = Mathf.Min(rect1.x, rect2.x);
            float yMin = Mathf.Min(rect1.y, rect2.y);
            return new Rect(
                xMin, 
                yMin, 
                Mathf.Max(rect1.x + rect1.width, rect2.x + rect2.width) - xMin, 
                Mathf.Max(rect1.y + rect1.height, rect2.x + rect2.height) - yMin);
        }
    }
}