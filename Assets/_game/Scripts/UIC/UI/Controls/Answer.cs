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
            text.text = p.parsedText;
            passage = p;
        }

        public void OnClick()
        {
            var cw = UIManager.Instance.ChatWidget;
            cw.currentPassage = passage; 
            // cw.PresentPassage(true);
            cw.ClearCurrentAnswers();
            cw.ContinueDialogue();
        }
    }
}

