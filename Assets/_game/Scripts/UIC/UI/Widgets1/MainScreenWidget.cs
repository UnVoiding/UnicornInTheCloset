using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using Sirenix.OdinInspector;
using DG.Tweening;
using Sirenix.Utilities.Editor;
using Button = UnityEngine.UI.Button;

namespace RomenoCompany
{
    public class MainScreenWidget : Widget
    {
        [Header("Main Screen Widget")]
        //// DATA
        [                                               SerializeField, FoldoutGroup("References")] 
        private Button profileBtn;
        [                                               SerializeField, FoldoutGroup("References")] 
        private Transform contentRoot;
        [                                               SerializeField, FoldoutGroup("References")] 
        private CompanionBtn companionBtnPfb;

        //// RUNTIME 
        [                                                NonSerialized, ReadOnly, FoldoutGroup("Runtime")] 
        public List<CompanionBtn> companionBtns;
        
        
        public override void InitializeWidget()
        {
            widgetType = WidgetType.MAIN;
            profileBtn.onClick.AddListener(() =>
            {
                UIManager.Instance.GoToComposition(Composition.PLAYER_PROFILE);
            });

            companionBtns = new List<CompanionBtn>();
            foreach (var dbCompanion in DB.Instance.companions.items)
            {
                CompanionBtn btn = Instantiate(companionBtnPfb, contentRoot);
                btn.Init(dbCompanion.emotions[0].sprite);
                companionBtns.Add(btn);
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


