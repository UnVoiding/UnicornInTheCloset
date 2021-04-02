using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace RomenoCompany
{
    public class Answer : MonoBehaviour
    {
        [                                              SerializeField, FoldoutGroup("References")]
        public TMP_Text text;


        public void SetText(string text)
        {
            this.text.text = text;
        }
    }
}

