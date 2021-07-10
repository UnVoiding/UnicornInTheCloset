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
        private List<Passage> tempNextAvailablePassages;

        public SFDialogue()
        {
            path = new List<int>();
            tempNextAvailablePassages = new List<Passage>();
        }

        public bool IsFinished()
        {
            if (path.Count != 0)
            {
                var currentPassage = root.Find(path[path.Count - 1]);

                tempNextAvailablePassages.Clear();
                currentPassage.GetNextAvailablePassages(ref tempNextAvailablePassages);

                return tempNextAvailablePassages.Count == 0;
            }

            return false;
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