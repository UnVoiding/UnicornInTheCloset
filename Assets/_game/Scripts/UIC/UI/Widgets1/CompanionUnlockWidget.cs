using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using Sirenix.OdinInspector;
using DG.Tweening;
using Button = UnityEngine.UI.Button;

namespace RomenoCompany
{
    public class CompanionUnlockWidget : Widget
    {
        [                               Header("Companion Unlock Widget"), FoldoutGroup("References")] 
        public UnlockedCompanion unlockedCompanionPfb;
        [                                                                  FoldoutGroup("References")] 
        public GridLayoutGroup contentRoot;
        [                                                                  FoldoutGroup("References")] 
        public RectTransform mainPanel;
        [                                                                  FoldoutGroup("References")] 
        public TMP_Text caption;
        [                                                                  FoldoutGroup("References")] 
        public Button closeBtn;

        [                                 Header("Companion Unlock Widget"), FoldoutGroup("Settings")] 
        public string captionText;
        [                                                                  FoldoutGroup("Settings")] 
        public string pluralCaptionText;
        [                                                                  FoldoutGroup("Settings")] 
        public int btnsPerRow = 3;

        [                         NonSerialized, ShowInInspector, ReadOnly, FoldoutGroup("Runtime")] 
        public List<UnlockedCompanion> unlockedCompanions;
        [                         NonSerialized, ShowInInspector, ReadOnly, FoldoutGroup("Runtime")] 
        public Action onClose;
        [                         NonSerialized, ShowInInspector, ReadOnly, FoldoutGroup("Runtime")] 
        public bool firstTimeShown = true;

        
        public override void InitializeWidget()
        {
            base.InitializeWidget();

            widgetType = WidgetType.COMPANION_UNLOCK;
            closeBtn.onClick.AddListener(() =>
            {
                Hide();
            });
            
            float compBtnWidth = (int)((mainPanel.rect.width - btnsPerRow * contentRoot.spacing.x - contentRoot.padding.left) / btnsPerRow);
            contentRoot.cellSize = new Vector2(compBtnWidth, 2 * compBtnWidth);

            unlockedCompanions = new List<UnlockedCompanion>();

            Ocean.Instance.CreatePool(unlockedCompanionPfb.gameObject, 10);
        }

        public void ShowForCompanions(List<CompanionData.ItemID> unlockedCompanionIds)
        {
            if (firstTimeShown)
            {
                firstTimeShown = false;
            }
            
            if (unlockedCompanionIds.Count > 1)
            {
                caption.text = pluralCaptionText;
            }
            else
            {
                caption.text = captionText;
            }
            
            foreach (var uc in this.unlockedCompanions)
            {
                Ocean.Instance.Return(uc);
            }
            this.unlockedCompanions.Clear();

            float esw = LayoutManager.Instance.esw;
            var margin = LayoutManager.Instance.defaultMargins;
            
            foreach (var cid in unlockedCompanionIds)
            {
                var ucEntry = Ocean.Instance.Get(unlockedCompanionPfb);
                // var ucEntry = Instantiate(unlockedCompanionPfb, contentRoot.transform);
                
                var companionState = Inventory.Instance.worldState.Value.GetCompanion(cid);
                ucEntry.name.text = companionState.Data.name;
                ucEntry.name.fontSize = esw;
                ucEntry.name.margin = Vector4.zero;
                
                ucEntry.image.sprite = companionState.Data.mainScreenImage;
                this.unlockedCompanions.Add(ucEntry);
            }
            
            Show();

            RectTransform rt = contentRoot.transform as RectTransform;
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(rt);
            LayoutRebuilder.ForceRebuildLayoutImmediate(rt);
            LayoutRebuilder.ForceRebuildLayoutImmediate(rt);
        }

        public override void Show(System.Action onComplete = null)
        {
            base.Show(onComplete);
        }

        public override void Hide(Action onComplete = null)
        {
            if (!hidding)
            {
                onClose?.Invoke();
            }

            base.Hide(onComplete);
        }
    }
}