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
    public class CompanionInfoWidget : Widget
    {
        [Header("Companion Info Widget")]
        //// DATA
        [                                               FoldoutGroup("References")] 
        public Button closeBtn;
        [                                               FoldoutGroup("References")] 
        public Button talkBtn;
        [                                               FoldoutGroup("References")] 
        public TabController tabController;

        //// RUNTIME 
        [                                               NonSerialized, ReadOnly, FoldoutGroup("Runtime")] 
        public CompanionData companion;
        
        public override void InitializeWidget()
        {
            widgetType = WidgetType.COMPANION_INFO;
            closeBtn.onClick.AddListener(() =>
            {
                Hide();
            });
            
            talkBtn.onClick.AddListener(() =>
            {
                UIManager.Instance.GoToComposition(Composition.CHAT);
            });
            
            tabController.InitPrecreatedTabs();
        }

        public void ShowForCompanion(CompanionData companion)
        {
            this.companion = companion;
            
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


