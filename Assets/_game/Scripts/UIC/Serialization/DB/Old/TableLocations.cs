// using System.Collections.Generic;
// using UnityEngine;
//
// namespace RomenoCompany
// {
//     [CreateAssetMenu(
//     fileName = "TableLocations", 
//     menuName = "ScriptableObjects/TableLocations", 
//     order = 59)]
//     public class TableLocations : Table<LocationData.ItemID, LocationData>
//     {
//         [SerializeField] public DefeatBonus[] defeatRewards = new[] { new DefeatBonus(5000, 0) };
//         [System.Serializable]
//         public struct DefeatBonus : System.IComparable<DefeatBonus>
//         {
//             [Tooltip("from this value to next")]
//             public int fromLevel;
//             public int reward;
//
//             public DefeatBonus(int reward, int fromLevel)
//             {
//                 this.reward = reward;
//                 this.fromLevel = fromLevel;
//             }
//
//             public int CompareTo(DefeatBonus other)
//             {
//                 return fromLevel.CompareTo(other.fromLevel);
//             }
//         }
//         
//         [SerializeField] public List<EnemiesData.ItemID> availableCSGoldenEnemies = new List<EnemiesData.ItemID>(new []{EnemiesData.ItemID.Fighter, EnemiesData.ItemID.MiddleFighter, EnemiesData.ItemID.Archer});
//     }
// }