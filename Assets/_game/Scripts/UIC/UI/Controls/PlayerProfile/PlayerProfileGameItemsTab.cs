using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace RomenoCompany
{
    public class PlayerProfileGameItemsTab : Tab
    {
        [                        Header("PlayerProfileGameItems"), FoldoutGroup("References")]
        public GameItemBtn gameItemPfb;

        [                                                             FoldoutGroup("Runtime")]
        public List<GameItemBtn> gameItemBtns;

        
        public void Populate()
        {
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

