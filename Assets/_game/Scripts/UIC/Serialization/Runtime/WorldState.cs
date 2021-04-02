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
        public List<AdviceState> adviceStates;
        public List<PlayerItemState> gameItemStates;
        public int lastCompanion;

        public WorldState()
        {
            variables = new Dictionary<string, Variable>();
            companionStates = new List<CompanionState>();
            adviceStates = new List<AdviceState>();
            gameItemStates = new List<PlayerItemState>();
            companionJsonsLoaded = false;
            lastCompanion = 0;
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