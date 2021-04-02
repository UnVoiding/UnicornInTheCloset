using System;
using System.Collections.Generic;

namespace RomenoCompany
{
    [Serializable]
    public class CompanionState
    {
        public int companionId;
        public bool locked;
        public int activeDialogue;
        public int activePassageId;
        public int currentEmotion;
        public List<Dialogue> dialogues;

        public CompanionState(int companionId)
        {
            companionId = companionId;
            locked = true;
        }
    }

    public class Dialogue
    {
        public TwineRoot twineRoot;
    }
}
