using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace RomenoCompany
{
    public class PlayerProfileGameItemsTab : Tab
    {
        [                                 Header("Player Profile Game Items Tab"), FoldoutGroup("References")]
        public GameItemBtn gameItemPfb;

        [                   Header("Player Profile Game Items Tab"), SerializeField, FoldoutGroup("Settings")] 
        private int btnsPerRow = 3;

        [                                                                             FoldoutGroup("Runtime")]
        public List<GameItemBtn> gameItemBtns;

        
        public void Populate()
        {
            var cr = contentRoot.GetComponent<GridLayoutGroup>();
            
            float compBtnWidth = (int)((UIManager.Instance.canvasRectTransform.rect.width - btnsPerRow * cr.spacing.x - cr.padding.left) / btnsPerRow);
            cr.cellSize = new Vector2(compBtnWidth, compBtnWidth);

            foreach (var itemState in Inventory.Instance.worldState.Value.gameItemStates)
            {
                GameItemBtn gi = Instantiate(gameItemPfb, contentRoot);
                gi.Init(itemState);
                gi.gameObject.SetActive(itemState.found);
                gameItemBtns.Add(gi);
            }
        }

        public override void OnShow()
        {
            foreach (var gi in gameItemBtns)
            {
                gi.gameObject.SetActive(gi.itemState.found);
            }
        }

        protected override void OnActivate(bool activate)
        {
            base.OnActivate(activate);

            if (activate)
            {
                var ftueState = Inventory.Instance.ftueState.Value;
                if ((!ftueState.GetFTUE(FTUEType.PROFILE_SCREEN_ITEMS)
                    || !ftueState.GetFTUE(FTUEType.PROFILE_SCREEN_ITEM_INFO))
                    && ftueState.needShowProfileItemsFtue)
                {
                    UIManager.Instance.FTUEWidget.WithdrawFTUE();
                    ftueState.SetFTUE(FTUEType.PROFILE_SCREEN_ITEMS, true);
                    Inventory.Instance.ftueState.Save();

                    UIManager.Instance.FTUEWidget.PresentFTUE(gameItemBtns[0].gameObject, FTUEType.PROFILE_SCREEN_ITEM_INFO);
                }        
            }
        }
    }
}

