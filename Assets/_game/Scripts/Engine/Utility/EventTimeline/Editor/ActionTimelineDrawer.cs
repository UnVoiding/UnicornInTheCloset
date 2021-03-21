using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RomenoCompany
{
    // [CustomPropertyDrawer(typeof(ActionTimeline))]
    // public class ActionTimelineDrawer : PropertyDrawer
    // {
    //     private class Style
    //     {
    //         public GUIStyle background = (GUIStyle)"TE Toolbar";
    //         public Texture eventThumb = EditorGUIUtility.FindTexture("Animation.EventMarker");
    //     }
    //
    //
    //     private Style _style = null;
    //     private Style style => _style ?? (_style = new Style());
    //
    //     private TimeArea _timeArea;
    //     //private TimeArea timeArea => _timeArea ?? (_timeArea = new TimeArea());
    //
    //     private int _id = "EventTimeline".GetHashCode();
    //
    //     private int _select = -1;
    //     public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    //     {
    //
    //         var prefixRect = EditorGUI.PrefixLabel(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), label);
    //         float timeDurationWidth = 50;
    //         var timeLineRect = new Rect(prefixRect.x, prefixRect.y, prefixRect.width - timeDurationWidth - 5, prefixRect.height);
    //         var timeDurationRect = new Rect(prefixRect.x + prefixRect.width - timeDurationWidth, prefixRect.y, timeDurationWidth, prefixRect.height);
    //
    //         var duration = property.FindPropertyRelative("_duration");
    //         EditorGUI.PropertyField(timeDurationRect, duration, GUIContent.none, true);
    //
    //         Event e = Event.current;
    //
    //         if (e.type == EventType.Repaint)
    //         {
    //             style.background.Draw(timeLineRect, false, false, false, false);
    //         }
    //
    //         if (_timeArea == null)
    //         {
    //             _timeArea = new TimeArea(true);
    //             _timeArea.hRangeLocked = true;
    //             _timeArea.vRangeLocked = true;
    //             _timeArea.hSlider = false;
    //             _timeArea.vSlider = false;
    //             _timeArea.hRangeMin = 0;
    //             _timeArea.leftmargin = 5;
    //             _timeArea.rightmargin = 25;
    //             _timeArea.scaleWithWindow = true;
    //             _timeArea.ignoreScrollWheelUntilClicked = true;
    //         }
    //
    //         timeLineRect.xMin += 5;
    //         timeLineRect.xMax -= 4;
    //
    //         if (Event.current.type == EventType.Repaint)
    //             _timeArea.rect = timeLineRect;
    //
    //         _timeArea.hRangeMax = duration.floatValue;
    //         _timeArea.SetShownHRangeInsideMargins(0, duration.floatValue);
    //
    //
    //         _timeArea.TimeRuler(timeLineRect, 100);
    //         var toDelete = -1;
    //         var events = property.FindPropertyRelative("_events");
    //         for (int i = 0; i < events.arraySize; i++)
    //         {
    //             var et = events.GetArrayElementAtIndex(i);
    //             var propertyPosition = et.FindPropertyRelative("position");
    //             var propertyName = et.FindPropertyRelative("name");
    //
    //             if (EventMarker.Do(_timeArea, Mathf.Clamp01(propertyPosition.floatValue) * duration.floatValue, propertyName.stringValue, _select == i, out float outPosition))
    //             {
    //                 if (e.alt)
    //                 {
    //                     toDelete = i;
    //                 }
    //                 else _select = i;
    //                 GUI.FocusControl("");
    //             }
    //
    //             propertyPosition.floatValue = Mathf.Clamp01(outPosition / duration.floatValue);
    //         }
    //
    //         if (_timeArea.TimeRulerButton(timeLineRect, out float time))
    //         {
    //             if (e.shift)
    //             {
    //                 events.InsertArrayElementAtIndex(0);
    //                 events.serializedObject.ApplyModifiedProperties();
    //                 var et = events.GetArrayElementAtIndex(0);
    //                 var propertyPosition = et.FindPropertyRelative("position");
    //                 var propertyName = et.FindPropertyRelative("name");
    //
    //                 propertyPosition.floatValue = Mathf.Clamp01(time / duration.floatValue);
    //                 propertyName.stringValue = $"OnSomeAction {events.arraySize}";
    //
    //                 events.serializedObject.ApplyModifiedProperties();
    //             }
    //             else _select = -1;
    //         }
    //
    //         if (_select >= 0 && _select < events.arraySize)
    //         {
    //             var et = events.GetArrayElementAtIndex(_select);
    //             float height = EditorGUI.GetPropertyHeight(et);
    //             Rect eventRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 5, position.width, height);
    //             EditorGUI.PropertyField(eventRect, et);
    //         }
    //
    //         if (toDelete >= 0)
    //         {
    //             events.DeleteArrayElementAtIndex(toDelete);
    //             _select = -1;
    //         }
    //     }
    //
    //     public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    //     {
    //         float height = EditorGUIUtility.singleLineHeight + 5;
    //
    //         var events = property.FindPropertyRelative("_events");
    //         if (_select >= 0 && _select < events.arraySize)
    //         {
    //             var et = events.GetArrayElementAtIndex(_select);
    //             height += EditorGUI.GetPropertyHeight(et);
    //         }
    //
    //         return height;
    //     }
    // }
}

