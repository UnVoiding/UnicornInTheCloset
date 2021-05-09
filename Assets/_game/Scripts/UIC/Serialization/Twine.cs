using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
                    p.RemoveLinksFromText();
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
        public string parsedText;
        public List<PassageLink> links;
        public List<string> tags;

        public PassageType type;
        public string imageKey; // for t:imgp passages
        public int adviceId; // t:f passages
        public List<ISFCondition> conditions;
        public List<SFStatement> effects;
        public float waitTimeBeforeExec = 1.0f;
        public float waitTimeAfterExec = 1.0f;
        public bool parsed;

        [NonSerialized]
        public List<Passage> passageLinks;

        public string ParsedText
        {
            get
            {
                return parsedText.Replace("<Name>", Inventory.Instance.playerState.Value.name);
            }
        }

        public Passage()
        {
            passageLinks = new List<Passage>();
            conditions = new List<ISFCondition>();
            effects = new List<SFStatement>();
        }

        public void RemoveLinksFromText()
        {
            int separatorPos = text.IndexOf("---");
            if (separatorPos != -1)
            {
                parsedText = text.Substring(0 , separatorPos).Trim();
            }
            else
            {
                parsedText = text.Trim();
            }
        }

        public void RegenerateLinks(TwineRoot root)
        {
            if (links != null)
            {
                foreach (var plink in links)
                {
                    if (!plink.broken)
                    {
                        var linkedPassage = root.Find(plink.pid);
                        if (linkedPassage != null)
                        {
                            passageLinks.Add(linkedPassage);
                        }
                        else
                        {
                            Debug.LogError($"Passage: Failed to find passage with pid {plink.pid}");
                        }
                    }
                    else
                    {
                        Debug.LogError($"Passage: Found broken link of passage {plink.pid}");
                    }
                }
            }
        }

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
                if (!conditions[i].Check()) return false;
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

        public SFStatement GetStatement(SFStatement.Type type)
        {
            for (int i = 0; i < effects.Count; i++)
            {
                if (effects[i].type == type) return effects[i];
            }

            return null;
        }
    }

    [Serializable]
    public class PassageLink
    {
        public int pid;
        public bool broken;
    }
}