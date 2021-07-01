#if UNITY_EDITOR
using UnityEditor;
#endif
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace RomenoCompany
{
    [ExecuteAlways]
    [RequireComponent(typeof(TMP_Text))]
    [DisallowMultipleComponent]
    public class TMPStyler : MonoBehaviour
    {
        [                                            FoldoutGroup("Settings")] 
        public string style = "main";
        
        private void Awake()
        {
            TMPStyle styleObj;
            TMPExtraSettings settings = Resources.Load<TMPExtraSettings>("TMPExtraSettings");
            bool b = settings.tmpStyles.TryGetValue(style, out styleObj);
            if (b)
            {
                var tmp = GetComponent<TMP_Text>();

                styleObj.ApplyStatic(tmp);
#if UNITY_EDITOR
                EditorUtility.SetDirty(tmp);
#endif
                if (Application.isPlaying)
                {
                    styleObj.ApplyRuntime(tmp);
                }
            }
        }
    }
}