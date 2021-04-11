using System;
using System.Collections.Generic;

namespace RomenoCompany
{
    [Serializable]
    public class SFDialogue
    {
        public TwineRoot root;
        // list of passage ids - a path in the twine graph
        public List<int> path;

        public SFDialogue(TwineRoot root)
        {
            root = root;
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