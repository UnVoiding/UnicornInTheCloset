using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RomenoCompany
{
    public class PlayerProfileAboutTab : Tab
    {
        [                           Header("PlayerProfileAboutTab"), FoldoutGroup("References")]
        public TMP_Text captionText;
        [                                                            FoldoutGroup("References")]
        public TMP_Text mainText;

        // [                             Header("PlayerProfileAdvicesTab"), FoldoutGroup("Settings")]
        // public int frameUpdateOffset = 0;

        // [                              Header("PlayerProfileAdvicesTab"), FoldoutGroup("Runtime")]
        // public List<AdviceSpoiler> advices;
        // [                                                                 FoldoutGroup("Runtime")]
        // public RectTransform contentRootRectTransform;
        // [                                                                 FoldoutGroup("Runtime")]
        // private int forceUpdateFrame = -1;
        
        // public override void Init(TabToggle tabToggle, TabController controller)
        // {
        //     base.Init(tabToggle, controller);
        //
        //     contentRootRectTransform = contentRoot.GetComponent<RectTransform>();
        // }

        // public void Populate()
        // {
        //     Dictionary<CompanionData.ItemID, int> adviceCountByCompanion = new Dictionary<CompanionData.ItemID, int>();
        //     
        //     foreach (var adviceState in Inventory.Instance.worldState.Value.unicornAdvicesState.unicornAdviceStates)
        //     {
        //         AdviceSpoiler s = Instantiate(adviceControlPfb, contentRoot);
        //         s.adviceState = adviceState;
        //         s.Init();
        //         s.onSwitchOnOff += OnSpoilerSwitch;
        //         int adviceCount = adviceCountByCompanion.Get(adviceState.data.companionId);
        //         var cs = Inventory.Instance.worldState.Value.GetCompanion(adviceState.data.companionId);
        //         string spoilerCaption = $"{cs.Data.name}. Совет {adviceCount}";
        //         adviceCountByCompanion[adviceState.data.companionId] = adviceCount + 1; 
        //         s.SetCaption(spoilerCaption);
        //         s.SetSpoilerText(adviceState.data.text);
        //         advices.Add(s);
        //     }
        // }

        // public void OnDestroy()
        // {
        //     foreach (var a in advices)
        //     {
        //         if (a != null)
        //         {
        //             a.onSwitchOnOff -= OnSpoilerSwitch;
        //         }
        //     }
        // }

        public override void OnShow()
        {
            float esw = LayoutManager.Instance.esw;
            var defaultMargins = LayoutManager.Instance.defaultMargins;

            captionText.fontSize = 1.25f * esw;
            captionText.margin = defaultMargins;
            
            mainText.fontSize = esw;
            mainText.margin = defaultMargins;
        }
    }
}

