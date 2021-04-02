using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace RomenoCompany
{
    public class PlayerProfileAbout : Tab
    {
        [                           Header("PlayerProfileGameItems"), FoldoutGroup("Runtime")]
        public Spoiler adviceControlPfb;
        
        public void Populate()
        {
            foreach (var adviceState in Inventory.Instance.worldState.Value.adviceStates)
            {
                Spoiler s = Instantiate(adviceControlPfb, contentRoot);
                s.Init();
                s.SetCaption(adviceState.adviceData.caption);
                s.SetSpoilerText(adviceState.adviceData.text);
            }
        }
    }
}

