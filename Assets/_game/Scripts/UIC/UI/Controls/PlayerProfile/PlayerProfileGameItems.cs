using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace RomenoCompany
{
    public class PlayerProfileGameItems : Tab
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
                gameItemBtns.Add(gi);
            }
        }

        public void OnShow()
        {
            foreach (var gi in gameItemBtns)
            {
                gi.gameObject.SetActive(gi.itemState.found);
            }
        }
    }
}

