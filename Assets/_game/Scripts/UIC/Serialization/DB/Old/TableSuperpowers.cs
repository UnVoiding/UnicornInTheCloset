// using System;
// using System.Collections.Generic;
// using UnityEngine;
// using System.Linq;
// using Random = UnityEngine.Random;
// using Sirenix.OdinInspector;
//
// namespace RomenoCompany
// {
//     [CreateAssetMenu(
//     fileName = "TableSuperpowers",
//     menuName = "ScriptableObjects/TableSuperpowers",
//     order = 59)]
//     public class TableSuperpowers : Table<SuperpowerData.ItemID, SuperpowerData>
//     {
//         public List<SuperpowerData.SuperpowerProgression> superpowerProgressions = new List<SuperpowerData.SuperpowerProgression>();
//         [Tooltip("Be CAREFUL N-1!!!!")] public int levelsCountCycle = 5;
//         public List<AbilityData.ItemID> superpowers = null;
//         [Range(0.0f, 100.0f)] public float boost = 30.0f;
//
//         public SuperpowerData.ItemID GetRandomSuperpower()
//         {
//             List<SuperpowerData.ItemID> ownedSuperpowers = Inventory.Instance.superpowers.Value;
//             List<SuperpowerData.ItemID> allSuperpowers = SuperpowerData.ItemID.GetValues(typeof(SuperpowerData.ItemID)).OfType<SuperpowerData.ItemID>().ToList();
//             List<SuperpowerData.ItemID> superpowersToChoose = allSuperpowers.Except(ownedSuperpowers).
//                 Except(new SuperpowerData.ItemID[1]{SuperpowerData.ItemID.NONE}).
//                 Where(x => !DB.Instance.superpowers.GetItem(x).inappPurchase).
//                 ToList();
//             int index = Random.Range(0, superpowersToChoose.Count);
//             SuperpowerData.ItemID res = superpowersToChoose[index];
//             Debug.Log($"Index - {index}, selected superower - {res}");
//             return res;
//         }
//
//         public int GetFirstSuperpowerUnlockLevel()
//         {
//             int firstSuperpowerLevelIndex = DB.Instance.progression.levels.FindIndex(x => x.type == LevelType.Boss);
//             firstSuperpowerLevelIndex = DB.Instance.progression.levels[firstSuperpowerLevelIndex].levelIndex;
//
//             return firstSuperpowerLevelIndex + 1;
//         }
//
//         public string[] SuperpowersNamesForIDs(List<SuperpowerData.ItemID> ids)
//         {
//             string[] superpowersNames = new string[ids.Count];
//             for (int i = 0; i < ids.Count; i++)
//             {
//                 SuperpowerData sd = Items.Find(s => s.itemId == ids[i]);
//                 if (sd != null)
//                 {
//                     superpowersNames[i] = sd.Title;
//                 }
//             }
//             return superpowersNames;
//         }
//
//         public List<int> GetSuperpowersDeltas()
//         {
//             List<int> deltas = new List<int>(superpowerProgressions.Count);
//             deltas.Add(superpowerProgressions[0].level);
//             for (ushort i = 0; i < superpowerProgressions.Count - 1; i++)
//             {
//                 deltas.Add(superpowerProgressions[i + 1].level - superpowerProgressions[i].level);
//             }
//             return deltas;
//         }
//     }
// }