// using System;
// using Sirenix.OdinInspector;
// using UnityEngine;
// using System.Collections.Generic;
// using System.Linq;
// using System.Net;
// using System.Text;
// using System.Text.RegularExpressions;
//
// namespace RomenoCompany
// {
//     [CreateAssetMenu(fileName = "TableSkins2", menuName = "ScriptableObjects/TableSkins2", order = 60)]
//     public class TableSkins : Table<SkinData.ItemID, SkinData>
//     {
//         [SerializeField] string tableUrl = null;
//         [SerializeField] string sheetId = null;
//         [SerializeField] string assetsPath = null;
//         
//         public Dictionary<SkinRarityData.Rarity, SkinRarityData> _skinRarities;
//
//         public string GetSkinRarityString(SkinRarityData.Rarity rarity)
//         {
//             var rarityData = _skinRarities[rarity];
//             var color = ColorUtility.ToHtmlStringRGB(rarityData.rarityStringColor);
//             return $"<color=#{color}> {rarityData.rarityString} </color>";
//         }
//
//         public int GetFirstSkinUnlockLevel()
//         {
//             var UnlockLevels = (from x in DB.Instance.skins.Items select x.levelToUnlock).Where(x => x > 0).ToList();
//             return Mathf.Min(UnlockLevels.ToArray());
//         }
//
//         // [Button("SortByLevel")]
//         // public void SortByLevel()
//         // {
//         //     Items.Sort((x,y) => x.levelToUnlock.CompareTo(y.levelToUnlock));
//         // }
// #if UNITY_EDITOR
//         
//         [SerializeField] bool regenerateEnums = false;
//         
//         [Button]
//         public void ImportFromGoogleSheets()
//         {
//             var url = $"{tableUrl}/export?format=csv&gid={sheetId}";
//             var www = new WebClient();
//             www.Encoding = Encoding.UTF8;
//             
//             int columnID = -1, columnEnumValue = -1, columnRarity = -1, columnLevelToUnlock = -1, 
//                 columnCoinsToUnlock = -1, columnAdsToUnlock = -1, columnShowAfterLevelCompletion = -1, columnInapp = -1;
//
//             www.DownloadProgressChanged += (object sender, DownloadProgressChangedEventArgs e) =>
//             {
//
//                 UnityEditor.EditorUtility.DisplayProgressBar("Fetching..", url, (float)e.ProgressPercentage / 100f);
//             };
//             
//             www.DownloadStringCompleted += (object sender, DownloadStringCompletedEventArgs e) =>
//             {
//                 
//                 //
//                 // convert data to double string array
//                 // 
//                 string data = e.Result;
//     
//                 data = new Regex("\"([^\"]|\"\"|\\n)*\"").Replace(data, m => m.Value.Replace("\n", "\\n"));
//     
//                 var rowsRaw = data.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
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
//                     if (grid[0][col] == "rarity") { columnRarity = col; }
//                     if (grid[0][col] == "levelToUnlock") { columnLevelToUnlock = col; }
//                     if (grid[0][col] == "coinsToUnlock") { columnCoinsToUnlock = col; }
//                     if (grid[0][col] == "adsToUnlock") { columnAdsToUnlock = col; }
//                     if (grid[0][col] == "showAfterLevelCompletion") { columnShowAfterLevelCompletion = col; }
//                     if (grid[0][col] == "inapp") { columnInapp = col; }
//                 }
//                 
//                 items = new List<SkinData>();
//                     
//                     
//                 UnityEditor.EditorUtility.DisplayProgressBar("Creating Enum", url, 100);
//                 //
//                 // generate enum
//                 //
//                 
//                 if (regenerateEnums)
//                 {
//                    var path = "Assets/Scripts/Data/gen/SkinData.cs";
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
//                     writer.WriteLine("    public partial class SkinData : ICoreParam<SkinData.ItemID>");
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
//                     var item = new SkinData();
//                     
//                     //
//                     // assign data from spritesheet
//                     
//                     item.itemId = (SkinData.ItemID)(int.Parse(grid[row][columnEnumValue]));
//                     Enum.TryParse(grid[row][columnRarity], out item.rarity);
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
//                     if (!bool.TryParse(grid[row][columnShowAfterLevelCompletion], out item.showAfterLevelCompletion))
//                     {
//                         item.showAfterLevelCompletion = false;
//                     }
//                     if (!bool.TryParse(grid[row][columnInapp], out item.inApp))
//                     {
//                         item.inApp = false;
//                     }
//                     
//                     //
//                     // assign asset data
//                     
//                     Debug.Log(grid[row][columnID]);
//                     var path = System.IO.Path.Combine(assetsPath, grid[row][columnID].ToLower());
//                     var files = System.IO.Directory.GetFiles(path).Where(str => !str.Contains(".meta")).ToList();
//                     
//                     // var iconPath = files.sele
//                     var iconPath = files.Single(str => str.Contains("icon"));
//                     var meshPath = files.Single(str => str.Contains("mesh"));
//                     var materialsPath = files.Where(str => str.Contains("material")).ToList();
//                     var previewMaterialsPath = files.Where(str => str.Contains("previewMaterial")).ToList();
//                     var universalMaterialsPath = files.Where(str => str.Contains("universal")).ToList();
//                     var attachedPath = files.Any(str => str.Contains("attached")) ? files.Where(str => str.Contains("attached")) : new List<string>();
//                     var previewAttachedPath = files.Any(str => str.Contains("previewAttached")) ? files.Where(str => str.Contains("previewAttached")) : new List<string>();
//                     
//                     var icon = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(iconPath);
//                     var mesh = UnityEditor.AssetDatabase.LoadAssetAtPath<Mesh>(meshPath);
//                     List<Material> materials = new List<Material>();
//                     foreach (var mat in materialsPath)
//                     {
//                         materials.Add(UnityEditor.AssetDatabase.LoadAssetAtPath<Material>(mat));
//                     }
//                     List<Material> pMaterials = new List<Material>();
//                     foreach (var mat in previewMaterialsPath)
//                     {
//                         pMaterials.Add(UnityEditor.AssetDatabase.LoadAssetAtPath<Material>(mat));
//                     }
//                     
//                     foreach (var mat in universalMaterialsPath)
//                     {
//                         materials.Add(UnityEditor.AssetDatabase.LoadAssetAtPath<Material>(mat));
//                         pMaterials.Add(UnityEditor.AssetDatabase.LoadAssetAtPath<Material>(mat));
//                     }
//                     
//                     List<GameObject> attached = new List<GameObject>();
//                     foreach (var obj in attachedPath)
//                     {
//                         attached.Add(UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(obj));
//                     }
//                     
//                     List<GameObject> previewAttached = new List<GameObject>();
//                     foreach (var obj in previewAttachedPath)
//                     {
//                         previewAttached.Add(UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(obj));
//                     }
//                     
//                     item.Icon = icon;
//                     item.mesh = mesh;
//                     item.materials = materials.ToArray();
//                     item.previewMaterials = pMaterials.ToArray();
//                     item.attachedObjects = attached;
//                     item.previewAttachedObjects = previewAttached;
//                     
//                     this.items.Add(item);
//                 }
//         
//                 UnityEditor.EditorUtility.ClearProgressBar();
//                 };
//             www.DownloadStringAsync(new Uri(url));
//         }
// #endif
//
//         public string[] SkinsNamesForIDs(List<SkinData.ItemID> ids)
//         {
//             string[] skinsNames = new string[ids.Count];
//             for (int i = 0; i < ids.Count; i++)
//             {
//                 skinsNames[i] = ids[i].ToString();
//             }
//             return skinsNames;
//         }
//     }
// }
//
//
