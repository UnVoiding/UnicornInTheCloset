using System;
using Sirenix.OdinInspector;
using UnityEngine;
using DG.Tweening;

namespace RomenoCompany
{
    // public class CoinsCounter : Counter
    // {
    //     public override ScreenType ScreenType => ScreenType.COINS_COUNTER;
    //
    //     private Canvas _rootCanvas = null;
    //
    //     //call only in game
    //     public Vector3 WorldPosition
    //     {
    //         get
    //         {
    //             if(_rootCanvas == null) _rootCanvas = GetComponentInParent<Canvas>();
    //
    //             var worldCamera = Eye.Instance.Camera;
    //             var uiCamera = _rootCanvas.worldCamera;
    //
    //             Vector3 screenPoisition = RectTransformUtility.WorldToScreenPoint(uiCamera, counterT.position);
    //            
    //             Ray ray = worldCamera.ScreenPointToRay(screenPoisition);
    //             Plane plane = new Plane(Vector3.back, 0);
    //             plane.Raycast(ray, out float d);
    //             Vector3 worldPosition = ray.origin + ray.direction * (d - 2);
    //             return worldPosition;
    //         }
    //     }
    //
    //     [SerializeField] Color textColor = new Color(0.8f, 0.66f, 0);
    //     [SerializeField] bool inventoryLinked = false;
    //     [SerializeField] bool gameLinked = false;
    //     [SerializeField] float animatedAccrualTime = 0.5f;
    //     [SerializeField] float punch = 0.2f;
    //     [SerializeField] float punchDuration = 0.2f;
    //     [SerializeField] ParticleSystem cashFX = null;
    //     [SerializeField] float playSoundDelay = 2.5f;
    //     Runtime cachedRuntime = null;
    //     int subscribeCount = 0;
    //     [ReadOnly] [SerializeField] int prevValue = 0;
    //     Tweener punchTween = null, accuralTween = null;
    //
    //     protected virtual void Start()
    //     {
    //         if (inventoryLinked)
    //         {
    //             Inventory.Instance.Coins.OnChange += OnInventoryValueChange;
    //             SetValue(Inventory.Instance.Coins.Value);
    //         }
    //     }
    //
    //     void OnDisable()
    //     {
    //         if (!gameLinked) return;
    //         if (cachedRuntime == null || cachedRuntime.OnCoinsChanged == null) return;
    //         cachedRuntime.OnCoinsChanged -= OnRuntimeCoinsChanged;
    //     }
    //
    //     private void OnDestroy()
    //     {
    //         Inventory.Instance.Coins.OnChange -= OnInventoryValueChange;
    //         if (punchTween != null)
    //         {
    //             punchTween.Kill();
    //             punchTween = null;
    //         }
    //         if (accuralTween != null)
    //         {
    //             accuralTween.Kill();
    //             accuralTween = null;
    //         }
    //     }
    //
    //     public override void Show(Action onComplete = null)
    //     {
    //         base.Show(onComplete);
    //         UIManager.Instance.OnCounterShown(this, true);
    //     }
    //
    //     public override void ShowInstant()
    //     {
    //         base.ShowInstant();
    //         UIManager.Instance.OnCounterShown(this, true);
    //     }
    //
    //     public override void Hide(Action onComplete = null)
    //     {
    //         base.Hide(onComplete);
    //         UIManager.Instance.OnCounterShown(this, false);
    //     }
    //
    //     public override void HideInstant()
    //     {
    //         base.HideInstant();
    //         UIManager.Instance.OnCounterShown(this, false);
    //     }
    //
    //     void OnInventoryValueChange(int newValue)
    //     {
    //         AddCoinsAnimated(newValue, 0);
    //     }
    //
    //     void OnRuntimeCoinsChanged(int newValue)
    //     {
    //         AddCoinsAnimated(newValue, 0);
    //     }
    //
    //     public override void SetValue(int v)
    //     {
    //         base.SetValue(v);
    //     }
    //
    //     private float lastPlayTime = 0;
    //     public void AddCoinsAnimated(int value, int minValue)
    //     {
    //         prevValue = value;
    //         float t = 0.0f;
    //         accuralTween = DOTween.To(() => t, x => t = x, animatedAccrualTime, animatedAccrualTime).OnUpdate(() =>
    //         {
    //             int tempVal = (int)Mathf.Lerp((float)prevValue, (float)value, t / animatedAccrualTime);
    //             SetValue(tempVal);
    //         }).SetUpdate(UpdateType.Normal, true);
    //         if (punchTween != null)
    //         {
    //             punchTween.Kill(true);
    //         }
    //         punchTween = counterT.DOPunchScale(Vector3.one * punch, punchDuration).OnComplete(() =>
    //         {
    //             punchTween = null;
    //         }).SetUpdate(UpdateType.Normal, true);
    //
    //         if (cashFX != null && cashFX.isPlaying) cashFX.Stop();
    //         if (cashFX != null) cashFX.Play();
    //
    //         if (Time.time - lastPlayTime > playSoundDelay)
    //         {
    //             lastPlayTime = Time.time;
    //         }
    //     }
    //
    //     public Tweener DOValue(int value, float duration, float delay)
    //     {
    //         float t = 0.0f;
    //         Tweener tweener = DOTween.To(() => value, x => value = x, value, duration).SetDelay(delay).SetUpdate(UpdateType.Normal, true).OnUpdate(() =>
    //         {
    //             if (t > 0.2f)
    //             {
    //                 t = 0.0f;
    //                 if (cashFX != null && cashFX.isPlaying) cashFX.Stop();
    //                 if (cashFX != null) cashFX.Play();
    //             }
    //             else
    //             {
    //                 t += Time.unscaledDeltaTime;
    //             }
    //         });
    //         if (duration == 0) tweener.Complete(true);
    //         return tweener;
    //     }
    // }
}
