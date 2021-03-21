using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RomenoCompany
{
    [CustomPropertyDrawer(typeof(Component), true)]
    public class ObjectDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.objectReferenceValue)
            {
                Rect objectRect = new Rect(position.x, position.y, position.width - 50, position.height);
                EditorGUI.PropertyField(objectRect, property, label, true);

                Rect editRect = new Rect((position.x + position.width) - 50, position.y, 50, position.height);
                if(GUI.Button(editRect, "Edit"))
                {
                    
                    PopupInspector.Inspect(property.objectReferenceValue);
                }
            }
            else
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }
    }

    public class PopupInspector : EditorWindow
    {
        private Editor _editor = null;
        private Vector2 _scroll = Vector2.zero;
        public static void Inspect(Object target)
        {
            PopupInspector window = PopupInspector.CreateInstance<PopupInspector>();
            window.titleContent = new GUIContent(target.name);
            window.Show();

        }

        private void OnEnable()
        {
            _scroll = Vector2.zero;
        }

        private void OnGUI()
        {
            _scroll = GUILayout.BeginScrollView(_scroll);
            if (_editor != null) _editor.OnInspectorGUI();
            GUILayout.EndScrollView();
        }
    }

}
