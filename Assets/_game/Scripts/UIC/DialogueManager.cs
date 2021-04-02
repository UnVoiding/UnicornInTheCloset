using System.Security.AccessControl;
using UnityEngine;
using Sirenix.OdinInspector;
using Newtonsoft.Json;

namespace RomenoCompany
{
    public class DialogueManager : StrictSingleton<DialogueManager>
    {
        protected override void Setup()
        {
            if (!Inventory.Instance.worldState.Value.companionJsonsLoaded)
            {
                foreach (var dbCompanion in DB.Instance.companions.items)
                {
                    CompanionState companionState = new CompanionState(dbCompanion.id);
                    
                    Inventory.Instance.worldState.Value.companionStates.Add(companionState);

                    foreach (var asset in dbCompanion.dialogueJsons)
                    {
                        TwineRoot root = JsonConvert.DeserializeObject<TwineRoot>(asset.text);
                    }
                }

                Inventory.Instance.worldState.Value.companionJsonsLoaded = true;
            }
        }
    }
}