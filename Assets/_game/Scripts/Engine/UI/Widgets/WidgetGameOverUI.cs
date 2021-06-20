using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.Serialization;
using Random = System.Random;

namespace RomenoCompany
{
    public class WidgetGameOverUI : Widget
    {
        [Header("References")]
        [SerializeField] private RawImage gameOverUITexture;

        [Header("Animation settings")]
        // [SerializeField] private float fxTokenSpawnDurationOffset = 0.015f;
        // [SerializeField] private float fxCoinSpawnDurationOffset = 0.015f;

        private Vector3 pathStartPosition;
        
        public override void InitializeWidget()
        {
            widgetType = WidgetType.GAME_OVER_UI;

            UIManager.Instance.Wait(0.1f, InitRenderTexture);
        }
        
        private void InitRenderTexture()
        {
            Rect previewSize = gameOverUITexture.GetComponent<RectTransform>().rect;
            RenderTexture rt = new RenderTexture(Mathf.RoundToInt(previewSize.width),
                Mathf.RoundToInt(previewSize.height), 16);
            rt.antiAliasing = 1;

            gameOverUITexture.texture = rt;
            UIManager.Instance.gameOverUICamera.targetTexture = rt;
        }

        public override void Show(System.Action onComplete = null)
        {
            base.Show(onComplete);
            UIManager.Instance.gameOverUICamera.gameObject.SetActive(true);
        }

        public override void ShowInstant()
        {
            base.ShowInstant();
            UIManager.Instance.gameOverUICamera.gameObject.SetActive(true);
        }

        public override void Hide(Action onComplete = null)
        {
            base.Hide(onComplete);
            
            UIManager.Instance.gameOverUICamera.gameObject.SetActive(false);
        }

        public override void HideInstant()
        {
            base.HideInstant();

            UIManager.Instance.gameOverUICamera.gameObject.SetActive(false);
        }

        public override void OnCompositionChanged()
        {
            base.OnCompositionChanged();
            var camera = UIManager.Instance.gameOverUICamera;
            camera.fieldOfView = Eye.Instance.Camera.fieldOfView;
            camera.transform.position = Eye.Instance.Camera.transform.position;
            camera.transform.rotation = Eye.Instance.Camera.transform.rotation;
        }

        // public void PlayGetTokensAnimation(Transform parent, GameObject widget, int count, int overallTokensValue, float dist = 7)
        // {
        //     Vector3 startPos = UIUtil.GetUIWdigetWorldPos(widget, Eye.Instance.Camera, dist);
        //     startPos -= Eye.Instance.Camera.transform.position - Runtime.Instance._gameOverUICamera.transform.position;
        //
        //     Vector3 destinationPos = UIUtil.GetUIWdigetWorldPos(
        //         UIManager.Instance.GetScreen<TokensCounter>().counterT.gameObject, Eye.Instance.Camera, dist);
        //     destinationPos -= Eye.Instance.Camera.transform.position - Runtime.Instance._gameOverUICamera.transform.position; 
        //
        //     for (int i = 0; i < count; i++)
        //     {
        //         int itemValue = overallTokensValue / count;
        //         if (i == count - 1)
        //         {
        //             itemValue += overallTokensValue % count;
        //         }
        //         SpawnToken(parent, startPos, destinationPos, i * fxTokenSpawnDurationOffset, itemValue);
        //     }
        // }
        //
        // private void SpawnToken(Transform parent, Vector3 startWorldPos, Vector3 destinationWorldPos, float spawnTimeOffset, int itemValue)
        // {
        //     var tokenGO = Pool.Instance.Get(Runtime.Instance._tokenPfb, parent);
        //     // var token = tokenGO.GetComponent<TokenOverUIObject>();
        //     // token.Init(startWorldPos, destinationWorldPos, spawnTimeOffset, itemValue);
        // }
        //
        // public void PlayGetCoinsAnimation(Transform parent, GameObject widget, int count, int overallCoinsValue, float dist = 7)
        // {
        //     Vector3 startPos = UIUtil.GetUIWdigetWorldPos(widget, Eye.Instance.Camera, dist);
        //     startPos -= Eye.Instance.Camera.transform.position - Runtime.Instance._gameOverUICamera.transform.position;
        //
        //     Vector3 destinationPos = UIUtil.GetUIWdigetWorldPos(
        //         UIManager.Instance.GetScreen<CoinsCounter>().counterT.gameObject, Eye.Instance.Camera, dist);
        //     destinationPos -= Eye.Instance.Camera.transform.position - Runtime.Instance._gameOverUICamera.transform.position; 
        //     
        //     for (int i = 0; i < count; i++)
        //     {
        //         int itemValue = overallCoinsValue / count;
        //         if (i == count - 1)
        //         {
        //             itemValue += overallCoinsValue % count;
        //         }
        //         SpawnCoin(parent, startPos, destinationPos, i * fxCoinSpawnDurationOffset, itemValue);
        //     }
        // }
        
        // private void SpawnCoin(Transform parent, Vector3 startWorldPos, Vector3 destinationWorldPos, float spawnTimeOffset, int itemValue)
        // {
        //     var coinGO = Pool.Instance.Get(Runtime.Instance._coinPfb, parent);
        //     var coin = coinGO.GetComponent<CoinOverUIObject>();
        //     coin.Init(startWorldPos, destinationWorldPos, spawnTimeOffset, itemValue);
        // }
    }
}
