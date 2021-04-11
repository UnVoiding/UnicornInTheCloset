using System;
using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace RomenoCompany
{
    [CreateAssetMenu(
        fileName = "TableCompanions", 
        menuName = "UIC/TableCompanions", 
        order = 60)]
    public class TableCompanions : SerializedScriptableObject
    {
        public List<CompanionData> items;
    }
}

