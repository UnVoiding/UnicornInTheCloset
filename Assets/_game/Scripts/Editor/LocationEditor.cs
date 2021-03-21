using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RomenoCompany
{
    public class LocationEditor : EditorWindow
    {
        [MenuItem("TSG/Locations List")]
        public static void Open()
        {
            var window = GetWindow<LocationEditor>();
            window.titleContent = new GUIContent("Locations");
        }
    }
}
