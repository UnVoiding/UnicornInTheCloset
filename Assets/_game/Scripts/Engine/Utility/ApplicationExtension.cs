using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEditor;

namespace RomenoCompany
{
    public static class ApplicationExtension
    {
        public static bool debugInternetReachability = true;
        public static NetworkReachability internetReachability
        {
            get
            {
#if UNITY_EDITOR
                return debugInternetReachability ? Application.internetReachability : NetworkReachability.NotReachable;
#else
            return Application.internetReachability;
#endif
            }
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Tools/internet/DisableInternetReachability")]
        public static void DisableInternetReachability()
        {
            debugInternetReachability = false;
        }
        [UnityEditor.MenuItem("Tools/internet/EnableInternetReachability")]
        public static void EnableInternetReachability()
        {
            debugInternetReachability = true;
        }
#endif
    }    
}

