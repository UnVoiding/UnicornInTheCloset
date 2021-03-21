// using System.Linq;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System;
// using Sirenix.OdinInspector;
// using Random = UnityEngine.Random;
//
// namespace RomenoCompany
// {
//     [CreateAssetMenu(fileName = "TableEnemies", menuName = "ScriptableObjects/TableEnemies", order = 59)]
//     public class TableEnemies : Table<EnemiesData.ItemID, EnemiesData>
//     {
//         public Dictionary<EnemiesData.ItemID, GameObject> bossAnouncers;
//
//         public float GetHardLvlMultiplier(EnemiesData.ItemID enemyID, ParametrType type)
//         {
//             var hardLevelParameters = GetItem(enemyID).hardLevelParameters;
//             
//             for (ushort j = 0; j < hardLevelParameters.Length; j++)
//             {
//                 if (hardLevelParameters[j].type == type)
//                 {
//                     return hardLevelParameters[j].value;
//                 }
//             }
//
//             return 1.0f;
//         }
//
//         public float GetHardEnemyMultiplier(EnemiesData.ItemID enemyID, ParametrType type)
//         {
//             var hardEnemyParameters = GetItem(enemyID).hardEnemyParameters;
//             
//             for (ushort j = 0; j < hardEnemyParameters.Length; j++)
//             {
//                 if (hardEnemyParameters[j].type == type)
//                 {
//                     return hardEnemyParameters[j].value;
//                 }
//             }
//
//             return 1.0f;
//         }
//        
//         public string GetHitSoundForEnemyWithKey(EnemiesData.ItemID key)
//         {
//             // for (int i = 0; i < Items.Count; i++)
//             // {
//             //     if (Items[i].Key == key && Items[i].sounds != null)
//             //     {
//             //         return Items[i].sounds.hitSound;
//             //     }
//             // }
//
//             // return null;
//             string res = null;
//             switch (key)
//             {
//                 case EnemiesData.ItemID.Fighter:
//                 case EnemiesData.ItemID.Archer:
//                 case EnemiesData.ItemID.MiddleFighter:
//                 case EnemiesData.ItemID.Ninja:
//                 case EnemiesData.ItemID.SuperNinja:
//                 case EnemiesData.ItemID.NinjaArmed:
//                 case EnemiesData.ItemID.Roller:
//                 case EnemiesData.ItemID.Biker:
//                 case EnemiesData.ItemID.PunkMolotov:
//                 case EnemiesData.ItemID.ShieldBandit:
//                 case EnemiesData.ItemID.ArmedBandit:
//                     res = "enemy_hurt";
//                     break;
//                 case EnemiesData.ItemID.BigFighter:
//                 case EnemiesData.ItemID.CrusherThug:
//                 case EnemiesData.ItemID.GrenadeThrower1:
//                 case EnemiesData.ItemID.GrenadeThrower3:
//                 case EnemiesData.ItemID.RocketThug1:
//                 case EnemiesData.ItemID.RocketThug3:
//                 case EnemiesData.ItemID.Gangster:
//                 case EnemiesData.ItemID.BossSharkHead:
//                 case EnemiesData.ItemID.BossBanditLeader:
//                     res = "big_enemy_hurt";
//                     break;
//             }
//
//             return res;
//         }
//
//         public string GetDeathSoundForEnemyWithKey(EnemiesData.ItemID key)
//         {
//             // for (int i = 0; i < Items.Count; i++)
//             // {
//             //     if (Items[i].Key == key && Items[i].sounds != null)
//             //     {
//             //         return Items[i].sounds.deathSound;
//             //     }
//             // }
//             
//             // return null;
//             string res = null;
//             switch (key)
//             {
//                 case EnemiesData.ItemID.Fighter:
//                 case EnemiesData.ItemID.Archer:
//                 case EnemiesData.ItemID.MiddleFighter:
//                 case EnemiesData.ItemID.Ninja:
//                 case EnemiesData.ItemID.SuperNinja:
//                 case EnemiesData.ItemID.NinjaArmed:
//                 case EnemiesData.ItemID.Roller:
//                 case EnemiesData.ItemID.Biker:
//                 case EnemiesData.ItemID.PunkMolotov:
//                 case EnemiesData.ItemID.ShieldBandit:
//                 case EnemiesData.ItemID.ArmedBandit:
//                     res = "enemy_hurt";
//                     break;
//                 case EnemiesData.ItemID.BigFighter:
//                 case EnemiesData.ItemID.CrusherThug:
//                 case EnemiesData.ItemID.GrenadeThrower1:
//                 case EnemiesData.ItemID.GrenadeThrower3:
//                 case EnemiesData.ItemID.RocketThug1:
//                 case EnemiesData.ItemID.RocketThug3:
//                 case EnemiesData.ItemID.Gangster:
//                 case EnemiesData.ItemID.BossSharkHead:
//                 case EnemiesData.ItemID.BossBanditLeader:
//                     res = "big_enemy_hurt";
//                     break;
//             }
//
//             return res;
//         }
//     }
// }