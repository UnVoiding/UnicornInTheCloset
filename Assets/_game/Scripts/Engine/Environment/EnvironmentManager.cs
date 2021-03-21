using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace RomenoCompany
{
    public class EnvironmentManager : ResourcesSingleton<EnvironmentManager>
    {
        public enum EnvironmentName
        {
            GENERAL_PRODUCTION = 0,
            CHINESE_PRODUCTION = 10,
            GENERAL_DEBUG = 20,
            CHINESE_DEBUG = 30,
            NONE = 1000,
        }

        [ShowInInspector]
        public EnvironmentName currentProfile
        {
            get
            {
                TextAsset curProfile = Resources.Load<TextAsset>("CurrentEnvironmentProfile");
                if (curProfile == null) return EnvironmentName.GENERAL_PRODUCTION;
                else return (EnvironmentName) Enum.Parse(typeof(EnvironmentName), curProfile.text);
            }
        }
        
        public Dictionary<EnvironmentName, EnvironmentProfile> profiles = new Dictionary<EnvironmentName, EnvironmentProfile>();

        public EnvironmentProfile GetCurrentProfile()
        {
            if (currentProfile == EnvironmentName.NONE) return null;

            return profiles[currentProfile];
        }

        public void ClearEnvironmentDefines()
        {
    #if  UNITY_EDITOR
            ClearEnvironmentDefinesForPlatform(BuildTargetGroup.Android);
            ClearEnvironmentDefinesForPlatform(BuildTargetGroup.iOS);
    #endif
        }

    #if  UNITY_EDITOR
        public void ClearEnvironmentDefinesForPlatform(BuildTargetGroup buildTarget)
        {
            var rawDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTarget);
            var defines = rawDefines.Split(';').ToList();

            var envDefines = profiles.Values.SelectMany(x => x.additionalDefines).Distinct().ToList();
            var noEnvDefines = defines.Except(envDefines).ToList();

            string resultDefines = String.Join(";", noEnvDefines);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTarget, resultDefines);
        }
    #endif

        protected override void Setup()
        {
            // var profile = GetCurrentProfile();
            DontDestroyOnLoad(gameObject);
        }
    }
}

