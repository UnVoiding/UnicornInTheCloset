using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Video;

namespace RomenoCompany
{
    [CreateAssetMenu(
        fileName = "TableImages", 
        menuName = "UIC/TableImages", 
        order = 59)]
    public class TableImages : SerializedScriptableObject
    {
        public Dictionary<string, Sprite> images;
    }
}