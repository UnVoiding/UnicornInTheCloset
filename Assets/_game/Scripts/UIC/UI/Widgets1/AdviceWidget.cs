using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
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

        
        public override void InitializeWidget()
        {
            widgetType = WidgetType.ADVICE;
            closeBtn.onClick.AddListener(() =>
            {
                Hide();
            });
        }

        public void ShowWithAdvice(string text)
        {
            adviceText.text = text;
            
            Show();
        }

        public override void Show(System.Action onComplete = null)
        {
            base.Show(onComplete);
        }

        public override void Hide(Action onComplete = null)
        {
            base.Hide(onComplete);
        }
    }
}