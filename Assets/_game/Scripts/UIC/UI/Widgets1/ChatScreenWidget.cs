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
    public class ChatScreenWidget : Widget
    {
        [Header("Chat Screen Widget")]
        //// DATA
        [                                               SerializeField, FoldoutGroup("References")] 
        private Button backBtn;
        [                                               SerializeField, FoldoutGroup("References")] 
        private Button infoBtn;
        [                                               SerializeField, FoldoutGroup("References")] 
        private Answer answerPfb;
        [                                               SerializeField, FoldoutGroup("References")] 
        private Transform answerRoot;

        //// RUNTIME 
        [                                                NonSerialized, ReadOnly, FoldoutGroup("Runtime")] 
        public bool qq;
        
        public override void InitializeWidget()
        {
            base.InitializeWidget();

            widgetType = WidgetType.CHAT;
            backBtn.onClick.AddListener(() =>
            {
                UIManager.Instance.GoToComposition(Composition.MAIN);
            });
            
            infoBtn.onClick.AddListener(() =>
            {
                UIManager.Instance.GetWidget(WidgetType.COMPANION_INFO);
            });
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