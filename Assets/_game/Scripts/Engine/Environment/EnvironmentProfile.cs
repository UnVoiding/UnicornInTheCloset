using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Serialization;

namespace RomenoCompany
{
    [CreateAssetMenu(fileName = "EnvironmentProfile", menuName = "TSG/Game/EnvironmentProfile", order = 60)]
    public class EnvironmentProfile : ScriptableObject
    {
        [FoldoutGroup("General")]
        public EnvironmentManager.EnvironmentName environment;
        [FoldoutGroup("General")]
        [Tooltip("CAUTION! Changing this may lead to save loss for current users")]
        public string companyName;
        [FoldoutGroup("General")]
        [Tooltip("Used for automatic signing in XCode")]
        public string appleDevelopmentTeamID;
        [FoldoutGroup("General")]
        [Tooltip("CAUTION! Changing this may lead to save loss for current users")]
        public string appName;
        [FoldoutGroup("General")]
        public string iosAppId;
        [FoldoutGroup("General")]
        public string version;
        [FoldoutGroup("General")]
        public string bundleIdentifier;

        [FormerlySerializedAs("gameAnalyticsGameKey")] [FoldoutGroup("GameAnalytics")]
        public string iOSgameAnalyticsGameKey;
        [FormerlySerializedAs("gameAnalyticsSecretKey")] [FoldoutGroup("GameAnalytics")]
        public string iOSgameAnalyticsSecretKey;
        [FoldoutGroup("GameAnalytics")]
        public string androidGameAnalyticsGameKey;
        [FoldoutGroup("GameAnalytics")]
        public string androidGameAnalyticsSecretKey;
        
        [FoldoutGroup("Amplitude")]
        public string amplitudeApiKey;
        
        [FoldoutGroup("AppsFlyer")]
        public string appsFlyerDevKey;

        [FormerlySerializedAs("ironSourceIosAppKey")] [FoldoutGroup("IronSource")]
        public string iOSIronSourceAppKey;
        [FoldoutGroup("IronSource")]
        public string androidIronSourceAppKey;

        [FormerlySerializedAs("facebookAppId")] [FoldoutGroup("Facebook")]
        public string iOSFacebookAppId;
        [FoldoutGroup("Facebook")]
        public string androidFacebookAppId;

        [FormerlySerializedAs("googleAdMobIosAppId")] [FoldoutGroup("GoogleAdMob")]
        public string iOSGoogleAdMobIosAppId;
        [FoldoutGroup("GoogleAdMob")]
        public string androidGoogleAdMobIosAppId;

        [FormerlySerializedAs("superSonicGameId")] [FoldoutGroup("SuperSonic")]
        public string iOSSuperSonicGameId;
        [FoldoutGroup("SuperSonic")]
        public string androidSuperSonicGameId;

        [FoldoutGroup("Defines")]
        public List<string> additionalDefines = new List<string>();

        // need to always call editor ManifestMod.GenerateManifest(); after applying profile
        public void Apply()
        {
    #if UNITY_EDITOR
            EnvironmentManager envManager = Resources.Load<EnvironmentManager>(EnvironmentManager.PrefabResourcePath);

            PlayerSettings.companyName = companyName;
            PlayerSettings.iOS.appleDeveloperTeamID = appleDevelopmentTeamID;
            PlayerSettings.productName = appName;
            PlayerSettings.bundleVersion = version;
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, bundleIdentifier);
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, bundleIdentifier);

            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
            {
                AddDefinesForPlatform(BuildTargetGroup.Android);
                AddDefinesForPlatform(BuildTargetGroup.iOS);
            }
            else
            {
                AddDefinesForPlatform(BuildTargetGroup.iOS);
                AddDefinesForPlatform(BuildTargetGroup.Android);
            }
    #endif
        }

    #if UNITY_EDITOR
        public void AddDefinesForPlatform(BuildTargetGroup buildTarget)
        {
            var rawDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTarget);
            var defines = rawDefines.Split(';').ToList();
            defines.AddRange(additionalDefines);
            defines = defines.Distinct().ToList();
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTarget, String.Join(";", defines));
        }
    #endif
    }
}

