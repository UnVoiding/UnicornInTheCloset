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
                foreach (var compState in Inventory.Instance.worldState.Value.companionStates)
                {
                    foreach (var asset in compState.data.dialogueJsons)
                    {
                        TwineRoot root = JsonConvert.DeserializeObject<TwineRoot>(asset.text);
                    }
                }

                Inventory.Instance.worldState.Value.companionJsonsLoaded = true;
                Inventory.Instance.worldState.Save();
            }
        }
    }
}