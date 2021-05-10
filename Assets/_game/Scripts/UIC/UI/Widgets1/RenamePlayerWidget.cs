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
    public class RenamePlayerWidget : Widget
    {
        #region Fields
        
        [                       Header("Rename Player Widget"), SerializeField, FoldoutGroup("References")] 
        private TMP_Text captionText;
        [                                                       SerializeField, FoldoutGroup("References")] 
        private VerticalLayoutGroup vertGroup;
        [                                                       SerializeField, FoldoutGroup("References")] 
        private HorizontalLayoutGroup horizGroup;
        [                                                       SerializeField, FoldoutGroup("References")] 
        private Button okBtn;
        [                                                       SerializeField, FoldoutGroup("References")] 
        private TMP_Text okBtnText;
        [                                                       SerializeField, FoldoutGroup("References")] 
        private Button cancelBtn;
        [                                                       SerializeField, FoldoutGroup("References")] 
        private TMP_Text cancelBtnText;
        [                                                       SerializeField, FoldoutGroup("References")] 
        private TMP_InputField inputField;
        
        // [                            Header("Rename Player Widget"), SerializeField, FoldoutGroup("Settings")] 
        // private float typingAnimationSpeed = 0.33f;
        
        #endregion
        
        public override void InitializeWidget()
        {
            base.InitializeWidget();

            widgetType = WidgetType.RENAME_PLAYER;
            okBtn.onClick.AddListener(() =>
            {
                string trimmedText = inputField.text.Trim();
                if (trimmedText.Length != 0)
                {
                    Inventory.Instance.playerState.Value.name = trimmedText;
                    Hide();
                }
            });
            
            cancelBtn.onClick.AddListener(() =>
            {
                Hide(() =>
                {
                    captionText.text = "Изменить имя";
                });
            });
        }

        public override void Show(Action onComplete = null)
        {
            base.Show(onComplete);

            var lm = LayoutManager.Instance;

            captionText.fontSize = lm.esw;

            vertGroup.spacing = lm.esw;
            vertGroup.padding.top = (int)lm.esw;
            vertGroup.padding.bottom = (int)lm.esw;

            inputField.pointSize = lm.esw;
            inputField.textComponent.margin = lm.defaultMargins;
            (inputField.placeholder as TMP_Text).margin = lm.defaultMargins;

            horizGroup.spacing = lm.esw;
            horizGroup.padding.left = (int) lm.esw;
            horizGroup.padding.top = (int) lm.esw;
            horizGroup.padding.right = (int) lm.esw;
            horizGroup.padding.bottom = (int) lm.esw;

            okBtnText.fontSize = lm.esw;
            okBtnText.margin = lm.defaultMargins;
            
            cancelBtnText.fontSize = lm.esw;
            cancelBtnText.margin = lm.defaultMargins;
        }

        public void ShowFirstTime()
        {
            captionText.text = "Назовите своего персонажа";
            
            Show();
        }

        public override void Hide(Action onComplete = null)
        {
            base.Hide(onComplete);
        }
    }
}