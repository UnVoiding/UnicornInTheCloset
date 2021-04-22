using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;

namespace RomenoCompany
{
    public class PlatfromController : IActiveBuildTargetChanged
    {
        public int callbackOrder { get { return 0; } }
        public void OnActiveBuildTargetChanged(BuildTarget previousTarget, BuildTarget newTarget)
        {
            Debug.Log("Switched build target to " + newTarget);

            EnvironmentManager envManager = Resources.Load<EnvironmentManager>(EnvironmentManager.PrefabResourcePath);

            if (envManager != null)
            {
                envManager.GetCurrentProfile().Apply();
            }
            else
            {
                Debug.LogWarning("PlatformController: Warning: There is no EnvironmentManager prefab. Skipping.");
            }
        }
    }    
}
