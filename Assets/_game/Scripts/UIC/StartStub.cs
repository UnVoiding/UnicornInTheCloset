using UnityEngine;
using UnityEngine.SceneManagement;

namespace RomenoCompany
{
    public class StartStub : MonoBehaviour
    {
        private void Start()
        {
            SceneManager.LoadScene("Start", LoadSceneMode.Single);
        }
    }
}
