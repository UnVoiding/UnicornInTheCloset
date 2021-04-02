using UnityEngine.SceneManagement;
using UnityEngine;

namespace RomenoCompany
{
    public class SceneLoader : StrictSingleton<SceneLoader>
    {
        public Scene ActiveScene => SceneManager.GetActiveScene();

        protected override void Setup()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void GoToScene(string name, LoadSceneMode mode = LoadSceneMode.Single)
        {
            SceneManager.LoadScene(name, mode);//Debug.LogError("scene");
        }

        public AsyncOperation GoToSceneAsync(string name, LoadSceneMode mode = LoadSceneMode.Single)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(name, mode);
            return asyncOperation;
        }

        public AsyncOperation UnloadSceneAsync(string name)
        {
            AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(name);
            return asyncOperation;
        }

        public void ReloadActiveScene()
        {
            SceneManager.LoadScene(ActiveScene.name);
        }

        public Scene GetSceneByName(string name)
        {
            return SceneManager.GetSceneByName(name);
        }
    }
}
