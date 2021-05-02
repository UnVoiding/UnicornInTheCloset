using System;
using System.Collections.Generic;

namespace RomenoCompany
{
    [Serializable]
    public class SFDialogue
    {
        [NonSerialized]
        public TwineRoot root;
        // list of passage ids - a path in the twine graph
        public List<int> path;

        public SFDialogue()
        {
            path = new List<int>();
        }
    }

    // [Serializable]
    // public class SFDialogueEntry
    // {
    //     public SFCommand command;
    //     public SFCommand next;
    //     public SFCommand previous;
    // }
}