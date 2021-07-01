using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RomenoCompany
{
    public class RCButton : Button
    {
        public TMP_Text text;
        public float clickedTextOffset = 0;
        
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

            if (currentSelectionState == SelectionState.Pressed && 
                eventData.button == PointerEventData.InputButton.Left)
            {
                var m = text.margin;
                m.y = m.y + clickedTextOffset;
                m.w = m.w - clickedTextOffset;
                text.margin = m;
            }
        }
        
        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            
            if (currentSelectionState != SelectionState.Pressed && 
                eventData.button == PointerEventData.InputButton.Left)
            {
                var m = text.margin;
                m.y = m.y - clickedTextOffset;
                m.w = m.w + clickedTextOffset;
                text.margin = m;
            }
        }
    }
}

