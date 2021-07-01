using UnityEditor;
using UnityEditor.UI;

namespace RomenoCompany.Editor
{
    [CustomEditor(typeof(RCButton), true)]
    [CanEditMultipleObjects]
    /// <summary>
    ///   Custom Editor for the RCButton Component.
    ///   Extend this class to write a custom editor for a component derived from Button.
    /// </summary>
    public class BRCuttonEditor : ButtonEditor
    {
        SerializedProperty m_Text;
        SerializedProperty m_ClickedTextOffset;

        protected override void OnEnable()
        {
            base.OnEnable();
            
            m_Text = serializedObject.FindProperty("text");
            m_ClickedTextOffset = serializedObject.FindProperty("clickedTextOffset");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();
            EditorGUILayout.PropertyField(m_Text);
            EditorGUILayout.PropertyField(m_ClickedTextOffset);
            serializedObject.ApplyModifiedProperties();
        }
    }
}