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
//     fileName = "TableQuests", 
//     menuName = "ScriptableObjects/TableQuests", 
//     order = 59)]
//     public class TableQuests : Table<QuestData.ItemID, QuestData>
//     {
//         
//         [SerializeField] string tableUrl = null;
//         [SerializeField] string sheetId = null;
//         [SerializeField] string iconPath = null;
//         
//         [SerializeField] public float timeBetweenIngameBubbles = 3f;
//         
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
//             //id	face	placement	level	text	coinReward	gemReward
//             int columnID = -1, columnQuestRewardID = -1, columnFace = -1, 
//                 columnPlacement = -1, columnPosition = -1, columnBubble = -1,
//                 columnLevel = -1, columnText = -1, 
//                 columnCoinReward = -1, columnGemReward = -1;
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
//                 data = new System.Text.RegularExpressions.Regex("\"([^\"]|\"\"|\\n)*\"")
//                     .Replace(data, m => m.Value.Replace("\r\n", "||").Replace("\n", "||"));
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
//                     if (grid[0][col] == "rewardID") { columnQuestRewardID = col; }
//                     if (grid[0][col] == "face") { columnFace = col; }
//                     if (grid[0][col] == "placement") { columnPlacement = col; }
//                     if (grid[0][col] == "position") { columnPosition = col; }
//                     if (grid[0][col] == "bubble") { columnBubble = col; }
//                     if (grid[0][col] == "level") { columnLevel = col; }
//                     if (grid[0][col] == "text") { columnText = col; }
//                     if (grid[0][col] == "coinReward") { columnCoinReward = col; }
//                     if (grid[0][col] == "gemReward") { columnGemReward = col; }
//                 }
//                 
//                 items = new List<QuestData>();
//                     
//                     
//                 UnityEditor.EditorUtility.DisplayProgressBar("Creating Enum", url, 100);
//                 //
//                 // generate enum
//                 //
//                 
//                 if (regenerateEnums)
//                 {
//                    var path = "Assets/Scripts/Data/gen/QuestData.cs";
//                     System.IO.StreamWriter writer = new System.IO.StreamWriter(path, false, Encoding.UTF8);
//                     writer.WriteLine("namespace TSG.Data");
//                     writer.WriteLine("{");
//                     writer.WriteLine("    public partial class QuestData");
//                     writer.WriteLine("    {");
//                     writer.WriteLine("        public enum ItemID");
//                     writer.WriteLine("        {");
//                     for (int row = 1; row < grid.Count; row++)
//                     {
//                         if (string.IsNullOrEmpty(grid[row][columnID])) continue;
//                         writer.WriteLine($"            {grid[row][columnID]},");
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
//                 int enumRow = 0;
//                 for (int row = 1; row < grid.Count; row++)
//                 {
//                     if (string.IsNullOrEmpty(grid[row][columnID])) continue;
//                     
//                     var item = new QuestData();
//                     
//                     //
//                     // assign data from spritesheet
//                     
//                     
//                     item.itemId = (QuestData.ItemID) (enumRow);
//                     item.rewardID = grid[row][columnQuestRewardID].Replace("\"", "");
//                     item.placement = (QuestData.Placement)Enum.Parse(typeof(QuestData.Placement), grid[row][columnPlacement]);
//                     if (!string.IsNullOrEmpty(grid[row][columnPosition]))
//                     {
//                         item.position = (QuestData.Position)Enum.Parse(typeof(QuestData.Position), grid[row][columnPosition]);
//                     } else
//                     {
//                         item.position = QuestData.Position.MENU_POS;
//                     }
//                     item.bubble = (QuestData.Bubble)Enum.Parse(typeof(QuestData.Bubble), grid[row][columnBubble]);
//                     
//                     if (!int.TryParse(grid[row][columnLevel], out item.level))
//                     {
//                         item.level = -1;
//                     }
//                     
//                     if (columnCoinReward > grid[row].Count-1 ||!int.TryParse(grid[row][columnCoinReward], out item.coinReward))
//                     {
//                         item.coinReward = -1;
//                     }
//                     
//                     if (columnGemReward > grid[row].Count-1 || !int.TryParse(grid[row][columnGemReward], out item.gemReward))
//                     {
//                         item.gemReward = -1;
//                     }
//                     
//                     item.Description = grid[row][columnText].Replace("%COMMA%", ",").Replace("\"", "").Replace("||", "\n");
//                     
//                     var files = System.IO.Directory.GetFiles(this.iconPath).Where(str => !str.Contains(".meta")).ToList();
//                     var iconPath = files.Single(str => str.Contains($"icon_partner_{grid[row][columnFace]}"));
//                     item.Icon = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(iconPath);
//                     
//                     items.Add(item);
//                     
//                     enumRow++;
//                 }
//         
//                 UnityEditor.EditorUtility.ClearProgressBar();
//                 };
//             www.DownloadStringAsync(new Uri(url));
//         }
// #endif
//     }
// }