using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RomenoCompany
{
    public class GameManager : StrictSingleton<GameManager>
    {
        // [FoldoutGroup("Prefabs")]
        // [SerializeField] private FXPool fxPoolPrefab;
        // [                                              SerializeField, FoldoutGroup("References")]
        // private Ocean oceanPfb;
        [                                              SerializeField, FoldoutGroup("References")]
        private DB dbPfb;
        [                                              SerializeField, FoldoutGroup("References")]
        private Inventory inventoryPfb;
        [                                              SerializeField, FoldoutGroup("References")]
        private UIManager uiManagerPfb;
        [                                              SerializeField, FoldoutGroup("References")]
        private DialogueManager dialogueManagerPfb;
        [                                              SerializeField, FoldoutGroup("References")]
        private AudioManager audioManagerPfb;
        [                                              SerializeField, FoldoutGroup("References")]
        private UICAudioManager uicAudioManagerPfb;
        
        [                                                   SerializeField, FoldoutGroup("Debug")] 
        public bool skipIntroVideo = true;
        
        [                       NonSerialized, ShowInInspector, ReadOnly, FoldoutGroup("Runtime")] 
        public bool mainSceneActivated;

        private void Awake()
        {
            InitInstance(this);
        }

        protected override void Setup()
        {
            Application.targetFrameRate = 60;

            SceneLoader.InitInstanceFromEmptyGameObject();

            SceneManager.activeSceneChanged += OnActiveSceneChanged;
            SceneManager.sceneLoaded += OnSceneLoaded;
            
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            // Inventory.Instance.lastLoginTime.Value = TimeManager.getTimeSecondsNow;

            // Debug.unityLogger.logEnabled = false;

            SRDebug.Init();
            SceneLoader.Instance.GoToScene("Main", LoadSceneMode.Single);
        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnActiveSceneChanged(Scene oldScene, Scene newScene)
        {
            Debug.Log($"GameManager: Active Scene has been changed to <color=yellow>{newScene.name}</color>");

            // these are called when scene is activated because Menu scene is loaded
            // additively and all the Awakes are happening before scene is active
            // That's why Pool was created under the Loading scene, destroyed and 
            // then created again empty in Menu scene
            switch (newScene.name)
            {
                case "Start":
                    break;
                case "Main":
                    mainSceneActivated = true;
                    InitSingletons();
                    if (skipIntroVideo)
                    {
                        UIManager.Instance.GoToComposition(Composition.MAIN);
                    }
                    else
                    {
                        var w = UIManager.Instance.GetWidget<VideoWidget>();
                        w.ShowForVideo(DB.Instance.videos.items["start"], () =>
                        {
                            UIManager.Instance.GoToComposition(Composition.MAIN);
                        });
                    }
                    break;
            }
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            Debug.Log($"GameManager: Scene Loaded <color=yellow>{scene.name}</color>");

            switch (scene.name)
            {
                case "Start":
                    break;
                case "Main":
                    break;
            }
        }

        public void InitSingletons()
        {
            DB.InitInstanceFromPrefab(dbPfb);
            Ocean.InitInstanceFromEmptyGameObject();
            Inventory.InitInstanceFromPrefab(inventoryPfb);
            DialogueManager.InitInstanceFromPrefab(dialogueManagerPfb);
            UIManager.InitInstanceFromPrefab(uiManagerPfb);
            AudioManager.InitInstanceFromPrefab(audioManagerPfb);
            UICAudioManager.InitInstanceFromPrefab(uicAudioManagerPfb);
        }
    }
}