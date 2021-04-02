using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace RomenoCompany
{
    public class PlayerProfileAdvicesTab : Tab
    {
        [                           Header("PlayerProfileAdvicesTab"), FoldoutGroup("References")]
        public AdviceSpoiler adviceControlPfb;

        [                              Header("PlayerProfileAdvicesTab"), FoldoutGroup("Runtime")]
        public List<AdviceSpoiler> advices;

        
        public void Populate()
        {
            foreach (var adviceState in Inventory.Instance.worldState.Value.adviceStates)
            {
                AdviceSpoiler s = Instantiate(adviceControlPfb, contentRoot);
                s.adviceState = adviceState;
                s.Init();
                s.SetCaption(adviceState.adviceData.caption);
                s.SetSpoilerText(adviceState.adviceData.text);
                advices.Add(s);
            }
        }

        public void OnShow()
        {
            foreach (var a in advices)
            {
                a.gameObject.SetActive(a.adviceState.found);
            }
        }
    }
}

