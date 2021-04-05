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
        [                                              SerializeField, FoldoutGroup("References")]
        private Button mainButton;
        [                                              SerializeField, FoldoutGroup("References")]
        private CompanionState companionState;


        public void Init(CompanionState state)
        {
            companionState = state;
            
            lockedState.sprite = state.data.mainScreenImage;
            unlockedState.sprite = state.data.mainScreenImage;

            mainButton = GetComponent<Button>();
            mainButton.onClick.AddListener(() =>
            {
                UIManager.Instance.GetWidget<CompanionInfoWidget>().ShowForCompanion(companionState);
            });

            if (companionState.locked)
            {
                SetState(State.Locked);
            }
            else
            {
                SetState(State.Unlocked);
            }
        }
        
        
        public void SetState(State state)
        {
            switch (state)
            {
                case State.Locked:
                    lockedState.gameObject.SetActive(true);
                    unlockedState.gameObject.SetActive(false);
                    mainButton.targetGraphic = lockedState;
                    mainButton.interactable = false;
                    break;
                case State.Unlocked:
                    lockedState.gameObject.SetActive(false);
                    unlockedState.gameObject.SetActive(true);
                    mainButton.targetGraphic = unlockedState;  
                    mainButton.interactable = true;
                    break;
                default:
                    Debug.LogError($"CompanionBtn: unknown state {state}");
                    break;
            }
        }
    }
}


