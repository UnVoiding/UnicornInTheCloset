using UnityEngine;
using UnityEngine.UI;
using System;
using Sirenix.OdinInspector;
using TMPro;

namespace RomenoCompany
{
    public class RadialProgress : MonoBehaviour
    {
        public enum State
        {
            Locked = 10,
            Unlocked = 20,
            
            None = 1000,
        }
        
        [SerializeField] private GameObject _lockedState = null;
        [SerializeField] private GameObject _unlockedState = null;
        [SerializeField] private Image _progress = null;
        [SerializeField] private Image _lockedProgress = null;
        
        [Header("Runtime Data")]
        [NonSerialized, ShowInInspector, ReadOnly] public State _state = State.Unlocked;

        public float GetProgress()
        {
            return _progress.fillAmount;
        }

        public void SetProgress(float progress)
        {
            _progress.fillAmount = progress;
            _lockedProgress.fillAmount = progress;
        }
        
        public void SetState(State state)
        {
            _state = state;

            switch (state)
            {
                case State.Locked:
                    _lockedState.gameObject.SetActive(true);
                    _unlockedState.gameObject.SetActive(false);
                    break;
                case State.Unlocked:
                    _lockedState.gameObject.SetActive(false);
                    _unlockedState.gameObject.SetActive(true);
                    break;
            }
        }
    }
}