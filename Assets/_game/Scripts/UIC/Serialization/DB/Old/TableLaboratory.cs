// using System;
// using UnityEngine;
// using System.Collections.Generic;
// using System.Linq;
//
// namespace RomenoCompany
// {
// 	[CreateAssetMenu(fileName = "TableLaboratory", menuName = "ScriptableObjects/TableLaboratory", order = 59)]
// 	public class TableLaboratory : Table<LaboratoryItemData.ItemID, LaboratoryItemData>
// 	{
// 		[Sirenix.Serialization.OdinSerialize, System.NonSerialized] public List<LaboratoryGroup> itemGroups = new List<LaboratoryGroup>();
// 		public int keysToUnlock = 3;
// 		public int levelToUnlock = 5;
// 		public bool enableManualKeys = false;
//
// 		public AnimationCurve transitionImageScaleUpCurve;
// 		public float transitionDuration = 0.5f;
//
// 		public LaboratoryItemData.ItemID GetFirstUnlockedLaboratoryItem()
// 		{
// 			return itemGroups[0].items.First();
// 		}
//
//         public int GetMaxLaboratoryLevel()
//         {
//             return Items.Sum(x => (x.hide ? 0 : x.progression.Count - 1));
//         }
//
//         public LaboratoryItemProgression GetCurrentLevelItemData(LaboratoryItemData.ItemID itemId)
//         {
// 	        var dbItem = GetItem(itemId);
// 	        int level = Inventory.Instance.laboratoryStatus.Value.GetItem(itemId)._level;
//
// 		    return dbItem.progression[level];
//         }
// 	}
//
// 	[System.Serializable]
// 	public class LaboratoryGroup
// 	{
// 		[Sirenix.Serialization.OdinSerialize, NonSerialized] public HashSet<LaboratoryItemData.ItemID> items;
// 		[Sirenix.Serialization.OdinSerialize, NonSerialized] public Dictionary<LaboratoryItemData.ItemID, int> terms;
// 	}
// }