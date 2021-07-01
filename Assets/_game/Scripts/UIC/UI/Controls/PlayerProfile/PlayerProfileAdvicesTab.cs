using System.Collections;
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

        [                             Header("PlayerProfileAdvicesTab"), FoldoutGroup("Settings")]
        public int frameUpdateOffset = 0;

        [                              Header("PlayerProfileAdvicesTab"), FoldoutGroup("Runtime")]
        public List<AdviceSpoiler> advices;
        [                                                                 FoldoutGroup("Runtime")]
        public RectTransform contentRootRectTransform;
        [                                                                 FoldoutGroup("Runtime")]
        private int forceUpdateFrame = -1;
        
        public override void Init(TabToggle tabToggle, TabController controller)
        {
            base.Init(tabToggle, controller);

            contentRootRectTransform = contentRoot.GetComponent<RectTransform>();
        }

        public void Populate()
        {
            Dictionary<CompanionData.ItemID, int> adviceCountByCompanion = new Dictionary<CompanionData.ItemID, int>();
            
            foreach (var adviceState in Inventory.Instance.worldState.Value.unicornAdvicesState.unicornAdviceStates)
            {
                AdviceSpoiler s = Instantiate(adviceControlPfb, contentRoot);
                s.adviceState = adviceState;
                s.Init();
                s.onSwitchOnOff += OnSpoilerSwitch;
                int adviceCount = adviceCountByCompanion.Get(adviceState.data.companionId);
                var cs = Inventory.Instance.worldState.Value.GetCompanion(adviceState.data.companionId);
                string spoilerCaption = $"{cs.Data.name}. Совет {adviceCount}";
                adviceCountByCompanion[adviceState.data.companionId] = adviceCount + 1; 
                s.SetCaption(spoilerCaption);
                s.SetSpoilerText(adviceState.data.text);
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

        public override void OnShow()
        {
            foreach (var a in advices)
            {
                a.gameObject.SetActive(a.adviceState.found);
            }
        }

        private void Update()
        {
            if (Time.frameCount == forceUpdateFrame)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(contentRootRectTransform);
                forceUpdateFrame = -1;
            }
        }

        protected override void OnActivate(bool activate)
        {
            base.OnActivate(activate);

            if (activate)
            {
                if (advices.Count > 0)
                {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(contentRootRectTransform);
                    LayoutRebuilder.ForceRebuildLayoutImmediate(contentRootRectTransform);
                    LayoutRebuilder.ForceRebuildLayoutImmediate(contentRootRectTransform);
                }

                if (UIManager.Instance.GetWidget<ProfileScreenWidget>().shown)
                {
                    var ftueState = Inventory.Instance.ftueState.Value;
                    if (ftueState.GetFTUE(FTUEType.PROFILE_SCREEN_ITEMS)
                        && ftueState.GetFTUE(FTUEType.PROFILE_SCREEN_ITEM_INFO)
                        && !ftueState.GetFTUE(FTUEType.PROFILE_SCREEN_ADVICES)
                        && ftueState.needShowProfileUnicornAdvicesFtue)
                    {
                        UIManager.Instance.FTUEWidget.WithdrawFTUE();
                        ftueState.SetFTUE(FTUEType.PROFILE_SCREEN_ADVICES, true);
                        Inventory.Instance.ftueState.Save();
                    }
                }
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

