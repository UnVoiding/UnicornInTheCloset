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
        [FoldoutGroup("Prefabs")]
        [SerializeField] private Ocean oceanPfb;
        [FoldoutGroup("Prefabs")]
        [SerializeField] private DB dbPfb;
        [FoldoutGroup("Prefabs")]
        [SerializeField] private Inventory inventoryPfb;
        
        [FoldoutGroup("Runtime")]
        [NonSerialized, ShowInInspector, ReadOnly] public bool mainSceneActivated;

        private void Awake()
        {
            InitInstance(this);
        }

        protected override void Setup()
        {
            Application.targetFrameRate = 60;
            
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
            SceneManager.sceneLoaded += OnSceneLoaded;
            
            DontDestroyOnLoad(this);
        }

        private IEnumerator Start()
        {
            // Inventory.Instance.lastLoginTime.Value = TimeManager.getTimeSecondsNow;
            
            // first scene waits for 7 frames for iOS watchdog not to kill the game
            // why - ???
            yield return new WaitForEndOfFrame();
            Debug.Log("preload frame 0");
            yield return new WaitForEndOfFrame();
            Debug.Log("preload frame 1");
            yield return new WaitForEndOfFrame();
            Debug.Log("preload frame 2");
            yield return new WaitForEndOfFrame();
            Debug.Log("preload frame 3");
            yield return new WaitForEndOfFrame();
            Debug.Log("preload frame 4");
            yield return new WaitForEndOfFrame();
            Debug.Log("preload frame 5");
            yield return new WaitForEndOfFrame();
            // Debug.unityLogger.logEnabled = false;
            SceneLoader.Instance.GoToScene("Loading", LoadSceneMode.Single);
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
            Inventory.InitInstanceFromPrefab(inventoryPfb);
        }
    }
}