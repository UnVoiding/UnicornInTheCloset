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
        
        public override void InitializeWidget()
        {
            base.InitializeWidget();

            widgetType = WidgetType.WIN;
            developersBtn.onClick.AddListener(() =>
            {
                Hide();
                UIManager.Instance.GetWidget<PlayerProfileScreenWidget>().showDevelopers = true;
                UIManager.Instance.GoToComposition(Composition.PLAYER_PROFILE);
            });
        }

        public override void Show(Action onComplete = null)
        {
            base.Show(onComplete);

            var lm = LayoutManager.Instance;
            
            captionText.fontSize = 3 * lm.esw;

            congratsText.fontSize = 1.75f * lm.esw;
            
            developersBtnText.fontSize = lm.esw;

            vertGroup.spacing = lm.esw;
            vertGroup.padding.top = (int)lm.esw;
            vertGroup.padding.bottom = (int)lm.esw;
            vertGroup.padding.left = (int)lm.defaultMargins.x;
            vertGroup.padding.right = (int)lm.defaultMargins.z;
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(vertGroup.transform as RectTransform);
        }

        public override void Hide(Action onComplete = null)
        {
            base.Hide(onComplete);
        }
    }
}