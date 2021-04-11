using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RomenoCompany
{
    public class Answer : MonoBehaviour
    {
        [                                              SerializeField, FoldoutGroup("References")]
        public TMP_Text text;
        [                                              SerializeField, FoldoutGroup("References")]
        public Button btn;

        [                                              SerializeField, FoldoutGroup("Runtime")]
        public Passage passage;

        private void Awake()
        {
            OnCreate();
        }

        public void OnCreate()
        {
            btn.onClick.AddListener(OnClick);
        }

        public void SetPassage(Passage p)
        {
            text.text = p.text;
            passage = p;
        }

        public void OnClick()
        {
            UIManager.Instance.ChatWidget.currentPassage = passage; 
            UIManager.Instance.ChatWidget.PresentPassage(true);
        }
    }
}

