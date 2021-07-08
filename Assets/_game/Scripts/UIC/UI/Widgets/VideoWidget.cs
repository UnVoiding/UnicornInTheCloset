using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine.Video;
using Button = UnityEngine.UI.Button;

namespace RomenoCompany
{
    public class VideoWidget : Widget
    {
        [                                          Header("Video Widget"), FoldoutGroup("References")] 
        public RawImage videoImage;
        [                                                                  FoldoutGroup("References")] 
        public VideoPlayer videoPlayer;
        [                                                                  FoldoutGroup("References")] 
        public Button closeBtn;

        [                                NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")] 
        public RenderTexture renderTexture;
        [                                NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")] 
        public VideoClip videoClip;
        [                                NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")] 
        public bool requestedToPlay = false;
        [                                NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")] 
        public Action onVideoEnded;

        
        public override void InitializeWidget()
        {
            base.InitializeWidget();
            
            widgetType = WidgetType.VIDEO;
            closeBtn.onClick.AddListener(() =>
            {
                videoPlayer.Stop();
                ClearOutRenderTexture(renderTexture);
                Hide(onVideoEnded);
            });

            // no unsubscription yet
            videoPlayer.loopPointReached += VideoEndReached;
            videoPlayer.prepareCompleted += VideoPrepared;
            videoPlayer.targetCamera = Camera.main;
        }

        public void ShowForVideo(VideoClip videoClip, Action onVideoEnded = null)
        {
            ClearOutRenderTexture(renderTexture);
            
            this.onVideoEnded = onVideoEnded;
            this.videoClip = videoClip;
            requestedToPlay = false;

            Show(WidgetShown);

            if (renderTexture == null)
            {
                renderTexture = new RenderTexture((int)videoImage.rectTransform.rect.width, (int)videoImage.rectTransform.rect.height, 0);
            }
            
            videoImage.texture = renderTexture;
            videoPlayer.targetTexture = renderTexture;
        }
        
        public void ClearOutRenderTexture(RenderTexture renderTexture)
        {
            RenderTexture rt = RenderTexture.active;
            RenderTexture.active = renderTexture;
            GL.Clear(true, true, Color.clear);
            RenderTexture.active = rt;
        }

        public void WidgetShown()
        {
            videoPlayer.clip = videoClip;
            videoPlayer.Prepare();
            requestedToPlay = true;
            
            // if (videoPlayer.isPrepared)
            // {
            //     videoPlayer.Play();
            //
            //     // if (videoImageTweener != null) videoImageTweener.Kill();
            //     // videoImageTweener = videoImage.DOFade(1, 0.3f);    
            // }
            // else
            // {
            //     requestedToPlay = true;
            // }
        }
        
        // private void Update()
        // {
        //     if (Input.GetKey(KeyCode.Space))
        //     {
        //         Debug.LogWarning("~~~~ Space");
        //         videoPlayer.Play();
        //     }
        //     // if (shown && requestedToPlay && videoPlayer.isPrepared)
        //     // {
        //     //     videoPlayer.Play();
        //     //
        //     //     // if (imageTweener != null) imageTweener.Kill();
        //     //     // imageTweener = image.DOFade(0, 0.3f);
        //     //
        //     //     // if (videoImageTweener != null) videoImageTweener.Kill();
        //     //     // videoImageTweener = videoImage.DOFade(1, 0.3f);
        //     //     
        //     //     requestedToPlay = false;
        //     // }
        // }

        public void VideoPrepared(VideoPlayer p)
        {
             Debug.LogWarning("~!!! Video Prepared");
             videoPlayer.Play();
        }

        public void VideoEndReached(VideoPlayer p)
        {
            videoPlayer.Stop();
            ClearOutRenderTexture(renderTexture);
            Hide(onVideoEnded);
        }

        public override void Show(System.Action onComplete = null)
        {
            base.Show(onComplete);
        }

        public override void Hide(Action onComplete = null)
        {
            if (!hidding)
            {
                videoPlayer.Stop();
            }
            
            base.Hide(onComplete);
        }
    }
}