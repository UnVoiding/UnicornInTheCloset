using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using TMPro;
using Sirenix.OdinInspector;
using DG.Tweening;
using Button = UnityEngine.UI.Button;

namespace RomenoCompany
{
    public class AdviceWidget : Widget
    {
        [                                         Header("Advice Widget"), FoldoutGroup("References")] 
        public TMP_Text adviceText;
        [                                                                  FoldoutGroup("References")] 
        public Button closeBtn;
        [                                                                  FoldoutGroup("References")] 
        public RectTransform scrollRectTransform;
        [                                                                  FoldoutGroup("References")] 
        public RectTransform viewportRectTransform;

        [                             NonSerialized, ShowInInspector, ReadOnly, FoldoutGroup("Runtime")] 
        public Action onClose;

        
        public override void InitializeWidget()
        {
            base.InitializeWidget();
            
            widgetType = WidgetType.ADVICE;
            closeBtn.onClick.AddListener(() =>
            {
                Hide();
            });
        }

        private bool layoutRebuild = false;
        public void ShowWithAdvice(string text)
        {
            var chatScreen = UIManager.Instance.ChatWidget;
            
            adviceText.text = text;
            adviceText.fontSize = chatScreen.em;
            adviceText.margin = chatScreen.margins;
            
            Show();

            layoutRebuild = true;
            
            // StartCoroutine(LayoutRebuild());
        }

        private IEnumerator LayoutRebuild()
        {
            yield return null;

            layoutRebuild = true;
            
            yield return new WaitForSeconds(0.2f);

            layoutRebuild = true;
        }

        private void Update()
        {
            if (layoutRebuild)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(viewportRectTransform);
                LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRectTransform);
                layoutRebuild = false;
            }
            
            // if (Input.GetKey(KeyCode.A))
            // {
            //     LayoutRebuilder.ForceRebuildLayoutImmediate(rootRectTransform);
            // }
            
            // if (Input.GetKey(KeyCode.S))
            // {
            //     LayoutRebuilder.ForceRebuildLayoutImmediate(adviceText.rectTransform.parent.GetComponent<RectTransform>());
            // }
        }

        public override void Show(System.Action onComplete = null)
        {
            base.Show(onComplete);
        }

        public override void Hide(Action onComplete = null)
        {
            if (!hidding)
            {
                onClose?.Invoke();
            }

            base.Hide(onComplete);
        }
    }
}