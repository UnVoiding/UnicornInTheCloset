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
    public class UnlockedGameItemWidget : Widget
    {
        [                                         Header("UnlockedGameItemWidget"), FoldoutGroup("References")] 
        public TMP_Text itemName;
        [                                                                           FoldoutGroup("References")] 
        public TMP_Text itemDescription;
        [                                                                           FoldoutGroup("References")] 
        public Image itemImage;
        [                                                                           FoldoutGroup("References")] 
        public TMP_Text youGot;
        [                                                                           FoldoutGroup("References")] 
        public Button closeBtn;

        [                                    NonSerialized, ShowInInspector, ReadOnly, FoldoutGroup("Runtime")] 
        public Action onClose;

        
        public override void InitializeWidget()
        {
            base.InitializeWidget();

            widgetType = WidgetType.GAME_ITEM;
            closeBtn.onClick.AddListener(() =>
            {
                Hide();
            });
        }

        public void ShowForItem(PlayerItemData item, bool showYouGot)
        {
            itemName.text = item.name;
            itemDescription.text = item.description;
            itemImage.sprite = item.image;
            youGot.gameObject.SetActive(showYouGot);
            
            Show();
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

