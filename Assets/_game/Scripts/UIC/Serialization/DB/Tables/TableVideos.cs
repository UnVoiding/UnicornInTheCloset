using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Video;

namespace RomenoCompany
{
    [CreateAssetMenu(
        fileName = "TableVideos", 
        menuName = "UIC/TableVideos", 
        order = 59)]
    public class TableVideos : SerializedScriptableObject
    {
        public Dictionary<string, VideoClip> items;
    }
}

