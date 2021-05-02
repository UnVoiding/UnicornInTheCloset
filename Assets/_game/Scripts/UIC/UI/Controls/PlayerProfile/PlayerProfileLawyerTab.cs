using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace RomenoCompany
{
    public class PlayerProfileLawyerTab : Tab
    {
        [                           Header("PlayerProfileLawyer"), FoldoutGroup("References")]
        public AdviceSpoiler lawyerAdviceControlPfb;
        
        [                              Header("PlayerProfileAdvicesTab"), FoldoutGroup("Runtime")]
        public List<AdviceSpoiler> advices;
        [                                                                 FoldoutGroup("Runtime")]
        public RectTransform contentRootRectTransform;

        public override void Init(TabToggle tabToggle, TabController controller)
        {
            base.Init(tabToggle, controller);

            contentRootRectTransform = contentRoot.GetComponent<RectTransform>();
        }
        
        public void Populate()
        {
            foreach (var adviceData in DB.Instance.lawyerAdvices.items)
            {
                AdviceSpoiler s = Instantiate(lawyerAdviceControlPfb, contentRoot);
                s.adviceState = new AdviceState()
                {
                    id = adviceData.id,
                    found = true
                };
                s.onSwitchOnOff += OnSpoilerSwitch;
                s.Init();
                s.SetCaption(adviceData.caption);
                s.SetSpoilerText(adviceData.text);
                advices.Add(s);
            }
        }
        
        public void OnDestroy()
        {
            foreach (var a in advices)
            {
                if (a != null)
                {
                    a.onSwitchOnOff -= OnSpoilerSwitch;
                }
            }
        }
        
        protected override void OnActivate(bool activate)
        {
            base.OnActivate(activate);
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentRootRectTransform);
        }
        
        public void OnSpoilerSwitch()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentRootRectTransform);
        }
    }
}

