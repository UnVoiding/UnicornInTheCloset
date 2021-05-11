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
    }
}

