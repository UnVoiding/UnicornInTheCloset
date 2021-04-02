using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace RomenoCompany
{
    public class CompanionBtn : MonoBehaviour
    {
        public enum State
        {
            Locked = 0,
            Unlocked = 1,
        }

        
        [                                              SerializeField, FoldoutGroup("References")]
        private Image lockedState;
        [                                              SerializeField, FoldoutGroup("References")]
        private Image unlockedState;


        public void Init(Sprite sprite)
        {
            lockedState.sprite = sprite;
            unlockedState.sprite = sprite;
            
            SetState(State.Locked);
        }
        
        
        public void SetState(State state)
        {
            switch (state)
            {
                case State.Locked:
                    lockedState.gameObject.SetActive(true);
                    unlockedState.gameObject.SetActive(false);
                    break;
                case State.Unlocked:
                    lockedState.gameObject.SetActive(false);
                    unlockedState.gameObject.SetActive(true);
                    break;
                default:
                    Debug.LogError($"CompanionBtn: unknown state {state}");
                    break;
            }
        }
    }
}


