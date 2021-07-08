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
    public class GameItemUnlockWidget : Widget
    {
        [                                         Header("UnlockedGameItemWidget"), FoldoutGroup("References")] 
        public VerticalLayoutGroup mainPanel;
        [                                                                           FoldoutGroup("References")] 
        public TMP_Text itemName;
        [                                                                           FoldoutGroup("References")] 
        public TMP_Text itemDescription;
        [                                                                           FoldoutGroup("References")] 
        public Image itemImage;
        [                                                                           FoldoutGroup("References")] 
        public TMP_Text youGot;
        [                                                                           FoldoutGroup("References")] 
        public Button closeBtn;
        [                                                                           FoldoutGroup("References")] 
        public TMP_Text closeBtnText;

        [                                    NonSerialized, ShowInInspector, ReadOnly, FoldoutGroup("Runtime")] 
        public Action onClose;
        [                                    NonSerialized, ShowInInspector, ReadOnly, FoldoutGroup("Runtime")] 
        public bool shownAsItemInfo;
        [                                    NonSerialized, ShowInInspector, ReadOnly, FoldoutGroup("Runtime")] 
        public Vector4 closeBtnMargin;

        
        public override void InitializeWidget()
        {
            base.InitializeWidget();

            closeBtnMargin = closeBtnText.margin;

            widgetType = WidgetType.GAME_ITEM;
            closeBtn.onClick.AddListener(() =>
            {
                Hide();

                if (shownAsItemInfo)
                {
                    var ftueState = Inventory.Instance.ftueState.Value;
                    if (!ftueState.GetFTUE(FTUEType.PROFILE_SCREEN_ADVICES) &&
                        ftueState.needShowProfileUnicornAdvicesFtue)
                    {
                        UIManager.Instance.FTUEWidget.Show();
                        UIManager.Instance.FTUEWidget.PresentFTUE(
                            UIManager.Instance.GetWidget<PlayerProfileScreenWidget>().tabController.tabToggles[0].gameObject, 
                            FTUEType.PROFILE_SCREEN_ADVICES);
                    }
                }
            });
        }

        public void ShowForItem(PlayerItemData item, bool showYouGot)
        {
            shownAsItemInfo = !showYouGot;
            
            itemName.text = item.name;
            itemDescription.text = item.description;
            itemImage.sprite = item.image;
            youGot.gameObject.SetActive(showYouGot);
            
            float esw = LayoutManager.Instance.esw;
            var defaultMargins = LayoutManager.Instance.defaultMargins;

            itemDescription.fontSize = esw;
            itemDescription.margin = new Vector4(defaultMargins.x, defaultMargins.y, defaultMargins.z, defaultMargins.w + 20);

            closeBtnText.fontSize = esw;
            closeBtnText.margin = closeBtnMargin + defaultMargins;

            mainPanel.padding.left = (int)defaultMargins.x;
            mainPanel.padding.right = (int)defaultMargins.z;
            
            Show();

            var rt = mainPanel.transform as RectTransform;
            LayoutRebuilder.ForceRebuildLayoutImmediate(rt);
            LayoutRebuilder.ForceRebuildLayoutImmediate(rt);
            LayoutRebuilder.ForceRebuildLayoutImmediate(rt);
        }

        public override void Show(System.Action onComplete = null)
        {
            base.Show(onComplete);
        }

        public override void Hide(Action onComplete = null)
        {
            if (!hidding)
            {
                onClose?.Invoke();
            }

            base.Hide(onComplete);
        }
    }
}

