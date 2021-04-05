using System;

namespace RomenoCompany
{
    [Serializable]
    public class PlayerItemState
    {
        public PlayerItemData itemData;
        public bool found;

        public PlayerItemState(PlayerItemData playerItemData)
        {
            itemData = playerItemData;
        }
    }
}
