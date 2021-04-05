using System;
using System.Collections.Generic;

namespace RomenoCompany
{
    [Serializable]
    public class CompanionState
    {
        public CompanionData data;
        public bool locked;
        public int activeDialogue = 0;
        public int activePassageId = 0;
        public int currentEmotion = 0;
        public List<Dialogue> dialogues;

        public CompanionState()
        {
            
        }

        public CompanionState(CompanionData data)
        {
            this.data = data;
            locked = !data.openByDefault;
        }
    }

    public class Dialogue
    {
        public TwineRoot twineRoot;
    }
}
