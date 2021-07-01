using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using Sirenix.OdinInspector;

namespace RomenoCompany
{
    public class ProfileScreenWidget : Widget
    {
        [              Header("Profile Screen Widget"), SerializeField, FoldoutGroup("References")] 
        private Button backBtn;
        [                                               SerializeField, FoldoutGroup("References")] 
        private Button renamePlayerBtn;
        [                                               SerializeField, FoldoutGroup("References")] 
        private RectTransform playerNameBlock;
        [                                               SerializeField, FoldoutGroup("References")] 
        private TMP_Text playerNameText;
        [                                               SerializeField, FoldoutGroup("References")] 
        public TabController tabController;
        
        [                Header("Profile Screen Widget"), SerializeField, FoldoutGroup("Settings")] 
        public bool showDevelopers = false;

        private void Awake()
        {
            Debug.Log($"~~~~~~~~~~~ Profile Screen Awake is called at {Time.frameCount}");
        }

        private void Start()
        {
            Debug.Log($"~~~~~~~~~~~ Profile Screen Start is called at {Time.frameCount}");
        }

        private bool firstUpdate = true;
        private void Update()
        {
            if (firstUpdate)
            {
                Debug.Log($"~~~~~~~~~~~ Profile Screen first Update is called at {Time.frameCount}");
                firstUpdate = false;
            }
        }
        
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
            
            tabController.InitPrecreatedTabs();

            float esw = LayoutManager.Instance.esw;

            tabController.tabToggles[1].titleText.margin = 0.65f * LayoutManager.Instance.defaultMargins; 
            tabController.tabToggles[1].titleText.fontSizeMax = 1.2f * esw;
            
            playerNameText.fontSize = 1.5f * esw;
            playerNameBlock.anchoredPosition = new Vector2(0, 2 * esw);

            PlayerProfileAdvicesTab t1 = (PlayerProfileAdvicesTab) tabController.tabs[0];
            t1.Populate();

            PlayerProfileGameItemsTab t2 = (PlayerProfileGameItemsTab) tabController.tabs[1];
            t2.Populate();

            PlayerProfileLawyerTab t3 = (PlayerProfileLawyerTab) tabController.tabs[2];
            t3.Populate();
        }

        public override void Show(Action onComplete = null)
        {
            Debug.Log($"~~~~~~~~~~~ Profile Screen Show is called at {Time.frameCount}");

            tabController.ShowTab(2, Inventory.Instance.worldState.Value.lawyerFinished); 

            base.Show(onComplete);

            tabController.OnShow();
            
            if (showDevelopers)
            {
                tabController.ActivateTab(3);
                showDevelopers = false;
            }

            UpdateName();
            
            var ftueState = Inventory.Instance.ftueState.Value;
            
            if ((!ftueState.GetFTUE(FTUEType.PROFILE_SCREEN_ITEMS)
                ||!ftueState.GetFTUE(FTUEType.PROFILE_SCREEN_ITEM_INFO)) 
                && ftueState.needShowProfileItemsFtue)
            {
                UIManager.Instance.FTUEWidget.Show(() =>
                {
                    UIManager.Instance.FTUEWidget.PresentFTUE(tabController.tabToggles[1].gameObject, FTUEType.PROFILE_SCREEN_ITEMS);
                });
            }
            else if (!ftueState.GetFTUE(FTUEType.PROFILE_SCREEN_ADVICES) &&
                     ftueState.needShowProfileUnicornAdvicesFtue)
            {
                UIManager.Instance.FTUEWidget.Show(() =>
                {
                    UIManager.Instance.FTUEWidget.PresentFTUE(tabController.tabToggles[0].gameObject, FTUEType.PROFILE_SCREEN_ADVICES);
                });
            }
            else if (!ftueState.GetFTUE(FTUEType.PROFILE_SCREEN_LAWYER_ADVICES) &&
                     ftueState.needShowProfileLawyerAdvicesFtue)
            {
                UIManager.Instance.FTUEWidget.Show(() =>
                {
                    UIManager.Instance.FTUEWidget.PresentFTUE(tabController.tabToggles[2].gameObject,
                        FTUEType.PROFILE_SCREEN_LAWYER_ADVICES);
                });
            }
        }

        public void UpdateName()
        {
            playerNameText.text = Inventory.Instance.playerState.Value.name;
        }

        public override void Hide(Action onComplete = null)
        {
            base.Hide(onComplete);
        }
    }
}