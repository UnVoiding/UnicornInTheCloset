using System;
using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Newtonsoft.Json;

namespace RomenoCompany
{
    [CreateAssetMenu(
        fileName = "TableAdvices", 
        menuName = "UIC/TableAdvices", 
        order = 60)]
    public class TableLawyerAdvices : SerializedScriptableObject
    {
        public List<AdviceData> items;
    }
}
