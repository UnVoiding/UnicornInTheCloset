// using Sirenix.OdinInspector;
// using UnityEngine;
//
// namespace RomenoCompany
// {
//     [CreateAssetMenu(
//     fileName = "TableTalents", 
//     menuName = "ScriptableObjects/TableTalents", 
//     order = 59)]
//     public class TableTalents : Table<TalentData.ItemID, TalentData>
//     {
//         public int maxLevels = 60;
//         public int[] cost = null;
//         
//         [Title("Mockup cost fill")]
//         [SerializeField] int startCost = 100;
//         [SerializeField] int increment = 100;
//
//         [Header("Attack speed curve")]
//         [SerializeField] AnimationCurve upgradeCurve = new AnimationCurve();
//         [OnValueChanged("ChangeCurve")] [SerializeField] Vector2 attackSpeedValueRange = new Vector2(1.0f, 1.2f);
//         [SerializeField] int attackSpeedLevelCount = 20;
//
//         [Button("Calculate attack speed")]
//         void Calculate()
//         {
//             float step = 1.0f / (attackSpeedLevelCount - 1);
//             TalentData.TalentProgression[] newProgression = new TalentData.TalentProgression[attackSpeedLevelCount];
//             for (int i = 0; i < attackSpeedLevelCount; i++)
//             {
//                 newProgression[i].value = upgradeCurve.Evaluate(i * step);
//             }
//
//             TalentData td = GetItem(TalentData.ItemID.ATTACK_SPEED);
//             td.progression = newProgression;
//         }
//
//         void ChangeCurve()
//         {
//             if (upgradeCurve.length < 2) return;
//
//             Keyframe[] keyframes = upgradeCurve.keys;
//             Keyframe first = new Keyframe(0.0f, attackSpeedValueRange.x);
//             Keyframe last = new Keyframe(1.0f, attackSpeedValueRange.y);
//             keyframes[0] = first;
//             keyframes[keyframes.Length - 1] = last;
//             upgradeCurve.keys = keyframes;
//         }
//
//         
//         [Button]
//         void fillMocupCost()
//         {
//             cost = new int[maxLevels];
//             for (int i = 0; i < maxLevels; i++)
//             {
//                 cost[i] = startCost + i * increment;
//             }
//         }
//
//         public bool ShowTalents
//         {
//             get
//             {
//                 if (!Inventory.Instance.ftueStages.Value.talentsUpgrade)
//                 {
//                     int blockingStage = 1000;
//                     for (int i = 0; i < Items.Count; i++)
//                     {
//                         for (int j = 0; j < Items[i].progression.Length; j++)
//                         {
//                             if (blockingStage > Items[i].progression[j].blockingStage && Items[i].progression[j].blocked)
//                             {
//                                 blockingStage = Items[i].progression[j].blockingStage;
//                             }
//                         }
//                     }
//
//                     return blockingStage <= Inventory.Instance.completion.Value.level + 1;
//                 }
//
//                 return true;
//             }
//         }
//     }
// }