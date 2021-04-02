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
    }
}