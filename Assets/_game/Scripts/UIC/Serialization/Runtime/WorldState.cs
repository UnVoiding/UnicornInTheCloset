using System;
using System.Collections.Generic;
using UnityEngine;

namespace RomenoCompany
{
    [Serializable]
    public class WorldState
    {
        public List<Variable> variables;
        public List<CompanionState> companionStates;
        public UnicornAdvicesState unicornAdvicesState;
        public bool advicesLoaded = false;
        public List<PlayerItemState> gameItemStates;
        public int lastCompanion = 0;
        public bool lawyerFinished = false;

        public WorldState()
        {
            variables = new List<Variable>();
            companionStates = new List<CompanionState>();
            unicornAdvicesState = new UnicornAdvicesState();
            gameItemStates = new List<PlayerItemState>();

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

        public Variable GetVariable(string variableName)
        {
            for (int i = 0; i < variables.Count; i++)
            {
                if (variables[i].name == variableName) return variables[i];
            }

            return null;
        }

        public PlayerItemState GetPlayerItem(PlayerItemData.ItemID id)
        {
            for (int i = 0; i < gameItemStates.Count; i++)
            {
                if (gameItemStates[i].id == id) return gameItemStates[i];
            }

            return null;
        }

        public PlayerItemState GetPlayerItem(string itemCode)
        {
            for (int i = 0; i < gameItemStates.Count; i++)
            {
                if (gameItemStates[i].Data.code == itemCode) return gameItemStates[i];
            }

            return null;
        }

        public CompanionState GetCompanion(CompanionData.ItemID id)
        {
            for (int i = 0; i < companionStates.Count; i++)
            {
                if (companionStates[i].id == id) return companionStates[i];
            }

            return null;
        }
        
        public CompanionState GetCompanion(string companionCode)
        {
            for (int i = 0; i < companionStates.Count; i++)
            {
                if (companionStates[i].Data.code == companionCode) return companionStates[i];
            }

            return null;
        }

    }

    [Serializable]
    public class UnicornAdvicesState
    {
        public List<AdviceState> unicornAdviceStates;

        public UnicornAdvicesState()
        {
            unicornAdviceStates = new List<AdviceState>(20);
        }

        public AdviceState GetAdviceById(int id)
        {
            for (int i = 0; i < unicornAdviceStates.Count; i++)
            {
                if (unicornAdviceStates[i].id == id) return unicornAdviceStates[i];
            }

            return null;
        }
    }

    public enum VariableType
    {
        INT = 0,
        STRING = 1,
    }

    public enum VariableOperation
    {
        SUM = 0,
        SUBTRACT = 1,
        UNKNOWN = 1000,
    }

    [Serializable]
    public class Variable
    {
        public string name;
        public VariableType type;
        public string value;
    }
}