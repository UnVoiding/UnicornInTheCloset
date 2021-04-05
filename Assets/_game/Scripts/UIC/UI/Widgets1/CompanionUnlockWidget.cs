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
        public Transform contentRoot;
        [                                                                  FoldoutGroup("References")] 
        public Button closeBtn;

        [                                         NonSerialized, ShowInInspector, ReadOnly, FoldoutGroup("Runtime")] 
        public List<UnlockedCompanion> unlockedCompanions;

        public override void InitializeWidget()
        {
            base.InitializeWidget();

            widgetType = WidgetType.COMPANION_UNLOCK;
            closeBtn.onClick.AddListener(() =>
            {
                Hide();
            });
        }

        public void ShowForCompanions(List<CompanionData> unlockedCompanionsData)
        {
            foreach (var uc in this.unlockedCompanions)
            {
                Destroy(uc.gameObject);
            }
            this.unlockedCompanions.Clear();
            
            foreach (var ucData in unlockedCompanionsData)
            {
                var ucEntry = Instantiate(unlockedCompanionPfb, contentRoot);
                ucEntry.name.text = ucData.name;
                this.unlockedCompanions.Add(ucEntry);
            }
            
            Show();
        }

        public override void Show(System.Action onComplete = null)
        {
            base.Show(onComplete);
        }

        public override void Hide(Action onComplete = null)
        {
            base.Hide(onComplete);
        }
    }
}