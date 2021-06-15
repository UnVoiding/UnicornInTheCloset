using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace RomenoCompany
{
    public class GameItemBtn : MonoBehaviour
    {
        [                                                     FoldoutGroup("References")]
        public Button showItemInfoBtn;
        [                                                     FoldoutGroup("References")]
        public Image image;
        
        [              NonSerialized, ShowInInspector, ReadOnly, FoldoutGroup("Runtime")]
        public PlayerItemState itemState;


        public void Init(PlayerItemState itemState)
        {
            this.itemState = itemState;
            image.sprite = itemState.Data.image;
            showItemInfoBtn.onClick.AddListener(() =>
            {
                var w = UIManager.Instance.GetWidget<UnlockedGameItemWidget>();
                w.ShowForItem(this.itemState.Data, false);
                
                var ftueState = Inventory.Instance.ftueState.Value;
                if (!ftueState.GetFTUE(FTUEType.PROFILE_SCREEN_ITEM_INFO)
                    && ftueState.needShowProfileItemsFtue)
                {
                    UIManager.Instance.FTUEWidget.WithdrawFTUE(gameObject, FTUEType.PROFILE_SCREEN_ITEM_INFO);
                    ftueState.SetFTUE(FTUEType.PROFILE_SCREEN_ITEM_INFO, true);
                    Inventory.Instance.ftueState.Save();

                    if (ftueState.needShowProfileUnicornAdvicesFtue)
                    {
                        UIManager.Instance.FTUEWidget.PresentFTUE(
                            UIManager.Instance.GetWidget<ProfileScreenWidget>().tabController.tabToggles[0].gameObject, 
                            FTUEType.PROFILE_SCREEN_ADVICES);
                    }
                    else
                    {
                        UIManager.Instance.FTUEWidget.Hide();
                    }
                }
            });
        }
    }
}