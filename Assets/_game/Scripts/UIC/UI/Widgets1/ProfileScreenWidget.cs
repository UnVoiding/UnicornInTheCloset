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
    public class ProfileScreenWidget : Widget
    {
        [Header("Profile Screen Widget")]
        //// DATA
        [                                               SerializeField, FoldoutGroup("References")] 
        private Button backBtn;
        [                                               SerializeField, FoldoutGroup("References")] 
        private Button renamePlayerBtn;

        [                                               SerializeField, FoldoutGroup("References")] 
        private TabController tabController;

        //// RUNTIME 
        // [                                                NonSerialized, ReadOnly, FoldoutGroup("Runtime")] 
        
        public override void InitializeWidget()
        {
            base.InitializeWidget();

            widgetType = WidgetType.PLAYER_PROFILE;
            
            backBtn.onClick.AddListener(() =>
            {
                UIManager.Instance.GoToComposition(Composition.MAIN);
            });
            
            renamePlayerBtn.onClick.AddListener(() =>
            {
                Widget renameWid = UIManager.Instance.GetWidget(WidgetType.RENAME_PLAYER);
                renameWid.Show();
            });
        }

        public override void Show(Action onComplete = null)
        {
            PlayerProfileAdvicesTab t1 = (PlayerProfileAdvicesTab) tabController.tabs[0];
            t1.Populate();

            PlayerProfileGameItemsTab t2 = (PlayerProfileGameItemsTab) tabController.tabs[1];
            t2.Populate();

            if (Inventory.Instance.worldState.Value.lawyerFinished)
            {
                PlayerProfileLawyerTab t3 = (PlayerProfileLawyerTab) tabController.tabs[2];
                t3.Populate();
            }

            base.Show(onComplete);
        }

        public override void Hide(Action onComplete = null)
        {
            base.Hide(onComplete);
        }
    }
}