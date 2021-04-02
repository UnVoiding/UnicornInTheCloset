using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection.Emit;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Video;

namespace RomenoCompany
{
    public interface ICoreParam<TKey> { TKey Key { get; } }

    [Serializable]
    public abstract class BaseData
    {
        [SerializeField] public string title = null;
        [SerializeField] public Sprite icon = null;
        [SerializeField] public Sprite icon_Inactive = null;
        [SerializeField] public string description = null;

        public string Title => string.IsNullOrEmpty(title) ? "[Title not filled]" : title;
        public Sprite Icon => icon == null ? Resources.Load<Sprite>("UI/icon_not_found") : icon;
        public Sprite Icon_Inactive => icon_Inactive == null ? Resources.Load<Sprite>("UI/icon_not_found") : icon_Inactive;
        public string Description => string.IsNullOrEmpty(description) ? "[Description not filled]" : description;

        public ScriptableObject so;
    }
    
    // [Serializable]
    // public partial class SkinData : ICoreParam<SkinData.ItemID>
    // {
    //     [VerticalGroup("row/right"), LabelWidth(60)] public ItemID itemId;
    //     public ItemID Key => itemId;
    //     public enum ItemID
    //     {
    //         DEBRIS = 1,
    //         GAUNTLET = 2,
    //         LASER_EYES = 3,
    //         DEEP_FREEZE = 4,
    //         TORNADO = 5,
    //         TSUNAMI = 8,
    //         SPACE_DUNK = 10,
    //         BLINK = 24,
    //         
    //         VIP_HAND_OF_GOD = 400,
    //         VIP_DEEP_FREEZE = 401,
    //         VIP_TORNADO = 402,
    //         
    //         NONE = 777
    //     }
    //
    //     [HorizontalGroup("row", 50), PreviewField(50, ObjectFieldAlignment.Left), HideLabel]
    //     public Sprite Icon = null;
    //     
    //     [VerticalGroup("row/right")] public SkinRarityData.Rarity rarity;
    //     [VerticalGroup("row/right")] public Mesh mesh;
    //     
    //     [HorizontalGroup("arrays", LabelWidth = 60), PropertySpace(SpaceBefore = 5)]
    //     public Material[] materials;
    //     [HorizontalGroup("arrays"), PropertySpace(SpaceBefore = 5)]
    //     public Material[] previewMaterials;
    //     
    //     private bool showLevelToUnlock => levelToUnlock >= 1;
    //     [ShowIf("showLevelToUnlock")]
    //     public int levelToUnlock;
    //     
    //     public bool unlockedForAds => adsToUnlock > 0;
    //     [ShowIf("unlockedForAds")] public int adsToUnlock;
    //     [HideIf("unlockedForAds"), HideIf("inApp")] public int coinsToUnlock;
    //     [ShowIf("showAfterLevelCompletion")] public bool showAfterLevelCompletion;
    //     
    //     [ShowIf("anyAttachedObject")] public List<GameObject> attachedObjects = null;
    //     [ShowIf("anyPreviewAttachedObject")] public List<GameObject> previewAttachedObjects = null;
    //     
    //     [ShowIf("inApp")] public bool inApp;
    //
    //     private bool anyAttachedObject => (attachedObjects != null && attachedObjects.Count>0);
    //     private bool anyPreviewAttachedObject => (previewAttachedObjects!=null && previewAttachedObjects.Count>0);
    // }
}
