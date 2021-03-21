using UnityEngine;


namespace RomenoCompany
{
    public static class UIUtil
    {
        public static Vector3 GetUIWdigetWorldPos(GameObject widget, Camera worldCamera, float dist)
        {
            var rootCanvas = widget.GetComponentInParent<Canvas>();

            Vector3 screenPoisition = RectTransformUtility.WorldToScreenPoint(rootCanvas.worldCamera, widget.transform.position);
               
            Ray ray = worldCamera.ScreenPointToRay(screenPoisition);
            Plane plane = new Plane(Vector3.back, ray.origin + ray.direction * dist);
            plane.Raycast(ray, out float d);
            Vector3 worldPosition = ray.origin + ray.direction * d;
            return worldPosition;
        }
    }
}
