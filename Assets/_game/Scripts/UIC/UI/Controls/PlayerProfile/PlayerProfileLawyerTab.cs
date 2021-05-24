using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace RomenoCompany
{
    public class PlayerProfileLawyerTab : Tab
    {
        [                           Header("PlayerProfileLawyerTab"), FoldoutGroup("References")]
        public AdviceSpoiler lawyerAdviceControlPfb;

        [                             Header("PlayerProfileLawyerTab"), FoldoutGroup("Settings")]
        public int frameUpdateOffset = 0;
        
        [                              Header("PlayerProfileLawyerTab"), FoldoutGroup("Runtime")]
        public List<AdviceSpoiler> advices;
        [                                                                FoldoutGroup("Runtime")]
        public RectTransform contentRootRectTransform;
        [                                                                FoldoutGroup("Runtime")]
        private int forceUpdateFrame = -1;

        public override void Init(TabToggle tabToggle, TabController controller)
        {
            base.Init(tabToggle, controller);

            contentRootRectTransform = contentRoot.GetComponent<RectTransform>();
        }
        
        private void Update()
        {
            if (Time.frameCount == forceUpdateFrame)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(contentRootRectTransform);
                forceUpdateFrame = -1;
            }
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

            if (activate)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(contentRootRectTransform);
                LayoutRebuilder.ForceRebuildLayoutImmediate(contentRootRectTransform);
                LayoutRebuilder.ForceRebuildLayoutImmediate(contentRootRectTransform);

                // StartCoroutine(ForceRebuildEndOfFrame());
                // forceUpdateFrame = Time.frameCount + frameUpdateOffset;
            }
        }
        
        private IEnumerator ForceRebuildEndOfFrame()
        {
            yield return new WaitForEndOfFrame();
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentRootRectTransform);
        }
        
        public void OnSpoilerSwitch()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentRootRectTransform);
        }
    }
}

