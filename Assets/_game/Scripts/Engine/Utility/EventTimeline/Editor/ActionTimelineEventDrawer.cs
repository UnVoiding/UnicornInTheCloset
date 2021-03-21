using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RomenoCompany
{
    [CustomPropertyDrawer(typeof(ActionTimelineEvent), true)]
    public class ActionTimelineEventDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var propertyName = property.FindPropertyRelative("name");
            var propertyPosition = property.FindPropertyRelative("position");
            var propertyCallback = property.FindPropertyRelative("callback");

            float timePositionWidth = 50;
            var nameRect = new Rect(position.x, position.y, position.width - timePositionWidth - 5, EditorGUIUtility.singleLineHeight);
            var timePositionRect = new Rect(position.x + position.width - timePositionWidth, position.y, timePositionWidth, EditorGUIUtility.singleLineHeight);
            var callbackRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2, position.width, EditorGUI.GetPropertyHeight(propertyCallback));

            EditorGUI.PropertyField(nameRect, propertyName, GUIContent.none);
            EditorGUI.PropertyField(timePositionRect, propertyPosition, GUIContent.none);
            EditorGUI.PropertyField(callbackRect, propertyCallback, GUIContent.none);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var propertyCallback = property.FindPropertyRelative("callback");
            return EditorGUIUtility.singleLineHeight + 2 + EditorGUI.GetPropertyHeight(propertyCallback);
        }
    }
}
