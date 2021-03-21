// using UnityEngine;
// using System.Collections.Generic;
//
// namespace RomenoCompany
// {
//     [CreateAssetMenu(
//         fileName = "TableBoosters", 
//         menuName = "ScriptableObjects/TableBoosters", 
//         order = 59)]
//     public class TableBoosters : Table<BoosterData.ItemID, BoosterData>
//     {
//         public HashSet<AbilityData.ItemID> boosterAbilities;
//         public HashSet<AbilityData.ItemID> blockingAbilities;
//         readonly public Dictionary<BoosterData.ItemID, string> boosterNames = new Dictionary<BoosterData.ItemID, string>
//         {
//             { BoosterData.ItemID.ABILITY, "add_abitity" },
//             { BoosterData.ItemID.DAMAGE_ENEMIES, "all_damage" },
//             { BoosterData.ItemID.REWARD, "more_money" },
//             { BoosterData.ItemID.SHIELD, "shield" },
//             { BoosterData.ItemID.SUPERPOWER_FILL, "super_power" }
//         };
//     }
// }