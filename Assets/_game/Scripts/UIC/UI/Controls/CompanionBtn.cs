using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.PlayerLoop;
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
        private Image notification;
        
        [                                              SerializeField, FoldoutGroup("Settings")]
        public float notifSize = 0.25f;
        [                                              SerializeField, FoldoutGroup("Settings")]
        public Vector2 notifOffset = Vector2.zero;
        [            Header("Notification Animation"), SerializeField, FoldoutGroup("Settings")]
        public Vector3 notifAnimDirection = new Vector3(1, 1, 0);
        [                                              SerializeField, FoldoutGroup("Settings")]
        public float notifAnimDuration = 1.5f;
        [                                              SerializeField, FoldoutGroup("Settings")]
        public int notifAnimVibrato = 1;
        [                                              SerializeField, FoldoutGroup("Settings")]
        public float notifAnimElasticity = 0.5f;
        [                                              SerializeField, FoldoutGroup("Settings")]
        public Color notifAnimToColor = Color.red;
        [                                              SerializeField, FoldoutGroup("Settings")]
        public float notifAnimPause = 0.5f;

        
        [                                       ShowInInspector, ReadOnly, FoldoutGroup("Runtime")]
        private CompanionState companionState;
        [                                       ShowInInspector, ReadOnly, FoldoutGroup("Runtime")]
        public float size;
        [                                       ShowInInspector, ReadOnly, FoldoutGroup("Runtime")]
        public Sequence notifSeq = null;


        public void Init(CompanionState state)
        {
            companionState = state;
            
            lockedState.sprite = state.Data.mainScreenImage;
            unlockedState.sprite = state.Data.mainScreenImage;

            mainButton.onClick.AddListener(() =>
            {
                if (!UIManager.Instance.inputAllowed) return;
                
                UIManager.Instance.GetWidget<CompanionInfoWidget>().ShowForCompanion(companionState, true);
                
                var ftueState = Inventory.Instance.ftueState.Value;
                if (!ftueState.GetFTUE(FTUEType.COMPANION_SELECTION1)
                    && ftueState.needShowCompanionSelection)
                {
                    UIManager.Instance.GetWidget<MainScreenWidget>().EnableScroll(true);
                    UIManager.Instance.FTUEWidget.WithdrawFTUE();
                    ftueState.SetFTUE(FTUEType.COMPANION_SELECTION1, true);
                    Inventory.Instance.ftueState.Save();
                }
            });

            ResizeAndReposition();

            RestartNotifAnim();
            
            UpdateState();
        }

        [FoldoutGroup("Settings"), Button]
        public void ResizeAndReposition()
        {
            var r = notification.rectTransform.rect;
            notification.rectTransform.anchoredPosition = new Vector3(-size * notifSize * notifOffset.x, -size * notifSize * notifOffset.y, 0);
            notification.rectTransform.sizeDelta = new Vector2(size * notifSize, size * notifSize);
        }

        [FoldoutGroup("Settings"), Button]
        public void RestartNotifAnim()
        {
            if (notifSeq != null)
            {
                Stop();
            }
            notifSeq = DOTween.Sequence();
            
            notifSeq
                .Append(notification.rectTransform
                .DOPunchScale(notifAnimDirection, notifAnimDuration, notifAnimVibrato, notifAnimElasticity))
                .Insert(0, notification.DOColor(notifAnimToColor, notifAnimDuration))
                .AppendInterval(notifAnimPause)
                .SetLoops(-1);
        }

        [FoldoutGroup("Settings"), Button]
        public void Stop()
        {
            notifSeq.Kill(true);

            notification.rectTransform.localScale = Vector3.one;
            notification.color = Color.white;
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

        public void UpdateState()
        {
            SetState(companionState.locked ? State.Locked : State.Unlocked);

            if (!companionState.locked)
            {
                bool needNotif = companionState.activeDialogue != companionState.lastDialogueTaken;
                notification.gameObject.SetActive(needNotif);
                // if (needNotif)
                // {
                //     notifSeq.Restart();
                // }
            }
            else
            {
                notification.gameObject.SetActive(false);
                // notifSeq.Pause();
            }
        }
    }
}


