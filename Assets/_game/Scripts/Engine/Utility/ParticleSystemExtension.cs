using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace RomenoCompany
{
    public static class ParticleSystemExtension
    {
        public static Tween FadeOut(this ParticleSystem fx, float time)
        {
            float t = 1.0f;
            return DOTween.To(() => t, x => t = x, 0.0f, time).OnUpdate(() =>
            {
                var mainModule = fx.main;
                mainModule.startColor = new Color(fx.main.startColor.color.r, fx.main.startColor.color.g, fx.main.startColor.color.b, t);
            });
        }
        
        public static Tween FadeIn(this ParticleSystem fx, float time)
        {
            float t = 0;
            return DOTween.To(() => t, x => t = x, 1.0f, time).OnUpdate(() =>
            {
                var mainModule = fx.main;
                mainModule.startColor = new Color(fx.main.startColor.color.r, fx.main.startColor.color.g, fx.main.startColor.color.b, t);
            });
        }

        public static Tween FadeInV2(this ParticleSystem fx, float time)
        {
            float t = 0;
            return DOTween.To(() => t, x => t = x, 1.0f, time).OnUpdate(() =>
            {
                var renderer = fx.GetComponent<Renderer>();
                var color = renderer.material.GetColor(ShaderProperty.color);
                renderer.material.SetColor(ShaderProperty.color, new Color(color.r, color.g, color.b, t));
            });
        }

        public static Tween FadeOutV2(this ParticleSystem fx, float time)
        {
            float t = 1.0f;
            return DOTween.To(() => t, x => t = x, 0.0f, time).OnUpdate(() =>
            {
                var renderer = fx.GetComponent<Renderer>();
                var color = renderer.material.GetColor(ShaderProperty.color);
                renderer.material.SetColor(ShaderProperty.color, new Color(color.r, color.g, color.b, t));
            });
        }
    }
}

