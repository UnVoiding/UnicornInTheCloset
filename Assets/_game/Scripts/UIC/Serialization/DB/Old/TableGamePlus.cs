// using Sirenix.OdinInspector;
// using System.Collections.Generic;
// using UnityEngine;
//
// namespace RomenoCompany
// {
//     [CreateAssetMenu(
//     fileName = "TableGamePlus",
//     menuName = "ScriptableObjects/TableGamePlus",
//     order = 59)]
//     public class TableGamePlus : Table<LocationData.ItemID, LocationGamePlus>
//     {
//         [System.Serializable, InlineProperty, PropertySpace(4,4)]
//         public class MultiplierAndStep
//         {
//             public float multiplier = 1;
//             public float step = 0.2f;
//
//             public float CalculateValue(float value, int level)
//             {
//                 return value * CalculateProgression(multiplier, step, level);
//             }
//         }
//
//         [System.Serializable]
//         public class Talent
//         {
//             public TalentData.ItemID id = TalentData.ItemID.ATTACK_SPEED;
//             public MultiplierAndStep cost = new MultiplierAndStep();
//             public enum Type { Steped, Liniar }
//
//             public Type typeProgression = Type.Steped;
//
//             [ShowIf("ShowSteped")]
//             public MultiplierAndStep steped = new MultiplierAndStep();
//
//             [ShowIf("ShowLiniar")]
//             public int maxAdditiveLevel;
//             [ShowIf("ShowLiniar")]
//             public float maxValue;
//
//             private bool ShowSteped => typeProgression == Type.Steped;
//             private bool ShowLiniar => typeProgression == Type.Liniar;
//
//             public int CalculateCost(float value, int level)
//             {
//                 return Mathf.CeilToInt(value * CalculateProgression(cost.multiplier, cost.step, level));
//             }
//
//             public int CalculateMaxLevel(int maxLevel)
//             {
//                 if (typeProgression == Type.Liniar) return maxLevel + maxAdditiveLevel;
//                 if (typeProgression == Type.Steped) return int.MaxValue;
//                 return maxLevel;
//             }
//
//             public float CalculateValue(float value, int maxLevel, int level)
//             {
//                 if (typeProgression == Type.Steped) return value * CalculateProgression(steped.multiplier, steped.step, level);
//                 if (typeProgression == Type.Liniar) return Mathf.Lerp(value, maxValue, ((float)level - maxLevel) / maxAdditiveLevel);
//                 return value;
//             }
//         }
//
//         public MultiplierAndStep waveCoinsReward = new MultiplierAndStep();
//         public MultiplierAndStep winCoinsReward = new MultiplierAndStep();
//         public MultiplierAndStep abilityCoinsReward = new MultiplierAndStep();
//         public MultiplierAndStep arkadeTokensReward = new MultiplierAndStep();
//
//         public List<Talent> talents = new List<Talent>();
//
//         public bool GetEnemyModify(LocationData.ItemID location, EnemiesData.ItemID enemy, ParametrType parametr, int levelProgresion, out float value)
//         {
//             value = 0;
//             var locationData = Items.Find(x => x.Key == location);
//             if (locationData == null) return false;
//             var enemyData = locationData.enemyModificator.Find(x => x.enemy == enemy);
//             if (enemyData == null) return false;
//             var parametrData = enemyData.parametrModificators.Find(x => x.parametr == parametr);
//             if (parametrData == null) return false;
//             value = CalculateProgression(parametrData.value, parametrData.step, levelProgresion);
//             return true;
//         }
//
//         public int GetTalentMaxLevel(TalentData.ItemID id, int maxLevel)
//         {
//             var talentData = talents.Find(x => x.id == id);
//             if (talentData == null) return maxLevel;
//             return talentData.CalculateMaxLevel(maxLevel);
//         }
//         
//         public bool GetTalentCost(TalentData.ItemID id, float lastCost, int currentLevel, out int cost)
//         {
//             cost = (int)lastCost;
//             var talentData = talents.Find(x => x.id == id);
//             if (talentData == null) return false;
//             int levelGamePlus = DB.Instance.talents.GetItem(id)?.GetGamePlusLevel(currentLevel) ?? -1;
//             if (levelGamePlus <= 0) return false;
//             cost = Mathf.RoundToInt(lastCost * CalculateProgression(talentData.cost.multiplier, talentData.cost.step, levelGamePlus));
//             return true;
//         }
//
//         public bool GetTalentValue(TalentData.ItemID id, float lastValue, int maxLevel, int currentLevel, out float value)
//         {
//             value = lastValue;
//             var talentData = talents.Find(x => x.id == id);
//             if (talentData == null) return false;
//
//             int levelGamePlus = DB.Instance.talents.GetItem(id)?.GetGamePlusLevel(currentLevel) ?? -1;
//             if (levelGamePlus <= 0) return false;
//
//             value = talentData.CalculateValue(lastValue, maxLevel, levelGamePlus);
//
//             return true;
//         }
//
//         public static float CalculateProgression(float value, float step, int level)
//         {
//             return value + step * (level - 1);
//         }
//     }
// }