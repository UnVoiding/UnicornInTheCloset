using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RomenoCompany
{
    public class EnvironmentWindow : EditorWindow
    {
        [MenuItem("UIC/Environment Editor")]
        static void StartWindow()
        {
            var w = EditorWindow.GetWindow(typeof(EnvironmentWindow));
            w.Show();
        }

        void OnGUI()
        {
            var rawDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            var defines = rawDefines.Split(';').ToList();

            if (!Directory.Exists($"{Application.dataPath}/Resources/Editor"))
            {
                AssetDatabase.CreateFolder("Assets/Resources", "Editor");
                AssetDatabase.CreateFolder("Assets/Resources/Editor", "DEFINES");
                AssetDatabase.Refresh();
                return;
            } else if (!Directory.Exists($"{Application.dataPath}/Resources/Editor/DEFINES"))
            {
                AssetDatabase.CreateFolder("Assets/Resources/Editor", "DEFINES");
                AssetDatabase.Refresh();
                return;
            }
            
            var definesSource = Resources.Load<TextAsset>("Editor/DEFINES");
            if (definesSource == null)
            {
                string str = "";
                using (FileStream fs = new FileStream("Assets/Resources/Editor/DEFINES.TXT", FileMode.Create)){
                    using (StreamWriter writer = new StreamWriter(fs)){
                        writer.Write(str);
                    }
                }
                UnityEditor.AssetDatabase.Refresh();
                return;
            }
            var DEFINES = definesSource.text.Split(',').Select(s => s.Trim(' '));

            foreach (var d in DEFINES)
            {
                EditorGUILayout.BeginHorizontal();
                var settingsValue = defines.Contains(d);
                var toggleValue = EditorGUILayout.Toggle(settingsValue);
                EditorGUILayout.LabelField(d);

                if (toggleValue != settingsValue)
                {
                    if (toggleValue)
                    {
                        defines.Add(d);
                    }
                    else
                    {
                        defines.Remove(d);
                    }
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, String.Join(";", defines));
                }
                EditorGUILayout.EndHorizontal();
            }
            
            EnvironmentManager envManager = Resources.Load<EnvironmentManager>(EnvironmentManager.PrefabResourcePath);
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField($"Current profile: {envManager.currentProfile.ToString()} ({EditorUserBuildSettings.activeBuildTarget.ToString()})");
            EditorGUILayout.BeginVertical();
            
            foreach (var profilePair in envManager.profiles)
            {
                var envName = profilePair.Key;
                var profile = profilePair.Value;

                if (GUILayout.Button($"Switch to {envName.ToString()}"))
                {
                    Debug.Log($"Switching environment to {envName.ToString()}");
                    envManager.ClearEnvironmentDefines();
                    
                    using (FileStream fs = new FileStream($"{Application.dataPath}/Resources/CurrentEnvironmentProfile.txt", FileMode.Create))
                    {
                        using (StreamWriter writer = new StreamWriter(fs)){
                            writer.Write(envName.ToString());
                        }
                    }
                    
                    profile.Apply();

                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }
            EditorGUILayout.EndVertical();
        }
    }    
}
