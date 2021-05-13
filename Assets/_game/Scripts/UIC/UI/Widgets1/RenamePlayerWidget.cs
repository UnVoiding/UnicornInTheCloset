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
                    Inventory.Instance.playerState.Value.nameEntered = true;
                    Inventory.Instance.playerState.Save();
                    
                    UIManager.Instance.GetWidget<MainScreenWidget>().UpdateName();
                    UIManager.Instance.GetWidget<ProfileScreenWidget>().UpdateName();

                    Hide(ShowCancel);
                }
            });
            
            cancelBtn.onClick.AddListener(() =>
            {
                Hide(ShowCancel);
            });
        }

        public override void Show(Action onComplete = null)
        {
            base.Show(onComplete);

            var lm = LayoutManager.Instance;

            vertGroup.spacing = lm.esw;
            vertGroup.padding.top = (int)lm.esw;
            vertGroup.padding.bottom = (int)lm.esw;
            vertGroup.padding.left = (int) lm.esw;
            vertGroup.padding.right = (int) lm.esw;

            captionText.fontSize = lm.esw;

            inputField.pointSize = lm.esw;
            inputField.textComponent.margin = lm.defaultMargins;
            (inputField.placeholder as TMP_Text).margin = lm.defaultMargins;

            horizGroup.spacing = lm.esw;
            horizGroup.padding.top = (int) lm.esw;

            okBtnText.fontSize = lm.esw;
            okBtnText.margin = lm.defaultMargins;
            
            cancelBtnText.fontSize = lm.esw;
            cancelBtnText.margin = lm.defaultMargins;
        }

        public void ShowFirstTime()
        {
            captionText.text = "Назовите своего персонажа";
            cancelBtn.gameObject.SetActive(false);
            
            Show();
        }

        private void ShowCancel()
        {
            captionText.text = "Изменить имя";
            cancelBtn.gameObject.SetActive(true);
        }
   }
}