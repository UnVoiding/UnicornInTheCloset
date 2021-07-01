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
        [                                                       SerializeField, FoldoutGroup("References")] 
        private Button okBtnSingle;
        [                                                       SerializeField, FoldoutGroup("References")] 
        private TMP_Text okBtnSingleText;
        
        // [                            Header("Rename Player Widget"), SerializeField, FoldoutGroup("Settings")] 
        // private float typingAnimationSpeed = 0.33f;
        
        public override void InitializeWidget()
        {
            base.InitializeWidget();

            widgetType = WidgetType.RENAME_PLAYER;
            okBtnSingle.onClick.AddListener(OnOk);

            okBtn.onClick.AddListener(OnOk);

            cancelBtn.onClick.AddListener(() =>
            {
                if (!UIManager.Instance.inputAllowed) return;
            
                Hide(ActivateCancel);
            });

            if (!Inventory.Instance.playerState.Value.nameEntered)
            {
                captionText.text = "Назовите своего персонажа";

                okBtn.gameObject.SetActive(false);
                cancelBtn.gameObject.SetActive(false);
                okBtnSingle.gameObject.SetActive(true);
            }
            else
            {
                captionText.text = "Изменить имя";

                okBtn.gameObject.SetActive(true);
                cancelBtn.gameObject.SetActive(true);
                okBtnSingle.gameObject.SetActive(false);
            }
        }

        public void OnOk()
        {
            if (!UIManager.Instance.inputAllowed) return;
        
            string trimmedText = inputField.text.Trim();
            if (trimmedText.Length != 0)
            {
                Inventory.Instance.playerState.Value.name = trimmedText;
                Inventory.Instance.playerState.Value.nameEntered = true;
                Inventory.Instance.playerState.Save();
                
                UIManager.Instance.GetWidget<MainScreenWidget>().UpdateName();
                UIManager.Instance.GetWidget<ProfileScreenWidget>().UpdateName();
        
                if (okBtnSingle.gameObject.activeInHierarchy)
                {
                    var ftueState = Inventory.Instance.ftueState.Value;
        
                    ftueState.needShowCompanionSelection = true;
                    Inventory.Instance.ftueState.Save();
        
                    if (!ftueState.GetFTUE(FTUEType.COMPANION_SELECTION1)
                        && ftueState.needShowCompanionSelection)
                    {
                        UIManager.Instance.FTUEWidget.Show(() =>
                        {
                            UIManager.Instance.GetWidget<MainScreenWidget>().ShowSelectCompanionFtue();
                        });
                    }
                }
        
                Hide(ActivateCancel);
            }
        }

        public override void Show(Action onComplete = null)
        {
            base.Show(onComplete);

            float esw = LayoutManager.Instance.esw;
            Vector4 defaultMargins = LayoutManager.Instance.defaultMargins;
            
            vertGroup.spacing = esw;
            vertGroup.padding.top = (int)esw;
            vertGroup.padding.bottom = (int)esw;
            vertGroup.padding.left = (int)esw;
            vertGroup.padding.right = (int)esw;
            
            captionText.fontSize = esw;
            
            inputField.pointSize = esw;
            inputField.textComponent.margin = defaultMargins;
            (inputField.placeholder as TMP_Text).margin = defaultMargins;
            
            horizGroup.spacing = esw;
            horizGroup.padding.top = (int)esw;
            
            okBtnText.fontSize = esw;
            okBtnText.margin = defaultMargins;

            okBtnSingleText.fontSize = esw;
            okBtnSingleText.margin = defaultMargins;

            cancelBtnText.fontSize = esw;
            cancelBtnText.margin = defaultMargins;
        }

        public void ShowFirstTime()
        {
            captionText.text = "Назовите своего персонажа";

            okBtn.gameObject.SetActive(false);
            cancelBtn.gameObject.SetActive(false);
            okBtnSingle.gameObject.SetActive(true);
            
            Show();
        }

        private void ActivateCancel()
        {
            captionText.text = "Изменить имя";
            
            okBtn.gameObject.SetActive(true);
            cancelBtn.gameObject.SetActive(true);
            okBtnSingle.gameObject.SetActive(false);
        }
   }
}