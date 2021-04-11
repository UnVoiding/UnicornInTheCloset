using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace RomenoCompany
{
    [RequireComponent(typeof(TMP_Text))]
    public class TMPStyler : MonoBehaviour
    {
        [                                            FoldoutGroup("Settings")] 
        public string style = "main";
        
#if UNITY_EDITOR
        private void Awake()
        {
            TMPStyle styleObj;
            bool b = DB.Instance.tmpSettings.tmpStyles.TryGetValue(style, out styleObj);
            if (b)
            {
                var tmp = GetComponent<TMP_Text>();
                tmp.font = styleObj.font;
            }
        }
#endif
    }
}