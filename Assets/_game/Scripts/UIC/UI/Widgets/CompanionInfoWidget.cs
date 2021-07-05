using UnityEngine;
using System;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using UnityEngine.UI;
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

                var ftueState = Inventory.Instance.ftueState.Value;
                if (!ftueState.GetFTUE(FTUEType.COMPANION_SELECTION2)
                    && ftueState.needShowCompanionSelection)
                {
                    UIManager.Instance.FTUEWidget.WithdrawFTUE();
                    Inventory.Instance.ftueState.Value.SetFTUE(FTUEType.COMPANION_SELECTION2, true);
                    Inventory.Instance.ftueState.Save();
                    
                    UIManager.Instance.FTUEWidget.Hide();
                }
                
                Hide();
            });
            
            tabController.InitPrecreatedTabs();

            for (int i = 0; i < tabController.tabToggles.Count; i++)
            {
                tabController.tabToggles[i].toggle.onValueChanged.AddListener(OnTabToggled(tabController.tabToggles[i].toggle)); 
            }
            
            tabController.tabToggles[0].toggle.targetGraphic.color = new Color(1, 1, 1, 0);
            
            tabController.ActivateTab(0);
        }

        private UnityAction<bool> OnTabToggled(Toggle toggle)
        {
            return (toggled) =>
            {
                // toggle.targetGraphic.gameObject.SetActive(!toggled);
                toggle.targetGraphic.color = new Color(1, 1, 1, toggled ? 0 : 1);
            };
        }

        public void ShowForCompanion(CompanionState companionState, bool showTalkBtn)
        {
            this.companionState = companionState;

            CompanionInfoImageTab t1 = (CompanionInfoImageTab) tabController.tabs[0];
            t1.Populate(companionState.Data);

            CompanionInfoInfoTab t2 = (CompanionInfoInfoTab) tabController.tabs[1];
            t2.Populate(companionState.Data);

            talkBtn.gameObject.SetActive(showTalkBtn);

            Show(() =>
            {
                var ftueState = Inventory.Instance.ftueState.Value;
                if (!ftueState.GetFTUE(FTUEType.COMPANION_SELECTION_INFO_TAB) 
                    && ftueState.needShowCompanionSelection)
                {
                    UIManager.Instance.FTUEWidget.Show();
                    UIManager.Instance.FTUEWidget.PresentFTUE(tabController.tabToggles[1].gameObject, FTUEType.COMPANION_SELECTION_INFO_TAB);
                }
                else if (!ftueState.GetFTUE(FTUEType.COMPANION_SELECTION2)
                         && ftueState.needShowCompanionSelection)
                {
                    UIManager.Instance.FTUEWidget.Show();
                    UIManager.Instance.FTUEWidget.PresentFTUE(talkBtn.gameObject, FTUEType.COMPANION_SELECTION2);
                }
            });
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


