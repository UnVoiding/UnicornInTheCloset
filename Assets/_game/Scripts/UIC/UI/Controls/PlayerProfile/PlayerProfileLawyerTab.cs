using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace RomenoCompany
{
    public class PlayerProfileLawyerTab : Tab
    {
        [                           Header("PlayerProfileLawyer"), FoldoutGroup("References")]
        public AdviceSpoiler lawyerAdviceControlPfb;
        
        public void Populate()
        {
            foreach (var adviceData in DB.Instance.lawyerAdvices.items)
            {
                AdviceSpoiler s = Instantiate(lawyerAdviceControlPfb, contentRoot);
                s.adviceState = new AdviceState()
                {
                    adviceData = adviceData,
                    found = true
                };
                s.Init();
                s.SetCaption(adviceData.caption);
                s.SetSpoilerText(adviceData.text);
            }
        }
    }
}

