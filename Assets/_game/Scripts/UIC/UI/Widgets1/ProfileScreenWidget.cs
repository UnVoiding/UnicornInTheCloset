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

        //// RUNTIME 
        [                                                NonSerialized, ReadOnly, FoldoutGroup("Runtime")] 
        public bool qq;
        
        public override void InitializeWidget()
        {
            widgetType = WidgetType.PLAYER_PROFILE;
            
            backBtn.onClick.AddListener(() =>
            {
                UIManager.Instance.GoToComposition(Composition.PLAYER_PROFILE);
            });
            
            renamePlayerBtn.onClick.AddListener(() =>
            {
                Widget renameWid = UIManager.Instance.GetWidget(WidgetType.RENAME_PLAYER);
                renameWid.Show();
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