using UnityEngine;
using UnityEngine.Events;

namespace TSG.Tweens
{
	public class TweenUiPosition : TweenBase 
	{
		[SerializeField] Vector3 startPosition;
		[SerializeField] Vector3 endPosition;
		[SerializeField] RectTransform target;

		public Vector3 StartPosition
		{
			get { return startPosition; }
			set { startPosition = value; }
		}

		public Vector3 EndPosition
		{
			get { return endPosition; }
			set { endPosition = value; }
		}

		protected override void Awake()
		{
			base.Awake();

			if (target == null)
			{
				target = transform as RectTransform;
			}
		}

		protected override void UpdateTweenWithFactor(float factor)
		{
			if (target != null)
			{
				target.anchoredPosition = startPosition + (endPosition - startPosition) * factor;
			}

		}

		public static TweenUiPosition MoveTo(RectTransform obj, Vector3 targetPos, float duration)
		{
			TweenUiPosition tween = obj.GetComponent<TweenUiPosition>();
			if (tween == null)
			{
				tween = obj.gameObject.AddComponent<TweenUiPosition>();
			}

			tween.OnBeginStateSet = new UnityEvent();
			tween.OnEndStateSet = new UnityEvent();

			tween.EndPosition = targetPos;
			tween.StartPosition = obj.anchoredPosition;
			tween.duration = duration;
			tween.target = obj;
			tween.SetEndState();

			return tween;
		}
	}
}

