using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace RomenoCompany
{
    public class CompanionInfoInfoTab : Tab
    {
        [                           Header("CompanionInfoInfoTab"), FoldoutGroup("References")] 
        public TMP_Text name;
        [                                                           FoldoutGroup("References")] 
        public TMP_Text characteristics;
        [                                                           FoldoutGroup("References")] 
        public TMP_Text description;
        
        
        public void Populate(CompanionData companion)
        {
            name.text = companion.name;
            if (companion.formattedCharacteristics == null)
            {
                companion.formattedCharacteristics = $"<b>Класс:</b> {companion.klass}\n<b>Предмет:</b> {companion.gameItem}\n<b>Особенный навык:</b> {companion.skill}\n<b>Уязвимость:</b> {companion.vulnerability}\n<b>Цель:</b> {companion.goal}";
            }
            characteristics.text = companion.formattedCharacteristics;
            description.text = companion.description;

            float esw = LayoutManager.Instance.esw;
            var margin = LayoutManager.Instance.defaultMargins;
            
            characteristics.fontSize = esw;
            characteristics.margin = margin;
            
            description.fontSize = esw;
            description.margin = margin;
        }
    }
}

