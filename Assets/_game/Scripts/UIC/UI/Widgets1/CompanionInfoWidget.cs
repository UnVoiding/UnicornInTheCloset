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
        public CompanionState companionState;
        
        public override void InitializeWidget()
        {
            base.InitializeWidget();

            widgetType = WidgetType.COMPANION_INFO;
            closeBtn.onClick.AddListener(() =>
            {
                Hide();
            });
            
            talkBtn.onClick.AddListener(() =>
            {
                Inventory.Instance.currentCompanion.Value = companionState.id;
                Inventory.Instance.currentCompanion.Save();
                UIManager.Instance.GoToComposition(Composition.CHAT);
                Hide();
            });
            
            tabController.InitPrecreatedTabs();
        }

        public void ShowForCompanion(CompanionState companionState, bool showTalkBtn)
        {
            this.companionState = companionState;

            CompanionInfoImageTab t1 = (CompanionInfoImageTab) tabController.tabs[0];
            t1.Populate(companionState.Data);

            CompanionInfoInfoTab t2 = (CompanionInfoInfoTab) tabController.tabs[1];
            t2.Populate(companionState.Data);

            talkBtn.gameObject.SetActive(showTalkBtn);

            Show();
        }

        public override void Show(System.Action onComplete = null)
        {
            tabController.OnShow();

            base.Show(onComplete);
        }

        public override void Hide(Action onComplete = null)
        {
            tabController.OnHide();

            base.Hide(onComplete);
        }
    }
}


