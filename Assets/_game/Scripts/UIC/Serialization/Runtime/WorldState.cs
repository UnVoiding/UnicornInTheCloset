using System;
using System.Collections.Generic;
using UnityEngine;

namespace RomenoCompany
{
    [Serializable]
    public class WorldState
    {
        public Dictionary<string, Variable> variables;
        public bool companionJsonsLoaded;
        public List<CompanionState> companionStates;
        public UnicornAdvicesState unicornAdvicesState;
        public List<PlayerItemState> gameItemStates;
        public int lastCompanion = 0;
        public bool lawyerFinished = false;

        public WorldState()
        {
            variables = new Dictionary<string, Variable>();
            companionStates = new List<CompanionState>();
            unicornAdvicesState = new UnicornAdvicesState();
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
    }

    [Serializable]
    public class UnicornAdvicesState
    {
        public List<AdviceState> unicornAdviceStates;
        public int lastAdvice;

        public UnicornAdvicesState()
        {
            unicornAdviceStates = new List<AdviceState>(20);
            lastAdvice = 1000;
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