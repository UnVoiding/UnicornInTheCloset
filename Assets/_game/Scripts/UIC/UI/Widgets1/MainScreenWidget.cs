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
        private TMP_Text playerNameText;
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
                for (int i = 0; i < companionBtns.Count; i++)
                {
                    companionBtns[i].UpdateState();
                }

                if (!Inventory.Instance.playerState.Value.nameEntered)
                {
                    Debug.LogError("88888888888 Showing RenamePlayerWidget");
                    UIManager.Instance.GetWidget<RenamePlayerWidget>().ShowFirstTime();
                }

                UpdateName();
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
    }
}


