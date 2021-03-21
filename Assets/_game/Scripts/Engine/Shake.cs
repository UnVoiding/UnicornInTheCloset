using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RomenoCompany
{
    [System.Serializable]
    public class Shake
    {
        private static float _lastShakeTime = 0f;

        [Tooltip("will shake even when shake cooldown is active")]
        [SerializeField] public bool _forceShake = false;
        [SerializeField] public float _shakeDuration = 0.2f;
        [SerializeField] private float _shakeTreshold = 0.1f;
        [SerializeField] private float _shakeForce = 1;
        [SerializeField] public AnimationCurve _shakeCurveX = new AnimationCurve();
        [SerializeField] public AnimationCurve _shakeCurveY = new AnimationCurve();

        private Tweener _shakeTweener = null;

        private bool IsPlay => Application.isPlaying;

        [Button("Shake"), EnableIf("IsPlay")]
        public void DoShake(float force = 1)
        {
            float n = 0;

            if (!_forceShake && Time.realtimeSinceStartup - _lastShakeTime < _shakeTreshold)
            {
                return;
            }

            _lastShakeTime = Time.realtimeSinceStartup;

            Eye.Instance.shakes.Add(this);

            if (_shakeTweener != null) _shakeTweener.Kill();
            _shakeTweener = DOTween.To(() => n, (float r) => { n = r; }, 1, _shakeDuration).OnUpdate(() =>
            {
                Position = new Vector3(_shakeCurveX.Evaluate(n) * force, _shakeCurveY.Evaluate(n) * force, 0) * _shakeForce;
            }).OnComplete(() =>
            {
                Eye.Instance.shakes.Remove(this);
            });
        }

        public Vector3 Position { get; private set; }
        public float Force => _shakeForce;
    }    
}
