using UnityEngine;
using UnityEditor;

namespace RomenoCompany
{
    [CustomEditor(typeof(TweenBase), true)]
public class TweenBaseEditor : UnityEditor.Editor
{
    private bool testInEditor;

    private TweenBase tween;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("General Parameters:", EditorStyles.boldLabel);
        EditorGUILayout.Separator();

        tween = (TweenBase)target;

        tween.duration = EditorGUILayout.FloatField("Duration", tween.duration);
        tween.durationScale = EditorGUILayout.FloatField("Duration Scale", tween.durationScale);

        tween.delay = EditorGUILayout.FloatField(new GUIContent("Delay", "Can be overriden by MBTweenAnimation"), tween.delay);

        tween.ignoreTimeScale = EditorGUILayout.Toggle("Ignore Time Scale", tween.ignoreTimeScale);

        tween.easingMethod = (EasingMethod)EditorGUILayout.EnumPopup("Easing Method",tween.easingMethod);

        if (tween.easingMethod == EasingMethod.Curve)
        {
            tween.curve = EditorGUILayout.CurveField("Curve", tween.curve);
        }
        if (tween.easingMethod == EasingMethod.DoubleCurve)
        {
            tween.beginStateCurve = EditorGUILayout.CurveField("Begin State Curve", tween.beginStateCurve);
            tween.endStateCurve = EditorGUILayout.CurveField("End State Curve", tween.endStateCurve);
        }

        if (tween.easingMethod == EasingMethod.EaseIn ||
            tween.easingMethod == EasingMethod.EaseOut ||
            tween.easingMethod == EasingMethod.EaseInOut)
        {
            tween.easingType = (EasingType)EditorGUILayout.EnumPopup("Easing Type", tween.easingType);
        }

        tween.looping = (LoopType)EditorGUILayout.EnumPopup("Loop Type", tween.looping);

        DrawEvent("OnEndStateSet");
        DrawEvent("OnBeginStateSet");
        DrawEvent("OnEndStateSetStarted");
        DrawEvent("OnBeginStateSetStarted");

        DrawDebugButtons(tween);

        bool previousTestInEditor = testInEditor;
        testInEditor = EditorGUILayout.Toggle("Test In Editor", testInEditor);
        if (previousTestInEditor != testInEditor)
        {
            if (testInEditor)
            {
                tween.SubscribeToEditorUpdates();
            }
            else
            {
                tween.UnsubscribeFromEditorUpdates();
            }
        }
    }

    private void OnDisable()
    {
        if (testInEditor)
        {
            tween.UnsubscribeFromEditorUpdates();
        }
        testInEditor = false;
    }

    void DrawEvent(string eventName)
    {
        SerializedProperty onCheck = serializedObject.FindProperty(eventName);

        EditorGUILayout.PropertyField(onCheck);

        if(GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();
        }
    }

    static void DrawDebugButtons(TweenBase tween)
    {
        Color defColor = GUI.color;

        GUI.color = tween.IsInBeginState ? Color.green : defColor;

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Set End State"))
        {
            tween.SetEndState();
        }
        EditorGUILayout.EndHorizontal();


        GUI.color = tween.IsInEndState ? Color.green : defColor;

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Set Begin State"))
        {
            tween.SetBeginState();
        }
        EditorGUILayout.EndHorizontal();

        GUI.color = defColor;
    }
}
}

