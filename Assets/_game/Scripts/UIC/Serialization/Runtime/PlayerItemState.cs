using System;

namespace RomenoCompany
{
    [Serializable]
    public class PlayerItemState
    {
        public PlayerItemData.ItemID id;
        public bool found;

        private PlayerItemData data;
        
        public PlayerItemData Data
        {
            get
            {
                if (data == null)
                {
                    data = DB.Instance.gameItems.items.Find((d) => d.id == id);
                }

                return data;
            }
        }

        public PlayerItemState(PlayerItemData playerData)
        {
            id = playerData.id;
            data = playerData;
        }
    }
}
