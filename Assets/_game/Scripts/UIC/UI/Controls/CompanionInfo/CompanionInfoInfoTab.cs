using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace RomenoCompany
{
    public class CompanionInfoInfoTab : Tab
    {
        [FormerlySerializedAs("name")] [                           Header("CompanionInfoInfoTab"), FoldoutGroup("References")] 
        public TMP_Text nameText;
        [                                                           FoldoutGroup("References")] 
        public TMP_Text characteristics;
        [                                                           FoldoutGroup("References")] 
        public TMP_Text description;
        
        
        public void Populate(CompanionData companion)
        {
            nameText.text = companion.name;
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

        protected override void OnActivate(bool activate)
        {
            base.OnActivate(activate);

            var w = UIManager.Instance.GetWidget<CompanionInfoWidget>();

            if (activate && w.shown)
            {
                var ftueState = Inventory.Instance.ftueState.Value;
                if (!ftueState.GetFTUE(FTUEType.COMPANION_SELECTION_INFO_TAB)
                    && ftueState.needShowCompanionSelection)
                {
                    UIManager.Instance.FTUEWidget.WithdrawFTUE();
                    ftueState.SetFTUE(FTUEType.COMPANION_SELECTION_INFO_TAB, true);
                    Inventory.Instance.ftueState.Save();

                    var infoWidget = UIManager.Instance.GetWidget<CompanionInfoWidget>();
                    UIManager.Instance.FTUEWidget.PresentFTUE(
                        infoWidget.talkBtn.gameObject,
                        FTUEType.COMPANION_SELECTION2);
                    infoWidget.talkBtn.text.SetAllDirty();
                }
            }
        }
    }
}

