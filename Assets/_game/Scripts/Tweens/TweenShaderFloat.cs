using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TSG.Tweens
{
	public class TweenShaderFloat : TweenShader
	{
		[SerializeField] private float startParam;
		[SerializeField] private float endParam;

		public float StartParam
		{
			get { return startParam; }
			set { startParam = value; }
		}

		public float EndParam
		{
			get { return endParam; }
			set { endParam = value; }
		}

		protected override void UpdateTweenWithFactor(float factor)
		{
			for (int i = 0; i < renderers.Count; i++)
			{
				renderers[i].GetPropertyBlock(PropertyBlock);

				PropertyBlock.SetFloat(propertyName, Mathf.Lerp(startParam, endParam, factor));

				renderers[i].SetPropertyBlock(PropertyBlock);
			}
		
		}
	}
}

