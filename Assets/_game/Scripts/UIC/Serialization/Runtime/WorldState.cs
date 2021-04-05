using System;
using System.Collections.Generic;

namespace RomenoCompany
{
    [Serializable]
    public class WorldState
    {
        public Dictionary<string, Variable> variables;
        public bool companionJsonsLoaded;
        public List<CompanionState> companionStates;
        public List<AdviceState> unicornAdviceStates;
        public List<PlayerItemState> gameItemStates;
        public int lastCompanion = 0;
        public bool lawyerFinished = false;

        public WorldState()
        {
            variables = new Dictionary<string, Variable>();
            companionStates = new List<CompanionState>();
            unicornAdviceStates = new List<AdviceState>();
            gameItemStates = new List<PlayerItemState>();
            companionJsonsLoaded = false;

            foreach (var cd in DB.Instance.companions.items)
            {
                companionStates.Add(new CompanionState(cd));
            }

            foreach (var gameItemData in DB.Instance.gameItems.items)
            {
                gameItemStates.Add(new PlayerItemState(gameItemData));
            }
        }
        
        public static WorldState CreateDefault()
        {
            return new WorldState();
        }
    }

    public enum VariableType
    {
        INT = 0,
        STRING = 1,
    }

    public class Variable
    {
        public string name;
        public VariableType type;
        public string value;
    }
}