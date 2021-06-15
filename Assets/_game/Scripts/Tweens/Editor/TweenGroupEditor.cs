using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RomenoCompany
{
	[CustomEditor(typeof(TweenGroup))]
public class TweenGroupEditor : UnityEditor.Editor
{
    private bool testInEditor;

    private TweenGroup anim;

    public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		anim = target as TweenGroup;
		
		DrawTweenList();
		
		DrawDebugButtons(anim);

	    bool previousTestInEditor = testInEditor;
	    testInEditor = EditorGUILayout.Toggle("Test In Editor", testInEditor);
	    if (previousTestInEditor != testInEditor)
	    {
	        if (testInEditor)
	        {
	            anim.SubscribeToEditorUpdates();
	        }
	        else
	        {
	            anim.UnsubscribeFromEditorUpdates();
	        }
	    }
	}

	private bool expand = true;
	void DrawTweenList()
	{
		if (anim == null)
		{
			return;
		}

		Color oldCOntentColor = GUI.contentColor;
		GUI.contentColor = Color.blue;
		expand = EditorGUILayout.Foldout(expand, "NewTweensEditor");

		GUI.contentColor = oldCOntentColor;
		
		if (!expand)
			return;
		
		GUIStyle headerStyle = new GUIStyle();
		headerStyle.fontSize = 30;
		headerStyle.fontStyle = FontStyle.Bold;
		headerStyle.alignment = TextAnchor.MiddleCenter;
		
		GUIStyle delayStyle = new GUIStyle(GUI.skin.textField);
		delayStyle.alignment = TextAnchor.MiddleRight;
		
		
		EditorGUILayout.BeginHorizontal(GUILayout.Height(40));
		EditorGUILayout.LabelField("Tweens:",headerStyle);
		EditorGUILayout.EndHorizontal();

		headerStyle.fontSize = 12;
		headerStyle.alignment = TextAnchor.MiddleLeft;
		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		EditorGUILayout.LabelField("TweenType", headerStyle, GUILayout.Width(100));
		EditorGUILayout.LabelField("Reference", headerStyle, GUILayout.Width(150));
		EditorGUILayout.LabelField("Delay", headerStyle, GUILayout.Width(50));
		EditorGUILayout.LabelField("IsFwd", headerStyle, GUILayout.Width(60));
		EditorGUILayout.LabelField("Del", headerStyle,  GUILayout.Width(25));
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		
		Color oldColor = GUI.color;
		
		for (int i = 0; i < anim.Tweens.Length; i++)
		{
			TweenGroup.TweenEntry entry = anim.Tweens[i];

			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			
			string name = entry.tween != null ? entry.tween.GetType().ToString().Replace("RomenoCompany.", "") : "Unknown";
			
			EditorGUILayout.LabelField(name, GUILayout.Width(100));
			entry.tween = EditorGUILayout.ObjectField(entry.tween, typeof(TweenBase), true, GUILayout.Width(150)) as TweenBase;
			entry.delay = EditorGUILayout.FloatField(entry.delay, delayStyle, GUILayout.Width(50));
			
			EditorGUILayout.BeginHorizontal(GUILayout.Width(60));
			GUILayout.FlexibleSpace();
			entry.isForwardOnlyTween = EditorGUILayout.Toggle(entry.isForwardOnlyTween, GUILayout.Width(10));
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			
			GUI.color = Color.red;
			if (GUILayout.Button("X", GUILayout.Width(25)))
			{
				List<TweenGroup.TweenEntry> tweenList = new List<TweenGroup.TweenEntry>(anim.Tweens);
				tweenList.RemoveAt(i);
				anim.Tweens = tweenList.ToArray();
				break;
			}

			GUI.color = oldColor;
			
			GUILayout.Space(10);
			
			anim.Tweens[i] = entry;
			
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
		}
		
		GUI.color = Color.green;

		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Add New Tween"))
		{
			List<TweenGroup.TweenEntry> tweenList = new List<TweenGroup.TweenEntry>(anim.Tweens);
			tweenList.Add(new TweenGroup.TweenEntry());
			anim.Tweens = tweenList.ToArray();
		}
		EditorGUILayout.EndHorizontal();
		
		GUI.color = oldColor;
		
	}

    private void OnDisable()
    {
        if (testInEditor)
        {
             anim.UnsubscribeFromEditorUpdates();
        }
        testInEditor = false;
    }

    static void DrawDebugButtons(TweenGroup animation)
	{
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Set End State"))
		{
			animation.SetEndState(0);
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Set Begin State"))
		{
			animation.SetBeginState(0);
		}
		EditorGUILayout.EndHorizontal();
	}

}
}
