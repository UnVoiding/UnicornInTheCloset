using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine.Serialization;

namespace RomenoCompany
{
    public class Runtime : Singleton<Runtime>
    {
         public bool isPolygon;
         public static bool IsPolygon => Instance.isPolygon;
         
         [SerializeField] public GameObject _coinPfb = null;
         [SerializeField] public GameObject _tokenPfb = null;
         [SerializeField] public Light _mainLight = null;
         [SerializeField] public Camera _gameOverUICamera = null;
         // [SerializeField] public GameObject _keyPfb = null;

         [ReadOnly] public LocationData.ItemID currentLocationID;

         [ReadOnly, System.NonSerialized] public bool isEndLevel = false;
         [ReadOnly, System.NonSerialized] public bool isArcadeStart = false;
         [ReadOnly] public bool inputEnabled = false;
         [ReadOnly] public LocationData locationData = null;
         [ReadOnly] public bool wasRevive = false;
         [ReadOnly] public bool hasDied = false;
         [ReadOnly] public bool hasLevelStarted = false;

         public float SuperTimeScale = 1;
         [ReadOnly] public float TimeScale = 1;
         [ShowInInspector] public float DeltaTime => Time.deltaTime * TimeScale * SuperTimeScale;
         public float UnscaledDeltaTime => Time.unscaledDeltaTime;

         public bool IsMuteRuntimeSound { get; set; }

         // [ReadOnly] public List<int> enemiesOnWaves = new List<int>();
         [ReadOnly] public List<int> enemiesToLevelUp = new List<int>();

         [ShowInInspector, ReadOnly] public int CurrentLevel = 0;
         [ShowInInspector, ReadOnly] public int CurrentWave = 0;
         [ShowInInspector, ReadOnly] public int CurrentEnemies = 0;

         [ShowInInspector] [ReadOnly] public int CurrentExpLevel { get; set; }
         int earnedCoins = 0;
         [ShowInInspector]
         [ReadOnly]
         public int EarnedCoins
         {
             get => earnedCoins;
             set
             {
                 if (value != earnedCoins)
                 {
                     OnCoinsChanged?.Invoke(earnedCoins);
                 }
             }
         }
         public System.Action<int> OnCoinsChanged;
         public System.Action KilledEnemiesAtWaveChanged;
         public System.Action KilledEnemiesAtLevelChanged;

         [ShowInInspector]
         [ReadOnly]
         public int KilledEnemiesAtWave
         {
             get => _killedEnemiesAtWave;
             set
             {
                 if (_killedEnemiesAtWave == value) return;

                 _killedEnemiesAtWave = value;
                 KilledEnemiesAtWaveChanged?.Invoke();
             }
         }

         private int _killedEnemiesAtWave = 0;

         [ShowInInspector]
         [ReadOnly]
         public int KilledEnemiesAtLevel
         {
             get
             {
                 return _killedEnemiesAtLevel;
             }
             set
             {
                 if (_killedEnemiesAtLevel == value) return;

                 _killedEnemiesAtLevel = value;
                 KilledEnemiesAtLevelChanged?.Invoke();
             }
         }

         private int _killedEnemiesAtLevel = 0;

         [ReadOnly] public bool completedLocation = false;

         private static Runtime _instance;
         public static Runtime Instance
         {
             get
             {
                 if (_instance == null)
                 {
                     _instance = FindObjectOfType<Runtime>();

                     if (_instance == null)
                     {
                         return null;
                         // throw new UnityException("thre is no runtime, plak plak");
                     } else {
                         _instance.Start();
                     }
                 }
                 return _instance;
             }
         }

         public static void ResetInstance()
         {
             _instance = null;
         }

         protected override void Setup()
         {
             
         }

         protected void Start()
         {
             // if (_inited) return;
             // _inited = true;
             //
             // Application.targetFrameRate = 60;
             // Input.multiTouchEnabled = false;
             // hasLevelStarted = false;
             //
             // var completion = Inventory.Instance.completion;
             //
             // LevelData = DB.Instance.progression.GetLevel(completion.Value.level);
             // if (LevelData.type == LevelType.Bonus)
             // {
             //     locationData = DB.Instance.locations.GetItem(LevelData.bonusContainer.location);
             // } 
             // else
             // {
             //     locationData = DB.Instance.locations.Items[LevelData.location];
             // }
             // currentLocationID = locationData.itemId;
             // SetRenderSettings();
             // RestoreColors(-1.0f, new AnimationCurve());
             //
             // #if UNITY_IOS
             // iOSHapticFeedback.Instance.debug = false;
             // #endif
             //
             // if (Inventory.Instance.completion.Value.level != 0) return;

         }
         
         public void SetRenderSettings()
         {
             // var completion = Inventory.Instance.completion;
             // var level = DB.Instance.progression.GetLevel(completion.Value.level);
             // LocationData data;
             //
             // if (level.type == LevelType.Bonus)
             // {
             //     data = DB.Instance.locations.GetItem(LevelData.bonusContainer.location);
             // } else
             // {
             //     data = DB.Instance.locations.Items[LevelData.location];
             // }
             //
             // RenderSettings.fog = true;
             // RenderSettings.fogMode = FogMode.Linear;
             // RenderSettings.fogColor = data.fog;
             // RenderSettings.fogStartDistance = data.forStart;
             // RenderSettings.fogEndDistance = data.fogEnd;
         }

         private void Update()
         {
             TimeScale = TimeManager.Instance.TimeScale;
             Time.timeScale = TimeScale;
         }

         public void Reload()
         {
         }

         public void Pause()
         {
         }

         public void Resume()
         {
         }

         public void Play()
         {
             // hasLevelStarted = true;
             //
             // if (Inventory.Instance.completion.Value.level == 0) return;
         }


         public void GoToMenu()
         {
             // TimeManager.Instance.Reset();
             // SceneLoader.Instance.ReloadActiveScene();
             // Inventory.Instance.loadedFromLoadingScene = false;
         }

         public void FadeColors(Vector3 fadeValue, float fadeTime, AnimationCurve ease)
         {

         }

         public void RestoreColors(float restoreTime, AnimationCurve ease)
         {
             
         }
    }
    
}
