using System.Collections.Generic;
using UnityEngine;

namespace RomenoCompany
{
	public class TweenShaderColor : TweenShader
	{
		[SerializeField] private Color startColor = Color.black;
		[SerializeField] private Color endColor = Color.black;

		protected override void UpdateTweenWithFactor(float factor)
		{
			for (int i = 0; i < renderers.Count; i++)
			{
				renderers[i].GetPropertyBlock(PropertyBlock);

				PropertyBlock.SetColor(propertyName, Color.Lerp(startColor, endColor, factor));

				renderers[i].SetPropertyBlock(PropertyBlock);
			}
		
		}
	}
}

