using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace RomenoCompany
{
    public class Counter : Widget
    {
        [SerializeField] public int order = 0;
        [SerializeField] protected Image icon = null;
        [SerializeField] protected TextMeshProUGUI text = null;

        [SerializeField] public RectTransform counterT = null;

        public virtual void SetValue(int v) { text.text = v.ToString(); }
        public void SetValue(float v) { text.text = v.ToString("E0"); }
        public void SetValue(string v) { text.text = v; }

        public override void InitializeWidget()
        {
            
        }

        protected int Value
        {
            get
            {
                int res = 0;
                Int32.TryParse(text.text, out res);
                return res;
            }
        }
    }
}
