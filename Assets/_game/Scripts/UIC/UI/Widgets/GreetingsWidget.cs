using UnityEngine;
using System;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.UI;

namespace RomenoCompany
{
    public class GreetingsWidget : Widget
    {
        [                                          Header("Greetings Widget"), FoldoutGroup("References")] 
        public TMP_Text captionText;
        [                                                                      FoldoutGroup("References")] 
        public TMP_Text greetingsText;
        [                                                                      FoldoutGroup("References")] 
        public Button okBtn;

        
        public override void InitializeWidget()
        {
            base.InitializeWidget();
            
            widgetType = WidgetType.GREETINGS;
            
            okBtn.onClick.AddListener(() =>
            {
                Inventory.Instance.firstLaunch.Value = false;
                Inventory.Instance.firstLaunch.Save();
                
                UIManager.Instance.GoToComposition(Composition.MAIN);
            });
        }

        public override void ShowInstant()
        {
            base.ShowInstant();

            OnShow();
        }

        public override void Show(System.Action onComplete = null)
        {
            base.Show(onComplete);

            OnShow();
        }

        private void OnShow()
        {
            var dm = LayoutManager.Instance.defaultMargins;
            
            captionText.margin = dm * 2;

            greetingsText.margin = dm;

            var btnRt = (okBtn.transform as RectTransform);
            btnRt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, btnRt.rect.size.x - dm.x - dm.z);
        }

        public override void Hide(Action onComplete = null)
        {
            base.Hide(onComplete);
        }
    }
}