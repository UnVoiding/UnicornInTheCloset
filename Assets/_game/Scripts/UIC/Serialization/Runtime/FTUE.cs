using System;
using System.Collections.Generic;
using UnityEngine;

namespace RomenoCompany
{
    // first time user experience
    [Serializable]
    public class FTUEState
    {
        [Serializable]
        public class FTUEElementState
        {
            public FTUEType ftueType;
            public bool passed = false;

            public FTUEElementState(FTUEType ftueType)
            {
                this.ftueType = ftueType;
            }
        }
        
        public List<FTUEElementState> ftueStates;
        
        public bool needShowCompanionSelection = false; 
        public bool needShowProfileUnicornAdvicesFtue = false; 
        public bool needShowProfileItemsFtue = false; 
        public bool needShowProfileLawyerAdvicesFtue = false; 
        public bool needShowUnlockedCompanionsFtue = false; // UNUSED
        public bool needShowChatScreenChooseAnswerFtue = true; 

        public FTUEState()
        {
            ftueStates = new List<FTUEElementState>(10);
            ftueStates.Add(new FTUEElementState(FTUEType.COMPANION_SELECTION1));
            ftueStates.Add(new FTUEElementState(FTUEType.COMPANION_SELECTION2));
            ftueStates.Add(new FTUEElementState(FTUEType.PROFILE_SCREEN));
            ftueStates.Add(new FTUEElementState(FTUEType.PROFILE_SCREEN_ADVICES));
            ftueStates.Add(new FTUEElementState(FTUEType.PROFILE_SCREEN_LAWYER_ADVICES));
            ftueStates.Add(new FTUEElementState(FTUEType.PROFILE_SCREEN_ITEMS));
            ftueStates.Add(new FTUEElementState(FTUEType.PROFILE_SCREEN_ITEM_INFO));
            ftueStates.Add(new FTUEElementState(FTUEType.UNLOCKED_COMPANIONS));
            ftueStates.Add(new FTUEElementState(FTUEType.CHAT_SCREEN_CHOOSE_ANSWER));
            ftueStates.Add(new FTUEElementState(FTUEType.COMPANION_SELECTION_INFO_TAB));
        }

        public static FTUEState CreateDefault()
        {
            return new FTUEState();
        }

        public void SetFTUE(FTUEType type, bool value)
        {
            var ftueElState = ftueStates.Find((f) => f.ftueType == type);
            if (ftueElState != null)
            {
                ftueElState.passed = value;
            }
            else
            {
                Debug.LogError($"FTUEState FTUEType {type} not found");
            }
        }

        public bool GetFTUE(FTUEType type)
        {
            var ftueElState = ftueStates.Find((f) => f.ftueType == type);
            if (ftueElState != null)
            {
                return ftueElState.passed;
            }
            else
            {
                Debug.LogError($"FTUEState FTUEType {type} not found");
                return false;
            }
        }

        public void SetAllPassed()
        {
            for (int i = 0; i < ftueStates.Count; i++)
            {
                ftueStates[i].passed = true;
            }
        }
    }
}


