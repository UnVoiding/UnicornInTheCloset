using UnityEngine;
using UnityEngine.UI;

namespace RomenoCompany
{
	public class TweenSpriteAnimation : TweenBase
	{

		[SerializeField] private Image targetImage = null;
		[SerializeField] private Sprite[] sprites = null;

		protected override void UpdateTweenWithFactor(float factor)
		{
			if (sprites.Length == 0)
			{
				return;
			}

			float factorPerSprite = 1f / sprites.Length;

			int index = Mathf.FloorToInt(factor / factorPerSprite);

			if (index >= sprites.Length)
			{
				index = 0;
			}
		
			targetImage.sprite = sprites[index];
		}
	}
}

