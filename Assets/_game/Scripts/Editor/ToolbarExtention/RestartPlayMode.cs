using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RomenoCompany
{
    [InitializeOnLoad]
    public static class RestartPlayMode
    {
        static RestartPlayMode()
        {
            ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUI);
        }
        
        static void OnToolbarGUI()
        {
            EditorGUIUtility.SetIconSize(new Vector2(17,17));

            if (GUILayout.Button("Clear Prefs", GUILayout.ExpandWidth(false)))
            {
                PlayerPrefs.DeleteAll();
            }

            // if (GUILayout.Button(EditorGUIUtility.IconContent("LookDevResetEnv@2x"), GUILayout.Width(30f)))
            if (GUILayout.Button(EditorGUIUtility.IconContent("RotateTool"), GUILayout.Width(30f)))
            {
                if (EditorApplication.isPlaying)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }

            if (EditorApplication.isPlaying)
            {
                TimeManager.Instance.SetTimeScale("TIME_EDITOR", EditorGUILayout.Slider("", TimeManager.Instance.GetTimeScale("TIME_EDITOR"), 0.1f, 20,GUILayout.Width(150)));
            }
        }
    }
}