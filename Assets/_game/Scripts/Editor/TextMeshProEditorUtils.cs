using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.Experimental.SceneManagement;


namespace RomenoCompany
{
    public class TextMeshProEditorUtils : EditorWindow
    {
        public TMP_FontAsset selectedFontAsset;
        
        [MenuItem("Window/TextMeshPro/Tools/FontChanger")]
        private static void FontChanger()
        {
            var w = EditorWindow.GetWindow(typeof(TextMeshProEditorUtils));
            w.Show();
        }
     
        // TODO: finish
        void OnGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField($"Current profile:");
            EditorGUILayout.BeginVertical();

            selectedFontAsset = (TMP_FontAsset)EditorGUILayout.ObjectField("Select font:", selectedFontAsset, typeof(TMP_FontAsset), false);

            if (GUILayout.Button($"b1"))
            {
                Scene curScene;
                PrefabStage stage = PrefabStageUtility.GetCurrentPrefabStage();
                if (stage != null)
                {
                    curScene = stage.scene;
                    List<GameObject> roots = new List<GameObject>(curScene.rootCount);
                    curScene.GetRootGameObjects(roots);
                }
                else
                {
                    curScene = SceneManager.GetActiveScene();
                    List<GameObject> roots = new List<GameObject>(curScene.rootCount);
                    curScene.GetRootGameObjects(roots);
                }
            }
            
            if (GUILayout.Button($"b2"))
            {
                
                Debug.Log($"b2 pressed");
            }
            
            EditorGUILayout.EndVertical();
        }
    }
}