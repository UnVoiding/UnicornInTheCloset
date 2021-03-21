// using System;
// using Sirenix.OdinInspector;
// using UnityEngine;
//
// [CreateAssetMenu(
// fileName = "TableEnergy", 
// menuName = "ScriptableObjects/TableEnergy", 
// order = 100)]
// public class TableEnergy : SerializedScriptableObject
// {
//     public int maxEnergy = 20;
//     public int energyPerGame = 5;
//     public int energyForAd = 5;
//     public int restoreOneEnergySeconds = 300;
//     public int restoreAdForEnergySeconds = 3000;
// }
//
// public static class EnergyHelper
// {
//     public static string TimeToMMSS(int time)
//     {
//         var ts = TimeSpan.FromSeconds(time);
//         return ts.ToString(@"mm\:ss");
//     }
//     
//     public static string TimeToHHMMSS(int time)
//     {
//         var ts = TimeSpan.FromSeconds(time);
//         return ts.ToString(@"hh\:mm\:ss");
//     }
//     
//     public static int UnixTimeSeconds
//     {
//         get
//         {
//             System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
//             return (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
//         }
//     }
// }