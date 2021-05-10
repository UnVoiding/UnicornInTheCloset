using System;
using System.Collections.Generic;
using UnityEngine;

namespace RomenoCompany
{
    [Serializable]
    public class CompanionState
    {
        public CompanionData.ItemID id;
        public bool locked;
        public int activeDialogue = 0;
        public int currentEmotion = 0;
        public List<SFDialogue> dialogues;

        //// RUNTIME
        private CompanionData data;

        public CompanionData Data
        {
            get
            {
                if (data == null)
                {
                    data = DB.Instance.companions.items.Find((d) => d.id == id);
                }

                return data;
            }
        }

        public CompanionState(CompanionData data)
        {
            id = data.id;
            this.data = data;
            locked = !data.openByDefault;
            dialogues = new List<SFDialogue>();
            Debug.LogError($"------------ Companion name = {data.name} data.dialogueJsons.Count = {data.dialogueJsons.Count}");
            for (int i = 0; i < data.dialogueJsons.Count; i++)
            {
                dialogues.Add(new SFDialogue());
            }
        }
    }
}
