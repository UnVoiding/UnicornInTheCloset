using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sirenix.OdinInspector;

namespace RomenoCompany
{
    [CreateAssetMenu(
    fileName = "TablePlayerItems", 
    menuName = "UIC/TablePlayerItems", 
    order = 59)]
    public class TablePlayerItems : SerializedScriptableObject
    {
        public List<PlayerItemData> items;

        public PlayerItemData GetPlayerItem(PlayerItemData.ItemID id)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].id == id) return items[i];
            }

            return null;
        }
        
        public PlayerItemData GetPlayerItem(string code)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].code == code) return items[i];
            }

            return null;
        }

    }
}

