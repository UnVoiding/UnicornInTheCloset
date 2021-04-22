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
    public class MainScreenWidget : Widget
    {
        [                            Header("Main Screen Widget"), SerializeField, FoldoutGroup("References")] 
        private Button profileBtn;
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

            float compBtnWidth = (int)((Screen.width - btnsPerRow * contentRoot.spacing.x - contentRoot.padding.left) / btnsPerRow);
            contentRoot.cellSize = new Vector2(compBtnWidth, compBtnWidth);

            companionBtns = new List<CompanionBtn>();
            foreach (var companionState in Inventory.Instance.worldState.Value.companionStates)
            {
                if (companionState.Data.enabled)
                {
                    CompanionBtn btn = Instantiate(companionBtnPfb, contentRoot.transform);
                    btn.Init(companionState);
                    companionBtns.Add(btn);
                }
            }
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


