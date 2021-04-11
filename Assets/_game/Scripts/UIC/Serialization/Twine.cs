using System;
using System.Collections.Generic;
using UnityEditor;

namespace RomenoCompany
{
    [Serializable]
    public class TwineRoot
    {
        public List<Passage> passages;
        
        public void PostDeserialize()
        {
            for (int i = 0; i < passages.Count; i++)
            {
                var p = passages[i];
                for (int j = 0; j < p.tags.Count; j++)
                {
                    p.tags[j] = p.tags[j].Trim();
                }
            }
        }

        public Passage Find(int pid)
        {
            for (int i = 0; i < passages.Count; i++)
            {
                if (passages[i].pid == pid) return passages[i];
            }

            return null;
        }
        
        public Passage FindPassageWithTag(string tag)
        {
            for (int i = 0; i < passages.Count; i++)
            {
                var p = passages[i];
                for (int j = 0; j < p.tags.Count; j++)
                {
                    if (p.tags[j] == tag)
                    {
                        return p;
                    }
                }
            }

            return null;
        }
        
        public int CountPassagesWithTag(string tag)
        {
            int count = 0;

            for (int i = 0; i < passages.Count; i++)
            {
                var p = passages[i];
                for (int j = 0; j < p.tags.Count; j++)
                {
                    if (p.tags[j] == tag)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        public Passage startPassage;
    }

    [Serializable]
    public class Passage
    {
        public enum PassageType
        {
            COMPANION_MESSAGE = 0,
            COMPANION_IMAGE = 10,
            HERO_MESSAGE = 20,
            ADVICE = 30,
        }
        
        public int pid;
        public string name;
        public string text;
        public List<PassageLink> links;
        public List<string> tags;

        [NonSerialized]
        public PassageType type;
        [NonSerialized]
        public List<Passage> passageLinks;
        [NonSerialized]
        public bool parsed;
        [NonSerialized]
        public string imageKey;
        [NonSerialized]
        public List<SFCondition> conditions;
        [NonSerialized]
        public List<SFStatement> effects;

        [NonSerialized]
        public float waitTimeBeforeExec = 1.0f;
        [NonSerialized]
        public float waitTimeAfterExec = 1.0f;

        public int CountNextAvailablePassages()
        {
            int result = 0;
            for (int i = 0; i < passageLinks.Count; i++)
            {
                var p = passageLinks[i];
                if (p.ConditionsAreMet()) result++;
            }

            return result;
        }

        public bool ConditionsAreMet()
        {
            var worldState = Inventory.Instance.worldState.Value;
            for (int i = 0; i < conditions.Count; i++)
            {
                if (!worldState.CheckCondition(conditions[i])) return false;
            }

            return true;
        }

        public void GetNextAvailablePassages(ref List<Passage> passages)
        {
            for (int i = 0; i < passageLinks.Count; i++)
            {
                var p = passageLinks[i];
                if (p.ConditionsAreMet())
                {
                    passages.Add(p);
                }
            }
        }
    }

    [Serializable]
    public class PassageLink
    {
        public int pid;
    }
}