using System;
using DG.Tweening;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace RomenoCompany
{
    public class Eye : StrictSingleton<Eye>
    {
        [Header("Track Settings")]
        [SerializeField]
        public Vector3 _offset = new Vector3(0, 0, 5);
        [SerializeField]
        public float lobbyZOffset = -10;
        public float lobbyYOffset = 3;
        public float lobbyExitDuration = 0.3f;
        public Vector3 lobbyCameraRotation;
        [System.NonSerialized]
        public Vector3 additionalOffset = Vector3.zero;
        [SerializeField]
        private Vector3 _angle = new Vector3(1, 0, 0);

        public Shake shake0 = new Shake();
        public Shake shake1 = new Shake();
        public Shake shake2 = new Shake();

        private Camera _camera = null;
        public Camera Camera => _camera ? _camera : _camera = GetComponent<Camera>();

        [FormerlySerializedAs("_slowmotionOffset")] [SerializeField]
        private float _tossFocusCooldown = 0;

        [SerializeField, BoxGroup]
        private FocusTweener _focusTweener = new FocusTweener();
        public FocusTweener FocusSettings => _focusTweener;

        [SerializeField, BoxGroup]
        private FocusTweener _gunTweener = new FocusTweener();

        [SerializeField, BoxGroup]
        private FocusTweener _tossTweener = new FocusTweener();

        [SerializeField, BoxGroup]
        private FocusTweener _arcadeTweener = new FocusTweener();

        [SerializeField, BoxGroup]
        private FocusTweener _bossTweener = new FocusTweener();

        private static Vector3 eyePosition { get; set; } = Vector3.zero;
        private static Quaternion eyeRotation = Quaternion.identity;

        [ReadOnly] [SerializeField] FocusTweener currentFocusTweener = null;

        protected override void Setup()
        {
            currentFocusTweener = null;
            TimeManager.Instance.SetTimeScale(this, 1); // first gameobject in scene
        }

        void OnEnable()
        {
            SceneManager.sceneUnloaded += SceneUnloaded;
        }

        void OnDisable()
        {
            SceneManager.sceneUnloaded -= SceneUnloaded;
        }

        private void OnLevelWasLoaded(int level)
        {
            transform.position = eyePosition;
            transform.rotation = eyeRotation;
        }
        
        void SceneUnloaded(Scene scene)
        {
            if (scene.name.Equals("Loading"))
            {
                gameObject.SetActive(true);
            }
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        [ShowInInspector] private Transform _target;
        public void Track(Transform transform)
        {
            _target = transform;
        }

        private Transform _proxy;
        public Transform Proxy => _proxy;
        public Transform BeginProxy()
        {
            GameObject gammeObject = new GameObject("[Eye Proxy]");
            Transform proxy = gammeObject.transform;
            proxy.position = transform.position;
            proxy.rotation = transform.rotation;
            _proxy = proxy;
            return proxy;
        }

        public void EndProxy(float duration = 0)
        {
            if (Mathf.Approximately(duration, 0))
            {
                Destroy(_proxy.gameObject);
            }
            else
            {
                _proxy.DORotateQuaternion(GetTargetRotation(), duration);
                _proxy.DOMove(GetTargetPosition(_target.position), duration).OnComplete(() => { Destroy(_proxy.gameObject); });
            }
        } 

        public void EndProxy(Vector3 prePoint, float duration = 0)
        {
            if (Mathf.Approximately(duration, 0))
            {
                Destroy(_proxy.gameObject);
            }
            else
            {
                _proxy.DORotateQuaternion(GetTargetRotation(), duration);
                _proxy.DOMove(GetTargetPosition(prePoint), duration).OnComplete(() => { Destroy(_proxy.gameObject); });
            }
        }

        public List<Shake> shakes = new List<Shake>();

        private Vector3 GetShake()
        {
            Vector3 sum = Vector3.zero;
            for (int i = 0; i < shakes.Count; i++)
            {
                if (shakes[i] != null)
                {
                    var p = shakes[i].Position;
                    if (sum.magnitude < p.magnitude) sum = p;
                }
            }
            return sum;
        }

        private void LateUpdate()
        {
            // Debug.LogError($"------------ Eye LateUpdate {Time.frameCount}");

            if (_proxy != null)
            {
                Vector3 targetPosition = _proxy.position + GetShake();
                transform.position = Vector3.Lerp(targetPosition, currentFocusTweener?.GetPosition() ?? transform.position, currentFocusTweener?.GetWaight() ?? 0);
                transform.rotation = Quaternion.Lerp(_proxy.rotation, currentFocusTweener?.GetRotation() ?? transform.rotation, currentFocusTweener?.GetWaight() ?? 0);
                FixEyePosition();
                return;
            }
            if (_target != null)
            {
                Vector3 targetPosition = GetTargetPosition(_target.position);
                transform.position = Vector3.Lerp(targetPosition, currentFocusTweener?.GetPosition() ?? transform.position, currentFocusTweener?.GetWaight() ?? 0);
                transform.rotation = Quaternion.Lerp(GetTargetRotation(), currentFocusTweener?.GetRotation() ?? transform.rotation, currentFocusTweener?.GetWaight() ?? 0);
                FixEyePosition();
            }
        }

        private void FixEyePosition()
        {
            eyePosition = transform.position;
            eyeRotation = transform.rotation;
        }

        public void ApplyNextLevelPosition(float pos)
        {
            eyePosition = GetTargetPosition(new Vector3(pos, 0, 1));
            eyeRotation = GetTargetRotation();
            transform.position = eyePosition;
            transform.rotation = eyeRotation;
            _target = null;
        }


        public void UpdateImmediately()
        {
            Vector3 targetPosition = GetTargetPosition(_target.position);
            transform.position = targetPosition;
            transform.rotation = GetTargetRotation();
        }

        public Vector3 GetTargetPosition(Vector3 point)
        {
            return point + GetShake() + _offset + additionalOffset;
        }

        public Quaternion GetTargetRotation()
        {
            return Quaternion.Euler(_angle);
        }

        [System.Serializable]
        public class FocusTweener
        {
            public float endTime = 0;

            [ShowInInspector]
            public bool DebugMode { get; set; }
            private Action _cb = null;
            private Eye _eye = null;
            private Transform _transform = null;

            [SerializeField] private float _duration = 0;
            public float Duration => _duration;
            [SerializeField] private AnimationCurve _curveWeight = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.5f, 1), new Keyframe(1, 0));
            public AnimationCurve CurveWeight => _curveWeight;

            [SerializeField] private Vector3 _offset = Vector3.zero;
            public Vector3 Offset => _offset;

            [SerializeField] private AxisConstraint _axisRotation = AxisConstraint.None;
            public AxisConstraint AxisRotation => _axisRotation;
            private bool HasRotation => _axisRotation != AxisConstraint.None;
            [SerializeField, ShowIf("HasRotation")] private AnimationCurve _curveRotation = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
            public AnimationCurve CurveRotation => _curveRotation;

            [SerializeField] private AxisConstraint _axisPosition = AxisConstraint.None;
            public AxisConstraint AxisPosition => _axisPosition;
            private bool HasPosition => _axisPosition != AxisConstraint.None;
            [SerializeField, ShowIf("HasPosition")] private AnimationCurve _curvePosition = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
            public AnimationCurve CurvePosition => _curvePosition;

            [ReadOnly] public bool _useSlowmotionPhysics = true;
            [SerializeField] private bool _useSlowmotion = true;
            public bool UseSlowmotion => _useSlowmotion;
            [SerializeField, ShowIf("_useSlowmotion")] private AnimationCurve _curveSlowMotion = new AnimationCurve(new Keyframe(0, 1), new Keyframe(0.5f, 0.5f), new Keyframe(1, 1));
            public AnimationCurve CurveSlowMotion => _curveSlowMotion;

            private float _time = 0;
            private Tweener _tweenerPosition = null;
            private Tweener _tweenerWeight = null;
            public bool IsPlaying => _tweenerPosition?.IsActive() ?? false;
            private float _weight = 0;
            private Vector3 _position = Vector3.zero;
            private Quaternion _rotation = Quaternion.identity;

            public FocusTweener() { }

            public FocusTweener(Eye eye, Transform transform)
            {
                _eye = eye;
                _transform = transform;
            }

            public FocusTweener(FocusTweener focusTweener)
            {
                SetParams(focusTweener);
            }

            public void SetParams(FocusTweener focusTweener, Action cb = null)
            {
                this._cb = cb;
                this.endTime = focusTweener.endTime;
                this._duration = focusTweener.Duration;
                this._curveWeight = focusTweener.CurveWeight;
                this._offset = focusTweener.Offset;
                this._axisRotation = focusTweener.AxisRotation;
                this._curveRotation = focusTweener.CurveRotation;
                this._axisPosition = focusTweener.AxisPosition;
                this._curvePosition = focusTweener.CurvePosition;
                this._useSlowmotion = focusTweener.UseSlowmotion;
                this._curveSlowMotion = focusTweener.CurveSlowMotion;
                this.DebugMode = focusTweener.DebugMode;
            }

            public FocusTweener SetEye(Eye eye) { _eye = eye; return this; }
            public FocusTweener SetTrack(Transform transform) { _transform = transform; return this; }
            public FocusTweener SetTrackRotation(AxisConstraint axis) { _axisRotation = axis; return this; }
            public FocusTweener SetTrackPosition(AxisConstraint axis) { _axisPosition = axis; return this; }
            public FocusTweener SetOffset(Vector3 offset) { _offset = offset; return this; }
            public FocusTweener SetDuration(float duration) { _duration = duration; return this; }

            public float GetNormalizeTime() { return _time; }
            public void SetNormalizeTime(float time) { _time = time; }

            public void Kill()
            {
                _cb?.Invoke();
                _tweenerWeight.Kill();
                _tweenerPosition.Kill();
                _position = _eye.transform.position;
                _rotation = _eye.transform.rotation;
                Physics.autoSimulation = true;
                TimeManager.Instance.RemoveOwner(this);
            }

            public float GetWaight() { return _curveWeight.Evaluate(_time); }
            public void SetWaight(float weight) { _weight = weight; }

            public Vector3 GetPosition()
            {
                return _position;
            }
            public Quaternion GetRotation()
            {
                return _rotation;
            }

            private void Update()
            {
                // Debug.LogError($"------------ FocusTweener Update {Time.frameCount}");
                
                Vector3 targetPosition = _transform.position + _offset;
                if (!_axisPosition.HasFlag(AxisConstraint.X)) targetPosition.x = _eye.transform.position.x;
                if (!_axisPosition.HasFlag(AxisConstraint.Y)) targetPosition.y = _eye.transform.position.y;
                if (!_axisPosition.HasFlag(AxisConstraint.Z)) targetPosition.z = _eye.transform.position.z;

                Quaternion targetRoatation = Quaternion.LookRotation(Vector3.Normalize(_transform.position - _eye.transform.position));
                Vector3 eulerAngles = targetRoatation.eulerAngles;
                if (!_axisRotation.HasFlag(AxisConstraint.X)) eulerAngles.x = _eye.transform.eulerAngles.x;
                if (!_axisRotation.HasFlag(AxisConstraint.Y)) eulerAngles.y = _eye.transform.eulerAngles.y;
                if (!_axisRotation.HasFlag(AxisConstraint.Z)) eulerAngles.z = _eye.transform.eulerAngles.z;
                targetRoatation.eulerAngles = eulerAngles;

                _position = Vector3.Lerp(_position, targetPosition, Mathf.Clamp01(_curvePosition.Evaluate(_time)));
                _rotation = Quaternion.Lerp(_rotation, targetRoatation, Mathf.Clamp01(_curveRotation.Evaluate(_time)));

                if(_useSlowmotionPhysics)
                    Physics.Simulate(0.005f);
                else
                    Physics.autoSimulation = true;

                if (_useSlowmotion)
                {
                    TimeManager.Instance.SetTimeScale(this, _curveSlowMotion.Evaluate(_time));
                }
            }

            public FocusTweener Play(float timeScale = 1)
            {
                if (!_tweenerPosition?.IsActive() ?? true)
                {
                    _position = _eye.transform.position;
                    _rotation = _eye.transform.rotation;
                }

                _time = 0;

                if (_tweenerPosition != null)
                {
                    _tweenerPosition.Kill();
                }

                _tweenerPosition = DOTween.To(GetNormalizeTime, SetNormalizeTime, 1, _duration * timeScale);

                _tweenerPosition.OnPlay(() =>
                {
                    _useSlowmotionPhysics = true;
                    Physics.autoSimulation = false;
                });

                _tweenerPosition.OnUpdate(Update).SetUpdate(UpdateType.Normal, true);

                _tweenerPosition.OnComplete(OnComplete);
                return this;
            }

            public void OnComplete()
            {
                // Debug.LogError($"----------- Focus OnComplete {Time.frameCount}");
                _time = 0;
                _cb?.Invoke();
                endTime = Time.realtimeSinceStartup;
                Physics.autoSimulation = true;
                TimeManager.Instance.SetTimeScale(this, 1);
            }
        }

        public FocusTweener Focus(Transform transform)
        {
            // Debug.LogError($"----------- Focus {Time.frameCount}");
            
            // if (_focusTweener == null) _focusTweener = new FocusTweener(this, transform);
            if (currentFocusTweener == null)
            {
                currentFocusTweener = new FocusTweener(_focusTweener);
                currentFocusTweener.SetEye(this);
                currentFocusTweener.SetTrack(transform);
            }

            if (currentFocusTweener.IsPlaying)
            {
                currentFocusTweener.Kill();
            }

            currentFocusTweener.SetParams(_focusTweener);
            currentFocusTweener.SetEye(this);
            currentFocusTweener.SetTrack(transform);
            return currentFocusTweener;
        }

        public FocusTweener BossFocus(Transform transform, Action OnComplete)
        {
            // Debug.LogError($"----------- Focus {Time.frameCount}");
            
            if (currentFocusTweener != null && currentFocusTweener.IsPlaying)
                currentFocusTweener.Kill();

            if (currentFocusTweener == null)
                currentFocusTweener = new FocusTweener(_bossTweener);

            currentFocusTweener.SetParams(_bossTweener, OnComplete);
            currentFocusTweener.SetEye(this);
            currentFocusTweener.SetTrack(transform);
            return currentFocusTweener;
        }

        public FocusTweener TossFocus(Transform transform, Action OnComplete)
        {
            // Debug.LogError($"----------- Toss Focus {Time.frameCount}");

            if (currentFocusTweener == null)
            {
                currentFocusTweener = new FocusTweener(_tossTweener);
                currentFocusTweener.SetEye(this);
                currentFocusTweener.SetTrack(transform);
            }

            if (currentFocusTweener.IsPlaying)
            {
                // Debug.LogError($"--------- TossFocus Killed {Time.frameCount}");
                currentFocusTweener.Kill();
                // OnComplete was not called for some reason so called it manually
                currentFocusTweener.OnComplete();
            }

            // Debug.LogError($"--------- {currentFocusTweener.endTime + _tossFocusCooldown} vs {Time.realtimeSinceStartup}");
            if (currentFocusTweener.endTime + _tossFocusCooldown > Time.realtimeSinceStartup)
            {
                OnComplete?.Invoke();

                // Debug.LogError($"----------- Toss Focus return null {Time.frameCount}");
                return null;
            }

            currentFocusTweener.SetParams(_tossTweener, OnComplete);
            currentFocusTweener.SetEye(this);
            currentFocusTweener.SetTrack(transform);
            return currentFocusTweener;
        }

        public FocusTweener ArcadeFocus(Transform transform)
        {
            if (currentFocusTweener == null)
            {
                currentFocusTweener = new FocusTweener(_arcadeTweener);
                currentFocusTweener.SetEye(this);
                currentFocusTweener.SetTrack(transform);
            }

            if (currentFocusTweener.IsPlaying) 
            {
                currentFocusTweener.Kill();
            }

            currentFocusTweener.SetParams(_arcadeTweener);
            currentFocusTweener.SetEye(this);
            currentFocusTweener.SetTrack(transform);
            return currentFocusTweener;
        }
    }
    
}
