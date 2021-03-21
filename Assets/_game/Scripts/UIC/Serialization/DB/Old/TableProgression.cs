// using Sirenix.OdinInspector;
// using Sirenix.Serialization;
// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using System.Net;
// using System.Text;
// using System.Text.RegularExpressions;
// using UnityEngine;
// using UnityEngine.Serialization;
//
// namespace RomenoCompany
// {
//     public enum WaveCommand
//     {
//         BeginWave, EndWave, Spawn, WaitSeconds, WaitKillAll, Ability, Boss, Bonus
//     }
//     public enum LevelType
//     {
//         Normal, Bonus, Ability, Boss
//     }
//
//     [Serializable]
//     public class LevelScript
//     {
//         [OdinSerialize] [NonSerialized] public List<WaveScript> waves = new List<WaveScript>();
//
//         public LevelType type;
//         [Space]
//         public ContainerAbility abilityContainer = null;
//         [Space]
//         public ContainerBonus bonusContainer = null;
//         [Space]
//         public int levelIndex = -1;
//         public int endLevelReward = -1;
//         public int endLevelKeysReward = -1;
//         public int endLevelTokensReward = -1;
//         public int location = -1;
//         public bool IsHard = false;
//
//         public int GetMaxPossibleEnemiesWithHealthBars()
//         {
//             return waves.Max(x => x.GetMaxPossibleEnemiesWithHealthBars());
//         }
//     }
//
//     [Serializable]
//     public class ContainerAbility
//     {
//         public static readonly int[] Indexes = {1,2,3};
//         public enum RewardType {Ability, Money}
//         public List<AbilityData.ItemID> ability = new List<AbilityData.ItemID>();
//         public List<int> rewards = new List<int>();
//         public int primaryAbility = -1;
//         public List<Reward> orderedRewards = new List<Reward>();
//
//         [Serializable]
//         public class Reward
//         {
//             public RewardType type = RewardType.Money;
//             public int amount = 0;
//             public bool tokens = false;
//             public AbilityData.ItemID abilityID = AbilityData.ItemID.NONE;
//             public int order = 0;
//
//             public Reward()
//             {
//
//             }
//
//             public Reward(RewardType type, int amount, AbilityData.ItemID abilityID, int order, bool tokens = false)
//             {
//                 this.type = type; this.amount = amount; this.abilityID = abilityID; this.order = order; this.tokens = tokens;
//             }
//         }
//     }
//
//     [Serializable]
//     public class ContainerBonus
//     {
//         public LocationData.ItemID location;
//     }
//
//     [Serializable]
//     public class WaveScript
//     {
//         public List<EnemyModificator> modificators = new List<EnemyModificator>();
//         [OdinSerialize] [NonSerialized] public List<LevelNode> commands = new List<LevelNode>();
//         public bool isBossFight = false;
//         public EnemiesData.ItemID boss = EnemiesData.ItemID.None;
//         public int waveIndex = -1;
//         public int enemiesOnLevel = 0;
//         public int waveReward = -1;
//
//         [FormerlySerializedAs("owner")] 
//         [SerializeField, HideInInspector] 
//         public LevelScript level;
//         
//         public int GetMaxPossibleEnemiesWithHealthBars()
//         {
//             int maxEnemies = 0;
//             int currentChunkMaxEnemies = 0;
//             for (int i = 0; i < commands.Count; i++)
//             {
//                 if (commands[i].Command == WaveCommand.BeginWave || 
//                     commands[i].Command == WaveCommand.WaitKillAll ||
//                     commands[i].Command == WaveCommand.EndWave)
//                 {
//                     if (currentChunkMaxEnemies > maxEnemies)
//                     {
//                         maxEnemies = currentChunkMaxEnemies;
//                     }
//                     currentChunkMaxEnemies = 0;
//                 }
//
//                 if (commands[i].Command == WaveCommand.Spawn)
//                 {
//                 }
//             }
//
//             return maxEnemies;
//         }
//     }
//
//     public interface LevelNode
//     {
//         WaveCommand Command { get; }
//         WaveScript Owner { get; }
//     }
//
//     [CreateAssetMenu(menuName = "TSG/Progression")]
//     public class TableProgression : SerializedScriptableObject
//     {
//         [SerializeField] string tableUrl = null;
//         [SerializeField] string[] sheetsIDs = null;
//         [ListDrawerSettings(NumberOfItemsPerPage = 5)] [OdinSerialize] [NonSerialized] public List<LevelScript> levels = null;
//
//         public LevelScript GetLevel(int level)
//         {
//             return levels[Mathf.Max(0, level % levels.Count)];
//         }
//
//         int columnLevel = -1;
//
//         int columnOverride = -1;
//
//         int columnWaveCommand = -1;
//         int columnWaveMult = -1;
//         int columnWaveEnemy = -1;
//         int columnWaveDirection = -1;
//         int columnWaveDropHeart = -1;
//         int columnWaveDropBerserk = -1;
//         int columnWaveDropWeapon = -1;
//         int columnWaveIsHard = -1;
//         int columnWaveValue = -1;
//         int columnIsGold = -1;
//         int columnBossName = -1;
//
//         Queue<string> sheets = new Queue<string>();
//         int index = 0;
//
//         [Button]
//         public void ImportFromGoogleSheets()
//         {
//             levels = new List<LevelScript>();
//             sheets = new Queue<string>(sheetsIDs);
//             index = 0;
//             CVSNext(sheets, () => { });
//         }
//
//         void CVSNext(Queue<string> cvs, System.Action onDone)
//         {
//             if (cvs.Count > 0)
//             {
//                 string sheet = cvs.Dequeue();
//                 CVSToGrid($"{tableUrl}/export?format=csv&gid={sheet}", (List<List<string>> result) =>
//                 {
//                     ParseLevels(result, index);
//                     index++;
//                     Debug.Log($"Done {sheet}");
//                     CVSNext(cvs, onDone);
//                 });
//             }
//             else
//             {
//                 Debug.Log("All Done");
// #if UNITY_EDITOR
//                 UnityEditor.EditorUtility.ClearProgressBar();
// #endif
//                 onDone.Invoke();
//             }
//         }
//
//         void CVSToGrid(string url, System.Action<List<List<string>>> onDone)
//         {
//             var www = new WebClient();
//             www.Encoding = Encoding.UTF8;
//
//             www.DownloadProgressChanged += (object sender, DownloadProgressChangedEventArgs e) =>
//             {
//
// #if UNITY_EDITOR
//                 UnityEditor.EditorUtility.DisplayProgressBar("Download", url, (float)e.ProgressPercentage / 100f);
// #endif
//             };
//             www.DownloadStringCompleted += (object sender, DownloadStringCompletedEventArgs e) =>
//         {
//             string data = e.Result;
//
//             data = new Regex("\"([^\"]|\"\"|\\n)*\"").Replace(data, m => m.Value.Replace("\n", "\\n"));
//
//             var rowsRaw = data.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
//             var grid = new List<List<string>>();
//             for (int i = 0; i < rowsRaw.Length; i++)
//             {
//                 grid.Add(new List<string>());
//                 grid[i] = rowsRaw[i].Split(',').ToList();
//             }
//
//             for (int col = 0; col < grid[0].Count; col++)
//             {
//                 if (grid[0][col] == "lvl") { columnLevel = col; }
//                 if (grid[0][col] == "command") { columnWaveCommand = col; }
//                 if (grid[0][col] == "mult") { columnWaveMult = col; }
//                 if (grid[0][col] == "enemy") { columnWaveEnemy = col; }
//                 if (grid[0][col] == "direction") { columnWaveDirection = col; }
//                 if (grid[0][col] == "heart") { columnWaveDropHeart = col; }
//                 if (grid[0][col] == "berserk") { columnWaveDropBerserk = col; }
//                 if (grid[0][col] == "weapon") { columnWaveDropWeapon = col; }
//                 if (grid[0][col] == "IsHard") { columnWaveIsHard = col; }
//                 if (grid[0][col] == "value") { columnWaveValue = col; }
//                 if (grid[0][col] == "oKey") { columnOverride = col; }
//                 if (grid[0][col] == "IsGold") { columnIsGold = col; }
//                 if (grid[0][col] == "bossName") { columnBossName = col; }
//             }
//
//             onDone.Invoke(grid);
//         };
//             www.DownloadStringAsync(new Uri(url));
//         }
//
//         private void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
//         {
//             Debug.Log(e.ProgressPercentage);
//         }
//
//         private void DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
//         {
//             Debug.Log("done");
//         }
//
//         void ParseLevels(List<List<string>> data, int location)
//         {
//             HashSet<int> levelHards = new HashSet<int>();
//             List<int> levelStart = new List<int>();
//             List<int> levelEnd = new List<int>();
//             List<LevelType> levelTypes = new List<LevelType>();
//
//             Dictionary<int, ContainerAbility> abilityContainers = new Dictionary<int, ContainerAbility>();
//             Dictionary<int, ContainerBonus> bonusContainers = new Dictionary<int, ContainerBonus>();
//             Dictionary<int, int> keysContainers = new Dictionary<int, int>();
//             Dictionary<int, int> tokensContainers = new Dictionary<int, int>();
//
//             int indexLevel = 0;
//             for (int row = 1; row < data.Count; row++)
//             {
//                 if (data[row][columnLevel] == "start")
//                 {
//                     levelTypes.Add(LevelType.Normal);
//                     int index = indexLevel = levelStart.Count;
//                     levelStart.Add(row);
//                     if (row + 1 < data.Count && columnLevel < data[row + 1].Count)
//                     {
//                         var type = data[row + 1][columnLevel];
//                         if (type.Contains("bonus"))
//                         {
//                             levelTypes[index] = LevelType.Bonus;
//
//                             ContainerBonus containerBonus = new ContainerBonus();
//
//                             var mach = Regex.Match(type, @"-l(.*)", RegexOptions.IgnoreCase);
//                             if (mach.Success)
//                             {
//                                 if (Enum.TryParse<LocationData.ItemID>(mach.Result("$1"), out LocationData.ItemID result)) containerBonus.location = result;
//                                 else containerBonus.location = LocationData.ItemID.LOCATION_1;
//
//                                 bonusContainers.Add(index, containerBonus);
//                             }
//                         }
//                         if (type.Contains("ability"))
//                         {
//                             ContainerAbility containerAbility = new ContainerAbility();
//                             int indexPrimaryAbility = 0;
//                             levelTypes[index] = LevelType.Ability;
//                             List<int> indexes = new List<int>(3);
//                             int tokens = 0;
//                             bool hasTokens = false;
//
//                             for (int innerRow = row + 2; innerRow < data.Count; innerRow++)
//                             {
//                                 type = data[innerRow][columnLevel];
//                                 if (type == "end" || type == "ability stop") break;
//                                 if (string.IsNullOrEmpty(type)) continue;
//
//                                 if (type.Contains("get"))
//                                 {
//                                     int startIndex = type.IndexOf(" get");
//                                     string getS = type.Split(' ')[1];
//                                     int.TryParse(getS.Remove(0, 3), out int orderedIndex);
//                                     string m = type.Remove(startIndex);
//                                     int.TryParse(m, out int result);
//                                     containerAbility.rewards.Add(result);
//                                     indexes.Add(orderedIndex);
//
//                                     containerAbility.orderedRewards.Add(
//                                         new ContainerAbility.Reward(ContainerAbility.RewardType.Money, result, AbilityData.ItemID.NONE, orderedIndex));
//                                 }
//                                 else if(type.Contains(" tokens"))
//                                 {
//                                     int startIndex = type.IndexOf(" tokens");
//                                     string count = type.Split(' ')[0];
//                                     int.TryParse(count, out tokens);
//                                     hasTokens = true;
//                                 }
//                                 else
//                                 {
//                                     string[] texts = type.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
//                                     if (texts.Length > 0)
//                                     {
//                                         var text = texts[0].Trim();
//                                         if (Enum.TryParse<AbilityData.ItemID>(text, out AbilityData.ItemID a))
//                                         {
//                                             containerAbility.ability.Add(a);
//                                             if (texts.Length > 1)
//                                             {
//                                                 if (texts[1].Contains("-p")) containerAbility.primaryAbility = indexPrimaryAbility;
//                                             }
//                                             indexPrimaryAbility++;
//                                         }
//                                     }
//                                 }
//                             }
//
//                             int abilityIndex = ContainerAbility.Indexes.Except(indexes).ToArray()[0];
//                             containerAbility.orderedRewards.Add(new ContainerAbility.Reward(ContainerAbility.RewardType.Ability, tokens, AbilityData.ItemID.NONE, abilityIndex, hasTokens));
//
//                             abilityContainers.Add(index, containerAbility);
//                         }
//                     }
//                 }
//                 if (data[row][columnLevel].Contains("keys"))
//                 {
//                     keysContainers.Add(indexLevel, int.Parse(data[row][columnLevel].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1]));
//                 }
//                 if (data[row][columnLevel].Contains("tokens "))
//                 {
//                     tokensContainers.Add(indexLevel, int.Parse(data[row][columnLevel].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1]));
//                 }
//                 if (data[row][columnLevel] == "hard level")
//                 {
//                     levelHards.Add(indexLevel);
//                 }
//                 if (data[row][columnLevel] == "end")
//                 {
//                     levelEnd.Add(row);
//                 }
//             }
//
//             if (levelStart.Count != levelEnd.Count)
//             {
//
// #if UNITY_EDITOR
//                 UnityEditor.EditorUtility.ClearProgressBar();
//                 UnityEditor.EditorUtility.DisplayDialog("Parse Error!", $"on locatoin {location} : mismatch of levels braces: {levelStart.Count} starts; but {levelEnd.Count} ends", "Ok");
// #endif
//                 throw new UnityException($"on locatoin {location} : mismatch of levels braces: {levelStart.Count} starts; but {levelEnd.Count} ends");
//             }
//
//             for (int i = 0; i < levelStart.Count; i++)
//             {
//                 var nextLevel = levels.Count;
//                 Debug.Log($"level {nextLevel + 1}: [{levelStart[i]}->{levelEnd[i]}]");
//                 levels.Add(new LevelScript {
//                     location = location,
//                     levelIndex = nextLevel,
//                     endLevelReward = int.Parse(data[levelEnd[i]][columnWaveValue]),
//                     type = levelTypes[i],
//                     abilityContainer = abilityContainers.ContainsKey(i) ? abilityContainers[i] : null,
//                     bonusContainer = bonusContainers.ContainsKey(i) ? bonusContainers[i] : null,
//                     endLevelKeysReward = keysContainers.ContainsKey(i) ? keysContainers[i] : -1,
//                     endLevelTokensReward = tokensContainers.ContainsKey(i) ? tokensContainers[i] : -1,
//                     IsHard = levelHards.Contains(i),
//                 });
//                 ParseLevel(data, levels[nextLevel], levelStart[i], levelEnd[i]);
//             }
//         }
//
//         void ParseLevel(List<List<string>> data, LevelScript lvl, int levelStart, int levelEnd)
//         {
//             int waveCounter = 0;
//             int waveStart = -1, waveEnd = -1;
//             bool isBossFight = false;
//             for (int row = levelStart; row <= levelEnd; row++)
//             {
//                 var cell = data[row][columnWaveCommand];
//                 if (Enum.TryParse(cell, out WaveCommand value))
//                 {
//                     if (value == WaveCommand.BeginWave)
//                     {
//                         waveStart = row;
//                         var argumentWave = data[row][columnWaveValue];
//                         if (!string.IsNullOrEmpty(argumentWave))
//                         {
//                             if (argumentWave == "boss")
//                             {
//                                 lvl.type = LevelType.Boss;
//                                 isBossFight = true;
//                             }
//                             else isBossFight = false;
//                         }
//                         else isBossFight = false;
//                     }
//
//                     if (value == WaveCommand.EndWave && waveStart > 0)
//                     {
//                         waveEnd = row;
//                         lvl.waves.Add(new WaveScript { waveIndex = waveCounter, level = lvl, isBossFight = isBossFight });
//                         // Debug.Log($"    wave {waveCounter}: [{waveStart}->{waveEnd}]");
//                         ParseWave(data, lvl.waves[waveCounter], waveStart, waveEnd);
//                         waveStart = waveEnd = -1;
//                         waveCounter++;
//                     }
//                 }
//             }
//         }
//
//         void ParseWave(List<List<string>> data, WaveScript wave, int waveStart, int waveEnd)
//         {
//             for (int row = waveStart; row <= waveEnd; row++)
//             {
//                 var cell = data[row][columnOverride];
//
//                 if (Enum.TryParse(cell, out EnemiesData.ItemID key))
//                 {
//                     var mod = new ParametrModificator();
//                     //mod.parametr = (ParametrType)Enum.Parse(typeof(ParametrType), data[row][columnOverride+1]);
//                     TryParse<ParametrType>(row, columnOverride + 1, data, out mod.parametr);
//                     TryParse<float>(row, columnOverride + 2, data, out mod.value); 
//                     //float.Parse(data[row][columnOverride+2], System.Globalization.CultureInfo.GetCultureInfo("en-US"));
//
//                     bool addedParametr = false;
//                     foreach (var m in wave.modificators)
//                     {
//                         if (m.enemy == key)
//                         {
//                             m.parametrModificators.Add(mod);
//                             addedParametr = true;
//                             break;
//                         }
//                     }
//                     if (!addedParametr)
//                     {
//                         var list = new List<ParametrModificator>();
//                         list.Add(mod);
//                         wave.modificators.Add(new EnemyModificator { enemy = key, parametrModificators = list });
//                     }
//                 }
//
//                 cell = data[row][columnWaveCommand];
//                 if (!Enum.TryParse(cell, out WaveCommand command)) continue;
//
//                 if (command == WaveCommand.Spawn)
//                 {
//                     //int mult;
//                     int.TryParse(data[row][columnWaveMult], out int mult);
//                     //TryParse<int>(row, columnWaveMult, data, out int mult);
//                     if (mult == 0) mult = 1; // empty string parsing will become 0, so go to default value
//
//                     wave.enemiesOnLevel += mult;
//
//                     EnemiesData.ItemID enemyKey = EnemiesData.ItemID.None; 
//                     //var enemyKey = (EnemyKey)Enum.Parse(typeof(EnemyKey), data[row][columnWaveEnemy]);
//                     TryParse<EnemiesData.ItemID>(row, columnWaveEnemy, data, out enemyKey);
//                     //var direction = (ScreenSpaceAnchor)Enum.Parse(typeof(ScreenSpaceAnchor), data[row][columnWaveDirection]);
//                     var dropHeart = data[row][columnWaveDropHeart] == "TRUE";
//                     var dropBerserk = data[row][columnWaveDropBerserk] == "TRUE";
//                     var dropWeapon = data[row][columnWaveDropWeapon] == "TRUE";
//                     var gold = (columnIsGold != -1 && data[row][columnIsGold] == "TRUE") ? enemyKey : EnemiesData.ItemID.None;
//                     var bossName = (columnBossName != -1) ? data[row][columnBossName] : string.Empty;
//                     if(bossName != string.Empty)
//                     {
//                         Debug.Log(bossName);
//                     }
//
//                     var enemyIsHard = false;
//                     if(columnWaveIsHard > -1)
//                     {
//                         enemyIsHard = data[row][columnWaveIsHard] == "TRUE";
//                     }
//
//
//                     // parse offsets
//                     var offsetsSrc = data[row][columnWaveValue];
//                     var offsetArr = offsetsSrc.Split(';');
//                     List<float> offsets = new List<float>();
//                     if (offsetArr.Length == mult)
//                     {
//                         for (int i = 0; i < offsetArr.Length; i++)
//                         {
//                             if (offsetArr[i].Length == 0)
//                             {
//                                 offsets.Add(3f);
//                                 // Debug.LogWarning($"Offset not specified in wave {wave.waveIndex}, row: {row + 1}");
//                             }
//                             else
//                             {
//                                 offsets.Add(float.Parse(offsetArr[i], System.Globalization.CultureInfo.GetCultureInfo("en-US")));
//                             }
//                         }
//                     }
//                     else
//                     {
//                         throw new UnityException($"Offsets mismatch in wave {wave.waveIndex}, row {row + 1} : {mult} enemies vs {offsetArr.Length} offsets");
//                     }
//
//                     // if (wave.commands[wave.commands.Count - 1] is NodeSpawnGroupEnemy)
//                     // {
//                     //     spawnCommand = wave.commands[wave.commands.Count - 1] as NodeSpawnGroupEnemy;
//                     //     if (spawnCommand.dropHeart != dropHeart)
//                     //     {
//                     //         wave.commands.Add(new NodeSpawnGroupEnemy(wave));
//                     //         spawnCommand = wave.commands[wave.commands.Count - 1] as NodeSpawnGroupEnemy;
//                     //     }
//                     //     if (spawnCommand.dropBerserk != dropBerserk)
//                     //     {
//                     //         wave.commands.Add(new NodeSpawnGroupEnemy(wave));
//                     //         spawnCommand = wave.commands[wave.commands.Count - 1] as NodeSpawnGroupEnemy;
//                     //     }
//                     //     if (spawnCommand.dropWeapon != dropWeapon)
//                     //     {
//                     //         wave.commands.Add(new NodeSpawnGroupEnemy(wave));
//                     //         spawnCommand = wave.commands[wave.commands.Count - 1] as NodeSpawnGroupEnemy;
//                     //     }
//                     // }
//                     // else
//                     // {
//                     //     wave.commands.Add(new NodeSpawnGroupEnemy(wave));
//                     //     spawnCommand = wave.commands[wave.commands.Count - 1] as NodeSpawnGroupEnemy;
//                     // }
//                     
//
//                     if (wave.isBossFight)
//                     {
//                         wave.boss = enemyKey;
//                     }
//                 }
//
//                 if (command == WaveCommand.EndWave)
//                 {
//                     TryParse<int>(row, columnWaveValue, data, out wave.waveReward);
//                     // wave.waveReward = int.Parse(data[row][columnWaveValue]);
//                 }
//
//                 if (command == WaveCommand.BeginWave)
//                 {
//                 }
//
//                 if (command == WaveCommand.WaitSeconds)
//                 {
//                 }
//
//                 if (command == WaveCommand.WaitKillAll)
//                 {
//                 }
//
//                 if (command == WaveCommand.Ability)
//                 {
//                 }
//             }
//         }
//
//         public bool TryParse<T>(int row, int colum, List<List<string>> data, out T value)
//         {
//             var type = typeof(T);
//             try
//             {
//                 var v = data[row][colum];
//
//                 if (string.IsNullOrEmpty(v))
//                 {
//                     // Debug.LogError($"Null value [{row}:{colum}] type of {typeof(T)}");
//                     value = default(T);
//                     return false;
//                 }
//
//                 if (type.IsEnum)
//                 {
//                     value = (T)Enum.Parse(typeof(T), v);
//                     return true;
//                 }
//
//                 if (type.IsValueType)
//                 {
//                     value = (T)Convert.ChangeType(v, typeof(T), System.Globalization.CultureInfo.GetCultureInfo("en-US"));
//                     return true;
//                 }
//
//
//                 value = default(T);
//                 return false;
//             }
//             catch
//             {
//                 value = default(T);
//                 Debug.LogError($"Error parse [{row}:{colum}] value: {data[row][colum]} type of {typeof(T)}");
//                 return false;
//             }
//         }
//     }
// }