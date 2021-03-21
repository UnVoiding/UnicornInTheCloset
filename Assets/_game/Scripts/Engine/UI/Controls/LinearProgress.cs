using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;

namespace RomenoCompany
{
    public class LinearProgress : MonoBehaviour
    {
        public enum State
        {
            Filling = 10,
            Filled = 20,
            
            None = 1000,
        }

        [SerializeField] public bool _showPercentage = false;
        [SerializeField] public float _fillDelay = 0;
        [SerializeField] public Ease _fillEase = Ease.OutSine;
        [SerializeField] public float _fillDuration = 0.5f;
        [SerializeField] public float _stateCrossFadeTime = 0.15f;
        
        [Header("Filling state")]
        [SerializeField] GameObject _fillingStateRoot = null;
        [SerializeField] Image _fillingProgress = null;
        [SerializeField] TMP_Text _fillingPercentageText = null;
        [SerializeField] ParticleSystem _fillingFillFx = null;

        [Header("Filled state")]
        [SerializeField] GameObject _filledStateRoot = null;
        [SerializeField] Image _filledProgress = null;
        [SerializeField] TMP_Text _filledPercentageText = null;
        [SerializeField] ParticleSystem _filledCompletionFx = null;
        
        [Header("Runtime Data")]
        [NonSerialized, ShowInInspector, ReadOnly] public State _state = State.Filling;

        private void Awake()
        {
            _fillingPercentageText.gameObject.SetActive(_showPercentage);
            _filledPercentageText.gameObject.SetActive(_showPercentage);
        }

        public float GetProgress()
        {
            return _fillingProgress.fillAmount;
        }

        public void SetProgress(float progress)
        {
            _fillingProgress.fillAmount = progress;
            _filledProgress.fillAmount = progress;

            if (progress >= 1)
            {
                SetState(State.Filled);
            }
            else
            {
                SetState(State.Filling);
            }

            SetPercentageText(progress);
        }

        void SetPercentageText(float progress)
        {
            _fillingPercentageText.text = $"{Mathf.FloorToInt(progress * 100)}%";
            _filledPercentageText.text = $"{Mathf.FloorToInt(progress * 100)}%";
        }
        
        public void SetState(State state)
        {
            _state = state;
        
            switch (state)
            {
                case State.Filling:
                    _fillingProgress.GetComponent<CanvasGroup>().alpha = 1f;
                    _filledProgress.GetComponent<CanvasGroup>().alpha = 0f;
                    break;
                case State.Filled:
                    _fillingProgress.GetComponent<CanvasGroup>().alpha = 0f;
                    _filledProgress.GetComponent<CanvasGroup>().alpha = 1f;
                    break;
            }
        }
        
        public void FillProgress(float from, float to, Action onComplete = null)
        {   
            SetProgress(from);

            if (from >= 1)
            {
                onComplete?.Invoke();
                return;
            }

            _fillingProgress.DOFillAmount(to, _fillDuration)
                .SetDelay(_fillDelay)
                .SetEase(_fillEase)
                .OnStart(() =>
                {
                    _fillingFillFx.gameObject.SetActive(true);
                    _fillingFillFx.Play(true);
                })
                .OnUpdate(() =>
                {
                    var size = _fillingProgress.rectTransform.rect;
                    _fillingFillFx.GetComponent<RectTransform>().anchoredPosition = new Vector2(7 + size.width * _fillingProgress.fillAmount, 0);
                    SetPercentageText(_fillingProgress.fillAmount);
                })
                .OnComplete(() =>
                {
                    _fillingFillFx.Stop(true);
                    if (Mathf.FloorToInt(to) == 1)
                    {
                        _fillingProgress.GetComponent<CanvasGroup>()
                            .DOFade(0, _stateCrossFadeTime);
                        _filledProgress.GetComponent<CanvasGroup>()
                            .DOFade(1, _stateCrossFadeTime)
                            .OnComplete(() =>
                            {
                                _filledCompletionFx.gameObject.SetActive(true);
                                _filledCompletionFx.Play(true);
                                onComplete?.Invoke();
                            });
                    } 
                    else
                    {
                        onComplete?.Invoke();
                    }
                });
        }

        [Button]
        public void PlayCompletionFx()
        {
            _filledCompletionFx.gameObject.SetActive(true);
            _filledCompletionFx.Play(true);
        }
        

    }
}