// using System;
// using UnityEngine;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using Sirenix.OdinInspector;
//
// namespace RomenoCompany
// {
//     [CreateAssetMenu(
//     fileName = "TableAbilities", 
//     menuName = "ScriptableObjects/TableAbilities", 
//     order = 59)]
//     public class TableAbilities : Table<AbilityData.ItemID, AbilityData>
//     {
//         [SerializeField] string tableUrl = null;
//         [SerializeField] string sheetId = null;
//         [SerializeField] string assetsPath = null;
//
//         public AbilityData.ItemID[] abilitiesWithConstantSwings = null;
//
//         public AbilityData abilityAbsorbShieldBooster;
//
//         public int GetFirstAbilityUnlockLevel()
//         {
//             int firstAbilityLevelIndex = DB.Instance.progression.levels.FindIndex(x => x.type == LevelType.Ability);
//             firstAbilityLevelIndex = DB.Instance.progression.levels[firstAbilityLevelIndex].levelIndex;
//
//             return firstAbilityLevelIndex + 1;
//         }
//
//         public string GetAddSoundByAbilityItemID(AbilityData.ItemID itemId)
//         {
//             for (int i = 0; i < Items.Count; i++)
//             {
//                 if (Items[i].hasAddSound && Items[i].itemId == itemId)
//                 {
//                     return Items[i].soundName;
//                 }
//             }
//             
//             return null;
//         }
//
//         public string[] AbilitiesNamesForIDs(List<AbilityData.ItemID> ids)
//         {
//             string[] abilitiesNames = new string[ids.Count];
//             for (int i = 0; i < ids.Count; i++)
//             {
//                 AbilityData ad = Items.Find(a => a.itemId == ids[i]);
//                 if (ad != null)
//                 {
//                     abilitiesNames[i] = ad.Title;
//                 }
//             }
//             return abilitiesNames;
//         }
//         
//         #if UNITY_EDITOR
//         
//         [SerializeField] bool regenerateEnums = false;
//         
//         [Button]
//         public void ImportFromGoogleSheets()
//         {
//             var url = $"{tableUrl}/export?format=csv&gid={sheetId}";
//             var www = new System.Net.WebClient();
//             www.Encoding = System.Text.Encoding.UTF8;
//             
//             int columnID = -1, columnEnumValue = -1, columnUsedAs = -1,
//                 columnTitle = -1, columnDescription = -1, columnSwings = -1,
//                 columnAddSound = -1, columnLevelToUnlock = -1, 
//                 columnCoinsToUnlock = -1, columnAdsToUnlock = -1, columnInApp = -1;
//
//             www.DownloadProgressChanged += (object sender, System.Net.DownloadProgressChangedEventArgs e) =>
//             {
//
//                 UnityEditor.EditorUtility.DisplayProgressBar("Fetching..", url, (float)e.ProgressPercentage / 100f);
//             };
//             
//             www.DownloadStringCompleted += (object sender, System.Net.DownloadStringCompletedEventArgs e) =>
//             {
//                 
//                 //
//                 // convert data to double string array
//                 // 
//                 string data = e.Result;
//     
//                 data = new System.Text.RegularExpressions.Regex("\"([^\"]|\"\"|\\n)*\"").Replace(data, m => m.Value.Replace("\n", "\\n"));
//     
//                 var rowsRaw = data.Split(new[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.None);
//                 var grid = new List<List<string>>();
//                 for (int i = 0; i < rowsRaw.Length; i++)
//                 {
//                     grid.Add(new List<string>());
//                     grid[i] = rowsRaw[i].Split(',').ToList();
//                 }
//     
//                 //
//                 // assign columns we seek
//                 //
//                 for (int col = 0; col < grid[0].Count; col++)
//                 {
//                     if (grid[0][col] == "id") { columnID = col; }
//                     if (grid[0][col] == "enumValue") { columnEnumValue = col; }
//                     if (grid[0][col] == "usedAs") { columnUsedAs = col; }
//                     if (grid[0][col] == "title") { columnTitle = col; }
//                     if (grid[0][col] == "description") { columnDescription = col; }
//                     if (grid[0][col] == "swings") { columnSwings = col; }
//                     if (grid[0][col] == "addSound") { columnAddSound = col; }
//                     if (grid[0][col] == "levelToUnlock") { columnLevelToUnlock = col; }
//                     if (grid[0][col] == "coinsToUnlock") { columnCoinsToUnlock = col; }
//                     if (grid[0][col] == "adsToUnlock") { columnAdsToUnlock = col; }
//                     if (grid[0][col] == "inapp") { columnInApp = col; }
//                 }
//                 
//                 items = new List<AbilityData>();
//                     
//                     
//                 UnityEditor.EditorUtility.DisplayProgressBar("Creating Enum", url, 100);
//                 //
//                 // generate enum
//                 //
//                 
//                 if (regenerateEnums)
//                 {
//                    var path = "Assets/Scripts/Data/gen/AbilityData.cs";
//                     System.IO.StreamWriter writer = new System.IO.StreamWriter(path, false, Encoding.UTF8);
//                     writer.WriteLine("");
//                     writer.WriteLine("// ACHTUNG! ATTENTION! BHIMANIE! 注意！");
//                     writer.WriteLine("// This file is generated. ");
//                     writer.WriteLine("// Any changes you make manually inside this file will be LOST");
//                     writer.WriteLine("// on next enum reimport from google sheets");
//                     writer.WriteLine("// have a nice day");
//                     writer.WriteLine("");
//                     writer.WriteLine("namespace TSG.Data");
//                     writer.WriteLine("{");
//                     writer.WriteLine("    public partial class AbilityData");
//                     writer.WriteLine("    {");
//                     writer.WriteLine("        // IMPORTANT:");
//                     writer.WriteLine("        // As ItemID is converted to string and is used to send GameAnalytics Design event");
//                     writer.WriteLine("        // it SHOULD NOT exceed 16 symbols in length otherwise it will be truncated");
//                     writer.WriteLine("        // E.g. this may lead to VIP_SUPERSONIC_PUNCH_1 and VIP_SUPERSONIC_PUNCH_2 be both");
//                     writer.WriteLine("        // reported as VIP_SUPERSONIC_P");
//                     writer.WriteLine("        public enum ItemID");
//                     writer.WriteLine("        {");
//                     // [row][column]
//                     for (int row = 1; row < grid.Count; row++)
//                     {
//                         if (string.IsNullOrEmpty(grid[row][columnID])) continue;
//                         writer.WriteLine($"            {grid[row][columnID]}={int.Parse(grid[row][columnEnumValue])},");
//                     }
//                     writer.WriteLine("        }");
//                     writer.WriteLine("    }");
//                     writer.WriteLine("}");
//                     writer.Close();
//        
//                     UnityEditor.AssetDatabase.ImportAsset(path);  
//                 }
//                 
//                 UnityEditor.EditorUtility.DisplayProgressBar("Filling items", url, 100);
//
//                 //
//                 // fill items
//                 //
//                 for (int row = 1; row < grid.Count; row++)
//                 {
//                     if (string.IsNullOrEmpty(grid[row][columnID])) continue;
//                     
//                     var item = new AbilityData();
//                     
//                     //
//                     // assign data from spritesheet
//                     if (grid[row][columnUsedAs] != "ABILITY" && grid[row][columnUsedAs] != "BOOSTER") continue;
//                     
//                     item.itemId = (AbilityData.ItemID)(int.Parse(grid[row][columnEnumValue]));
//                     // Enum.TryParse(grid[row][columnRarity], out item.rarity);
//                     item.Title = grid[row][columnTitle];
//                     item.Description = grid[row][columnDescription].Replace("%COMMA%", ",");
//                     if (!bool.TryParse(grid[row][columnSwings], out item.swings))
//                     {
//                         item.swings = false;
//                     }
//                     item.soundName = grid[row][columnAddSound];
//                     // Debug.Log($"sound {grid[row][columnAddSound]} from {row}-{columnAddSound}");
//                     // if (item.soundName == "FALSE")
//                     // {
//                     //     Debug.Log("asdf");
//                     // }
//                     if (!int.TryParse(grid[row][columnLevelToUnlock], out item.levelToUnlock))
//                     {
//                         item.levelToUnlock = -1;
//                     }
//                     if (!int.TryParse(grid[row][columnAdsToUnlock], out item.adsToUnlock))
//                     {
//                         item.adsToUnlock = -1;
//                     }
//                     if (!int.TryParse(grid[row][columnCoinsToUnlock], out item.coinsToUnlock))
//                     {
//                         item.coinsToUnlock = -1;
//                     }
//                     if (!bool.TryParse(grid[row][columnInApp], out item.inapp))
//                     {
//                         item.inapp = false;
//                     }
//                     item.isBooster = grid[row][columnUsedAs] == "BOOSTER";
//                     
//                     //
//                     // assign asset data
//                     
//                     Debug.Log(grid[row][columnID]);
//                     var path = System.IO.Path.Combine(assetsPath, grid[row][columnID].ToLower());
//                     var files = System.IO.Directory.GetFiles(path).Where(str => !str.Contains(".meta")).ToList();
//                     //
//                     // var iconPath = files.sele
//                     var iconPath = files.Single(str => str.Contains("icon"));
//                     var soPath = files.Single(str => str.Contains("script"));
//                     //
//                     item.Icon = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(iconPath);
//                     item.so = UnityEditor.AssetDatabase.LoadAssetAtPath<ScriptableObject>(soPath);
//                     
//                     items.Add(item);
//                 }
//         
//                 UnityEditor.EditorUtility.ClearProgressBar();
//                 };
//             www.DownloadStringAsync(new Uri(url));
//         }
// #endif
//     }
// }