using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RomenoCompany
{
    public class ButtonSoundBehaviour : MonoBehaviour, IPointerClickHandler
    {
        public bool allow = true;
    
        public void OnPointerClick(PointerEventData eventData)
        {
            if (allow)
            {
                PlaySound();
            }
        }

        private void PlaySound()
        {
        
        }
    }
}
