using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RomenoCompany
{
    [System.Serializable]
    public class ActionTimeline
    {
        [SerializeField]
        private float _duration = 1;

        [SerializeField]
        private List<ActionTimelineEvent> _events = new List<ActionTimelineEvent>();

        private float _previusTime = 0;

        public ActionTimeline() { }
        public ActionTimeline(float duration) { _duration = duration; }

        public float Duration => _duration;

        public float Normalize => _previusTime;

        public void AddNormalizeEvent(float time, UnityAction<ActionTimelineEvent> action)
        {
            var e = new ActionTimelineEvent();
            e.position = time;
            e.name = "Runtime Event";
            e.callback.AddListener(action);
            _events.Add(e);
        }
        public void AddEvent(float time, UnityAction<ActionTimelineEvent> action)
        {
            var e = new ActionTimelineEvent();
            e.position = time / _duration;
            e.name = "Runtime Event";
            e.callback.AddListener(action);
            _events.Add(e);
        }

        public float GetDistanceBetwen(string aAction, string bAction)
        {
            var a = _events.Find(x => x.name == aAction);
            var b = _events.Find(x => x.name == bAction);
            if (a != null && b != null)
            {
                return Mathf.Abs(a.position * _duration - b.position * _duration);
            }
            return 0;
        }

        public float GetDistanceTo(ActionTimelineEvent action, string name)
        {
            var e = _events.Find(x => x.name == name);
            if (e != null)
            {
                return Mathf.Abs(e.position * _duration - action.position * _duration);
            }
            return 0;
        }

        public float GetNextDistance(ActionTimelineEvent action)
        {
            int index = _events.FindIndex(x => x == action);
            if (index >= 0)
            {
                var e0 = _events[index];
                if (index + 1 < _events.Count)
                {
                    var e1 = _events[index + 1];
                    return e1.position * _duration - e0.position * _duration;
                }
                return _duration - e0.position * _duration;
            }
            return -1;
        }

        protected float GetNormalizeTime()
        {
            return _previusTime;
        }
        protected float GetTime()
        {
            return _previusTime * _duration;
        }

        protected void SetTime(float time)
        {
            EvaluateClamp(time);
        }

        private Tweener _tweener = null;
        public Tweener Play(float duration)
        {
            return _tweener = DOTween.To(GetNormalizeTime, EvaluateNormalize, 1, duration);
        }

        public Tweener Play()
        {
            return _tweener = DOTween.To(GetNormalizeTime, EvaluateNormalize, 1, _duration);
        }

        public void Reset()
        {
            if (_tweener != null) _tweener.Kill();
            _previusTime = 0;
        }

        public void EvaluateClamp(float t)
        {
            EvaluateNormalize(Mathf.Clamp01(t / _duration));
        }

        public void Evaluate(float t)
        {
            EvaluateNormalize(Mathf.Repeat(t, _duration) / _duration);
        }
        public void EvaluateClampSpeed(float speed)
        {
            EvaluateClamp(_previusTime * _duration + speed);
        }

        public bool IsEnd => Mathf.Approximately(_previusTime, 1);

        public void EvaluateNormalize(float t)
        {
            t = Mathf.Clamp01(t);

            float delta = t - _previusTime;
            int direction = delta > 0 ? 1 : -1;

            for (int i = 0; i < _events.Count; i++)
            {
                var e = _events[i];
                if (direction > 0)
                    if (t - delta <= e.position && e.position <= t)
                    {
                        e.Execute(this, direction);
                    }

                if (direction < 0)
                    if (t - delta >= e.position && e.position >= t)
                    {
                        e.Execute(this, direction);
                    }
            }

            _previusTime = t;
        }
    }

}
