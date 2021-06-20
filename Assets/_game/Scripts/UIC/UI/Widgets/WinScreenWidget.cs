using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using TMPro;
using Sirenix.OdinInspector;
using DG.Tweening;
using Button = UnityEngine.UI.Button;

namespace RomenoCompany
{
    public class WinScreenWidget : Widget
    {
        #region Fields
        [                         Header("Win Screen Widget"), SerializeField, FoldoutGroup("References")] 
        private Button developersBtn;
        [                                                       SerializeField, FoldoutGroup("References")] 
        private TMP_Text captionText;
        [                                                       SerializeField, FoldoutGroup("References")] 
        private TMP_Text congratsText;
        [                                                       SerializeField, FoldoutGroup("References")] 
        private TMP_Text developersBtnText;
        [                                                       SerializeField, FoldoutGroup("References")] 
        private VerticalLayoutGroup vertGroup;
        
        // [                            Header("Win Screen Widget"), SerializeField, FoldoutGroup("Settings")] 
        // private float typingAnimationSpeed = 0.33f;
        
        // [           Header("Win Screen Widget"), NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")] 
        // private float screenWidth;
        // [                                        NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")] 
        // public float em;
        // [                                        NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")] 
        // public Vector4 margins;

        
        #endregion
        
        public override void InitializeWidget()
        {
            base.InitializeWidget();

            widgetType = WidgetType.WIN;
            developersBtn.onClick.AddListener(() =>
            {
                UIManager.Instance.GoToComposition(Composition.PLAYER_PROFILE);
                UIManager.Instance.GetWidget<ProfileScreenWidget>().showDevelopers = true;
            });
        }

        public override void Show(Action onComplete = null)
        {
            base.Show(onComplete);

            var lm = LayoutManager.Instance;
            
            captionText.fontSize = 3 * lm.esw;
            congratsText.margin = lm.defaultMargins;

            congratsText.fontSize = lm.esw;
            congratsText.margin = lm.defaultMargins;
            
            developersBtnText.fontSize = lm.esw;
            developersBtnText.margin = lm.defaultMargins;

            vertGroup.spacing = lm.esw;
            vertGroup.padding.top = (int)lm.esw;
            vertGroup.padding.bottom = (int)lm.esw;
        }

        public override void Hide(Action onComplete = null)
        {
            base.Hide(onComplete);
        }
    }
}