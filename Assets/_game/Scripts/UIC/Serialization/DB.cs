using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace RomenoCompany
{
    public class DB : StrictSingleton<DB>
    {
        // public TableAbilities abilities;
        // public TableSuperpowers superpowers;
        // public TableEquipment equipment;
        // public TableTalents talents;
        // public TableLocations locations;
        // public TableEnergy energy;
        // public TableEnemies enemies;
        // public TableSkins skins;
        // public TableGamePlus gamePlus;
        // public TableBoosters boosters;
        // public SharedGameData sharedGameData;
        // public TableProgression progression;
        // public TableLaboratory laboratory;
        // public TableQuests quests;

        public TablePlayerItems gameItems;
        public TableCompanions companions;
        public TableLawyerAdvices lawyerAdvices;
        public TableImages images;
        public TableVideos videos;
        public SharedGameData shared;
        public TMPExtraSettings tmpSettings;

        protected override void Setup()
        {
            // PlayerPrefsData<int> indexGroup = new PlayerPrefsData<int>("AB_GROUP", -1);
            // Debug.Log("AB_GROUP : " + indexGroup.Value);
            // if(indexGroup.Value != -1 && indexGroup.Value < A_B_Groups.Length)
            // {
            //     Group = indexGroup.Value;
            // }
            DontDestroyOnLoad(gameObject);
        }
    }
}
