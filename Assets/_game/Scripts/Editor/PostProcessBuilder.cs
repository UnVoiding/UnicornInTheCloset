using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using System.IO;
using UnityEditor.iOS.Xcode;
#endif

namespace RomenoCompany
{
    public class PostProcessBuilder
    {
        [PostProcessBuild]
        public static void ChangePlist(BuildTarget buildTarget, string pathToBuiltProject)
        {
#if UNITY_IOS
        if (buildTarget != BuildTarget.iOS) return;

        string plistPath = pathToBuiltProject + "/Info.plist";
        PlistDocument plist = new PlistDocument();
        plist.ReadFromString(File.ReadAllText(plistPath));
        PlistElementDict rootDict = plist.root;
        string exitsOnSuspendKey = "UIApplicationExitsOnSuspend";
        if (rootDict.values.ContainsKey(exitsOnSuspendKey))
        {
            rootDict.values.Remove(exitsOnSuspendKey);
        }

        if (!rootDict.values.ContainsKey("GADIsAdManagerApp"))
        {
            rootDict.SetBoolean("GADIsAdManagerApp", true);
        }

        if (!rootDict.values.ContainsKey("GADApplicationIdentifier"))
        {
            rootDict.SetString("GADApplicationIdentifier", "ca-app-pub-9334727648055753~1482799688");
        }

        File.WriteAllText(plistPath, plist.WriteToString());
#endif
        }
    }    
}
