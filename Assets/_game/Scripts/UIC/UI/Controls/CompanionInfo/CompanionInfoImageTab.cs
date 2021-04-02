using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace RomenoCompany
{
    public class CompanionInfoImageTab : Tab
    {
        [                           Header("CompanionInfoImageTab"), FoldoutGroup("References")]
        public Image image;

        
        public void Populate(CompanionData companion)
        {
            SetImage(companion.fullLengthImage);
        }

        public void SetImage(Sprite sprite)
        {
            image.sprite = sprite;
        }
    }
}

