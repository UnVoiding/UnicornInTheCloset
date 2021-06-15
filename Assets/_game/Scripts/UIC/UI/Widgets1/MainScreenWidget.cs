using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using Sirenix.OdinInspector;
using DG.Tweening;
using Sirenix.OdinInspector.Editor.Drawers;
using Button = UnityEngine.UI.Button;

namespace RomenoCompany
{
    public class MainScreenWidget : Widget
    {
        [                            Header("Main Screen Widget"), SerializeField, FoldoutGroup("References")] 
        private Button profileBtn;
        [                                                          SerializeField, FoldoutGroup("References")] 
        private Button testFTUE;
        [                                                          SerializeField, FoldoutGroup("References")] 
        private TMP_Text playerNameText;
        [                                                          SerializeField, FoldoutGroup("References")] 
        private ScrollRect scroll;
        [                                                          SerializeField, FoldoutGroup("References")] 
        private GridLayoutGroup contentRoot;
        [                                                          SerializeField, FoldoutGroup("References")] 
        private CompanionBtn companionBtnPfb;

        [                              Header("Main Screen Widget"), SerializeField, FoldoutGroup("Settings")] 
        private int btnsPerRow;

        [                                                    NonSerialized, ReadOnly, FoldoutGroup("Runtime")] 
        public List<CompanionBtn> companionBtns;
        
        
        public override void InitializeWidget()
        {
            base.InitializeWidget();

            widgetType = WidgetType.MAIN;
            profileBtn.onClick.AddListener(() =>
            {
                UIManager.Instance.GoToComposition(Composition.PLAYER_PROFILE);

                if (!Inventory.Instance.ftueState.Value.GetFTUE(FTUEType.PROFILE_SCREEN) 
                    && (Inventory.Instance.ftueState.Value.needShowProfileItemsFtue 
                        || Inventory.Instance.ftueState.Value.needShowProfileLawyerAdvicesFtue
                        || Inventory.Instance.ftueState.Value.needShowProfileUnicornAdvicesFtue))
                {
                    UIManager.Instance.FTUEWidget.WithdrawFTUE(profileBtn.gameObject, FTUEType.PROFILE_SCREEN);
                    
                    // UIManager.Instance.FTUEWidget.HideTap();
                    // UIManager.Instance.FTUEWidget.EndHighlight(profileBtn.gameObject);
                    // UIManager.Instance.FTUEWidget.HideTooltip();
                    // UIManager.Instance.FTUEWidget.Hide();
                }
            });

            float compBtnWidth = (int)((UIManager.Instance.canvasRectTransform.rect.width - btnsPerRow * contentRoot.spacing.x - contentRoot.padding.left) / btnsPerRow);
            contentRoot.cellSize = new Vector2(compBtnWidth, compBtnWidth);

            companionBtns = new List<CompanionBtn>();
            foreach (var companionState in Inventory.Instance.worldState.Value.companionStates)
            {
                if (companionState.Data.enabled)
                {
                    CompanionBtn btn = Instantiate(companionBtnPfb, contentRoot.transform);
                    btn.size = compBtnWidth; 
                    btn.Init(companionState);
                    companionBtns.Add(btn);
                }
            }
        }

        public override void Show(System.Action onComplete = null)
        {
            OnShow();

            base.Show(onComplete);
        }

        public override void ShowInstant()
        {
            OnShow();

            base.ShowInstant();
        }

        public void OnShow()
        {
            if (!shown && !showing)
            {
                UpdateCompanionBtns();

                var ftueState = Inventory.Instance.ftueState.Value;
                var ftueWidget = UIManager.Instance.FTUEWidget;
                
                if (!ftueState.GetFTUE(FTUEType.PROFILE_SCREEN) 
                    && (ftueState.needShowProfileItemsFtue 
                        || ftueState.needShowProfileLawyerAdvicesFtue
                        || ftueState.needShowProfileUnicornAdvicesFtue))
                {
                    ftueWidget.Show(ShowProfileScreenBtnFtue);
                }

                if (!ftueState.GetFTUE(FTUEType.COMPANION_SELECTION1) 
                    && ftueState.needShowCompanionSelection)
                {
                    ftueWidget.Show(ShowSelectCompanionFtue);
                }

                if (!Inventory.Instance.playerState.Value.nameEntered)
                {
                    Debug.LogWarning("~~~ Showing RenamePlayerWidget");
                    UIManager.Instance.GetWidget<RenamePlayerWidget>().ShowFirstTime();
                }

                UpdateName();
            }
        }

        public void ShowProfileScreenBtnFtue()
        {
            UIManager.Instance.FTUEWidget.PresentFTUE(profileBtn.gameObject, FTUEType.PROFILE_SCREEN);
                        
            // UIManager.Instance.FTUEWidget.ShowFTUE(profileBtn.gameObject, FTUEType.PROFILE_SCREEN);
            // UIManager.Instance.FTUEWidget.BeginHighlight(profileBtn.gameObject);
            // UIManager.Instance.FTUEWidget.ShowTap(profileBtn.transform);
            // UIManager.Instance.FTUEWidget.ShowToolTip("test gavneco", new Vector2(500, 500), Vector2.zero);
        }

        public void ShowSelectCompanionFtue()
        {
            UIManager.Instance.FTUEWidget.PresentFTUE(companionBtns[0].gameObject, FTUEType.COMPANION_SELECTION1);
        }

        public void UpdateCompanionBtns()
        {
            for (int i = 0; i < companionBtns.Count; i++)
            {
                companionBtns[i].UpdateState();
            }
        }

        public void UpdateName()
        {
            if (Inventory.Instance.playerState.Value.nameEntered)
            {
                playerNameText.text = Inventory.Instance.playerState.Value.name;
            }
        }

        public override void Hide(Action onComplete = null)
        {
            base.Hide(onComplete);
        }

        public void EnableScroll(bool enable)
        {
            scroll.vertical = enable;
        }
    }
}


