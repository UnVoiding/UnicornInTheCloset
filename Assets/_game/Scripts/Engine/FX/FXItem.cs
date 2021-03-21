using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace RomenoCompany
{
    public class FXItem : MonoBehaviour, IPoolObject
    {
        public GameObject Prefab { get; set; }

        [SerializeField]
        private ParticleSystem _particleSystem = null;

        public bool pushWithFade = false;
        [ShowIf("pushWithFade")] public float fadeTime = 0.5f;
        [SerializeField] float simulationSpeed = 1.0f;
        public ParticleSystem Particle => _particleSystem;
        public TrailRenderer[] trails;
        public GameObject[] disableOnStop;
        [ReadOnly] public ParticleSystem[] ps;
        [ReadOnly] public bool emissionEnabled = true;
        float[] startAlphas = null;

        public bool IsLooped
        {
            get
            {
                bool res = false;
                for (int i = 0; i < ps.Length; i++)
                {
                    res |= ps[i].main.loop;
                }
                return res;
            }
        }

        public float Duration
        {
            get
            {
                float res = 0.0f;
                for (int i = 0; i < ps.Length; i++)
                {
                    if (res < ps[i].main.duration)
                    {
                        res = ps[i].main.duration;
                    }
                }
                return res;
            }
        }

        void Awake()
        {
            ps = GetComponentsInChildren<ParticleSystem>();
            trails = GetComponentsInChildren<TrailRenderer>();
            if (pushWithFade)
            {
                startAlphas = new float[ps.Length];
                for (int i = 0; i < ps.Length; i++)
                {
                    startAlphas[i] = ps[i].main.startColor.color.a;
                    var a = ps[i].colorOverLifetime;
                }
            }
            
            for (int i = 0; i < ps.Length; i++)
            {
                ParticleSystem.MainModule main = ps[i].main;
                main.simulationSpeed = simulationSpeed;
            }
        }

        public void OnParticleSystemStopped()
        {
            //Debug.Log($"Callback Stop {name} : {Time.time}");
            if (pushWithFade)
            {
                float t = 0.0f;
                DOTween.To(() => t, x => t = x, fadeTime, fadeTime).
                OnUpdate(() =>
                {
                    for (int i = 0; i < ps.Length; i++)
                    {
                        ParticleSystem.MainModule main = ps[i].main;
                        ParticleSystem.MinMaxGradient gradient = main.startColor;
                        Color current = gradient.color;
                        gradient.color = new Color(current.r, current.g, current.b, Mathf.Lerp(startAlphas[i], 0.0f, t / fadeTime));
                        main.startColor = gradient;
                    }
                }).
                OnComplete(() =>
                {
                    for (int i = 0; i < ps.Length; i++)
                    {
                        ParticleSystem.MainModule main = ps[i].main;
                        ParticleSystem.MinMaxGradient gradient = main.startColor;
                        Color current = gradient.color;
                        gradient.color = new Color(current.r, current.g, current.b, 0.0f);
                        main.startColor = gradient;
                    }
                });
            }
        }

        public void Play(float delay)
        {
            StartCoroutine(Delay(delay, Play));
        }

        private IEnumerator Delay(float delay, Action onComplete)
        {
            yield return new WaitForSeconds(delay);
            onComplete?.Invoke();
        }

        public void Play()
        {
            //Debug.Log($"Play {name} : {Time.time}");
            foreach (var hit in disableOnStop)
                hit.SetActive(true);
            if (!emissionEnabled)
            {
                EnableEmission(true);
            }
            if (pushWithFade)
            {
                for (int i = 0; i < ps.Length; i++)
                {
                    ParticleSystem.MainModule main = ps[i].main;
                    ParticleSystem.MinMaxGradient gradient = main.startColor;
                    Color current = gradient.color;
                    gradient.color = new Color(current.r, current.g, current.b, startAlphas[i]);
                    main.startColor = gradient;
                }
            }
            _particleSystem.Play();
        }

        public void ReturnToPool()
        {
            _particleSystem.Stop();
        }

        public void Push(float delay)
        {
            StartCoroutine(Delay(delay, Push));
        }

        public void Push()
        {
            _particleSystem.Stop();
        }

        private Action<FXItem> _onUpdate = null;
        public FXItem SetUpdate(Action<FXItem> onUpdate)
        {
            _onUpdate = onUpdate;
            return this;
        }

        private void Update()
        {
            if (_onUpdate == null) return;
            _onUpdate.Invoke(this);
        }

        public void ChangeLayer(string layerName)
        {
            if (ps == null) ps = gameObject.GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < ps.Length; i++)
            {
                ps[i].gameObject.layer = LayerMask.NameToLayer(layerName);
            }
        }

        public void EnableEmission(bool enable)
        {
            if (ps == null) ps = gameObject.GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < ps.Length; i++)
            {
                ParticleSystem.EmissionModule emission = ps[i].emission;
                emission.enabled = enable;
            }
            emissionEnabled = enable;
        }

        public void Stop()
        {
            foreach (var hit in disableOnStop)
                hit.SetActive(false);
            _particleSystem.Stop();
        }
    }
}
