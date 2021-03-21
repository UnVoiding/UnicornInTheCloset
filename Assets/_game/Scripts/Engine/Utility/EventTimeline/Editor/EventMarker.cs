using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
    // public class EventMarker
    // {
    //     private class Style
    //     {
    //         public Texture eventThumb = EditorGUIUtility.FindTexture("Animation.EventMarker");
    //         public Rect eventThumbRect => new Rect(0, 0, eventThumb.width, eventThumb.height);
    //
    //         public GUIStyle timeTooltip = new GUIStyle("AnimationEventTooltip")
    //         {
    //             margin = new RectOffset(0, 0, 0, 0),
    //             contentOffset = new Vector2(0, 0),
    //             border = new RectOffset(8, 8, 8, 8),
    //             padding = new RectOffset(10, 10, 5, 5),
    //             overflow = new RectOffset(0, 0, 0, 0)
    //         };
    //     }
    //
    //
    //     private static Style _style = null;
    //     private static Style style => _style ?? (_style = new Style());
    //
    //     private static int _idMaker = "EventMaker".GetHashCode();
    //     private static float _position;
    //     private static int _hoverMaker = 0;
    //     private static bool _isDrag;
    //     private static int _inputField = 0;
    //     public static bool Do(TimeArea timeArea, float position, string name, bool select, out float outPosition)
    //     {
    //         outPosition = position;
    //
    //         int id = EditorGUIUtility.GetControlID(_idMaker, FocusType.Passive);
    //         Event e = Event.current;
    //
    //         Rect eventThumbRect = style.eventThumbRect;
    //         eventThumbRect.y = timeArea.drawRect.y;
    //         eventThumbRect.x = timeArea.TimeToPixel(position, timeArea.drawRect) - eventThumbRect.width * 0.5f;
    //
    //         switch (e.GetTypeForControl(id))
    //         {
    //             case EventType.MouseDown:
    //                 {                     
    //                     if (_inputField != 0)
    //                     {
    //                         GUIContent contentTime = new GUIContent($"0");
    //                         Rect rectLabel = new Rect(0, 0, 100, style.timeTooltip.CalcHeight(contentTime, 100));
    //                         rectLabel.center = new Vector2(eventThumbRect.center.x, eventThumbRect.yMin - rectLabel.height * 0.5f - 2);
    //                         if (!rectLabel.Contains(e.mousePosition))
    //                         {
    //                             _inputField = 0;
    //                             e.Use();
    //                         }
    //                     }
    //
    //                     if (eventThumbRect.Contains(e.mousePosition))
    //                     {
    //                         if (e.clickCount > 1)
    //                         {
    //                             _inputField = id;
    //                         }
    //
    //                         _isDrag = false;
    //                         EditorGUIUtility.hotControl = id;
    //                         e.Use();
    //                     }
    //                 }
    //                 break;
    //             case EventType.MouseUp:
    //                 {
    //                     if (EditorGUIUtility.hotControl == id)
    //                     {
    //                         EditorGUIUtility.hotControl = 0;
    //                         e.Use();
    //                         if (!_isDrag) return true;
    //
    //                         _isDrag = false;
    //                     }
    //                 }
    //                 break;
    //             case EventType.MouseMove:
    //                 {
    //                     if (eventThumbRect.Contains(e.mousePosition)) _hoverMaker = id;
    //                 }
    //                 break;
    //             case EventType.MouseDrag:
    //                 {
    //                     if (EditorGUIUtility.hotControl == id)
    //                     {
    //                         _isDrag = true;
    //                         outPosition = timeArea.PixelToTime(e.mousePosition.x, timeArea.drawRect);
    //                         e.Use();
    //                     }
    //                 }
    //                 break;
    //             case EventType.KeyDown:
    //                 {
    //                     if (_inputField != 0)
    //                     {
    //                         if (e.keyCode == KeyCode.Return)
    //                         {
    //                             _inputField = 0;
    //                             GUI.FocusControl("");
    //                             e.Use();
    //                         }
    //                     }
    //                 }
    //                 break;
    //             case EventType.Repaint:
    //                 {
    //                     Color temp = GUI.color;
    //                     Color color = GUI.color;
    //
    //                     if (_inputField == id)
    //                     {
    //
    //                         GUIContent contentTime = new GUIContent(name);
    //                         Rect rectLabel = new Rect(Vector2.zero, style.timeTooltip.CalcSize(contentTime));
    //                         rectLabel.center = new Vector2(eventThumbRect.center.x, eventThumbRect.yMin - rectLabel.height * 0.5f - 2);
    //
    //                         GUI.SetNextControlName("Field");
    //                         EditorGUI.TextField(rectLabel, name, style.timeTooltip);
    //                         GUI.FocusControl("Field");
    //                         EditorGUI.TextField(rectLabel, name, style.timeTooltip);
    //                     }
    //                     else
    //                     {
    //                         if (EditorGUIUtility.hotControl == id)
    //                         {
    //                             if (_isDrag)
    //                             {
    //                                 GUIContent contentTime = new GUIContent($"{name} {timeArea.FormatTime(position, 100, TimeArea.TimeFormat.TimeFrame)}");
    //                                 Rect rectLabel = new Rect(Vector3.zero, style.timeTooltip.CalcSize(contentTime));
    //                                 rectLabel.center = new Vector2(eventThumbRect.center.x, eventThumbRect.yMin - rectLabel.height * 0.5f - 2);
    //                                 EditorGUI.LabelField(rectLabel, contentTime, style.timeTooltip);
    //                             }
    //                             color = Handles.preselectionColor;
    //                         }
    //                     }
    //
    //                     if (select) color = Handles.selectedColor;
    //
    //
    //                     GUI.color = color;
    //                     GUI.DrawTexture(eventThumbRect, style.eventThumb);
    //
    //                     GUI.color = temp;
    //                 }
    //                 break;
    //         }
    //         return false;
    //     }
    // }
}
