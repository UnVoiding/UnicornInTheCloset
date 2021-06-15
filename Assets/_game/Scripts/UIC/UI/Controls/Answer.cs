using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RomenoCompany
{
    public class Answer : MonoBehaviour, IDroplet
    {
        [                                              SerializeField, FoldoutGroup("References")]
        public RectTransform rectTransform;
        [                                              SerializeField, FoldoutGroup("References")]
        public TMP_Text text;
        [                                              SerializeField, FoldoutGroup("References")]
        public Button btn;

        [                          NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")]
        public Passage passage;

        public GameObject Prefab { get; set; }
        public GameObject GameObject => gameObject;

        private static readonly Vector3 pooledObjectsPos; 
        
        public void SetPassage(Passage p)
        {
            text.text = p.ParsedText;
            passage = p;
        }

        public void OnClick()
        {
            var cw = UIManager.Instance.ChatWidget;
            cw.currentPassage = passage; 
            cw.PresentPassage();
            cw.ClearCurrentAnswers();
            cw.ContinueDialogue();

            var ftueState = Inventory.Instance.ftueState.Value;
            if (!ftueState.GetFTUE(FTUEType.CHAT_SCREEN_CHOOSE_ANSWER)
                && ftueState.needShowChatScreenChooseAnswerFtue)
            {
                UIManager.Instance.FTUEWidget.WithdrawFTUE(gameObject, FTUEType.CHAT_SCREEN_CHOOSE_ANSWER);
                ftueState.SetFTUE(FTUEType.CHAT_SCREEN_CHOOSE_ANSWER, true);
                Inventory.Instance.ftueState.Save();
                
                UIManager.Instance.FTUEWidget.Hide();
            }
        }
        
        public void OnCreate()
        {
            btn.onClick.AddListener(OnClick);
            gameObject.SetActive(false);
        }

        public void OnGetFromPool()
        {
            var t = gameObject.transform;  
            
            // first set parent then enable
            t.SetParent(UIManager.Instance.ChatWidget.answerRoot, false);
            t.localPosition = Vector3.zero;
            t.localScale = Vector3.one;
            gameObject.SetActive(true);
        }

        public void OnReturnToPool()
        {
            // first disable then move
            gameObject.SetActive(false);
            gameObject.transform.SetParent(Ocean.Instance.transform);
            passage = null;
        }
    }
}

