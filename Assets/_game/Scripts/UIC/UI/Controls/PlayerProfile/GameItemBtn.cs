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
                var w = UIManager.Instance.GetWidget<GameItemUnlockWidget>();
                w.ShowForItem(this.itemState.Data, false);
                
                var ftueState = Inventory.Instance.ftueState.Value;
                if (!ftueState.GetFTUE(FTUEType.PROFILE_SCREEN_ITEM_INFO)
                    && ftueState.needShowProfileItemsFtue)
                {
                    UIManager.Instance.FTUEWidget.WithdrawFTUE();
                    ftueState.SetFTUE(FTUEType.PROFILE_SCREEN_ITEM_INFO, true);
                    Inventory.Instance.ftueState.Save();
                    UIManager.Instance.FTUEWidget.Hide();
                }
            });
        }
    }
}