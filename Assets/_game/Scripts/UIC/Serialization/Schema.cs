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
    
    [Serializable]
    public partial class SkinData : ICoreParam<SkinData.ItemID>
    {
        [VerticalGroup("row/right"), LabelWidth(60)] public ItemID itemId;
        public ItemID Key => itemId;
        public enum ItemID
        {
            DEBRIS = 1,
            GAUNTLET = 2,
            LASER_EYES = 3,
            DEEP_FREEZE = 4,
            TORNADO = 5,
            TSUNAMI = 8,
            SPACE_DUNK = 10,
            BLINK = 24,
            
            VIP_HAND_OF_GOD = 400,
            VIP_DEEP_FREEZE = 401,
            VIP_TORNADO = 402,
            
            NONE = 777
        }

        [HorizontalGroup("row", 50), PreviewField(50, ObjectFieldAlignment.Left), HideLabel]
        public Sprite Icon = null;
        
        [VerticalGroup("row/right")] public SkinRarityData.Rarity rarity;
        [VerticalGroup("row/right")] public Mesh mesh;
        
        [HorizontalGroup("arrays", LabelWidth = 60), PropertySpace(SpaceBefore = 5)]
        public Material[] materials;
        [HorizontalGroup("arrays"), PropertySpace(SpaceBefore = 5)]
        public Material[] previewMaterials;
        
        private bool showLevelToUnlock => levelToUnlock >= 1;
        [ShowIf("showLevelToUnlock")]
        public int levelToUnlock;
        
        public bool unlockedForAds => adsToUnlock > 0;
        [ShowIf("unlockedForAds")] public int adsToUnlock;
        [HideIf("unlockedForAds"), HideIf("inApp")] public int coinsToUnlock;
        [ShowIf("showAfterLevelCompletion")] public bool showAfterLevelCompletion;
        
        [ShowIf("anyAttachedObject")] public List<GameObject> attachedObjects = null;
        [ShowIf("anyPreviewAttachedObject")] public List<GameObject> previewAttachedObjects = null;
        
        [ShowIf("inApp")] public bool inApp;

        private bool anyAttachedObject => (attachedObjects != null && attachedObjects.Count>0);
        private bool anyPreviewAttachedObject => (previewAttachedObjects!=null && previewAttachedObjects.Count>0);
    }

    [Serializable]
    public class SkinRarityData
    {
        public enum Rarity
        {
            COMMON = 0,
            RARE = 10,
            EPIC = 20,
            VIP = 30
        }
        
        public Rarity rarity;
        public string rarityString;
        public string rarityStringOnSkinsScreen;
        public Color selectedTabTitleColor;
        public Color deselectedTabTitleColor;
        [FormerlySerializedAs("rarityColor")] public Color rarityStringColor;
        public Color rarityFrameColor;
        public bool includedInChinese = true;
        
        // public FXType rarityVfx;
    }

    [Serializable]
    public partial class AbilityData : ICoreParam<AbilityData.ItemID>
    {
        public enum ItemID
        {
            DEBRIS = 1,
            GAUNTLET = 2,
            LASER_EYES = 3,
            DEEP_FREEZE = 4,
            TORNADO = 5,
            TSUNAMI = 8,
            SPACE_DUNK = 10,
            BLINK = 24,
            
            VIP_HAND_OF_GOD = 400,
            VIP_DEEP_FREEZE = 401,
            VIP_TORNADO = 402,
            
            NONE = 777
        }
        
        [HorizontalGroup("row", 50), PreviewField(50, ObjectFieldAlignment.Left), HideLabel]
        public Sprite Icon = null;
        
        [VerticalGroup("row/right"), LabelWidth(60)] public ItemID itemId;
        [VerticalGroup("row/right")] public string Title = null;
        [VerticalGroup("row/right")] public ScriptableObject so;
        [VerticalGroup("row/right"), ShowIf("hasAddSound")] [ReadOnly] public string soundName;
        [VerticalGroup("row/right"), ShowIf("showLevelToUnlock")] public int levelToUnlock;
        [VerticalGroup("row/right"), ShowIf("unlockedForAds")] public int adsToUnlock;
        [VerticalGroup("row/right"), HideIf("unlockedForAds"), HideIf("inapp")] public int coinsToUnlock;
        // [VerticalGroup("row/right")] public ScriptableObject so;
        
        public ItemID Key => itemId;

        [ShowIf("swings")] public bool swings;
        [ShowIf("inapp")] public bool inapp;
        [ShowIf("isBooster")] public bool isBooster;
        
        [Multiline(3), HideIf("isBooster")] public string Description = null;
        
        public bool hasAddSound => !string.IsNullOrEmpty(soundName);

        private bool showLevelToUnlock => levelToUnlock >= 1;
        
        public bool unlockedForAds => adsToUnlock > 0;
        
    }
    
    [Serializable]
    public partial class QuestData : ICoreParam<QuestData.ItemID>
    {
        [VerticalGroup("row/right"), LabelWidth(60)] public ItemID itemId;
        public ItemID Key => itemId;
        public enum ItemID
        {
            DEBRIS = 1,
            GAUNTLET = 2,
            LASER_EYES = 3,
            DEEP_FREEZE = 4,
            TORNADO = 5,
            TSUNAMI = 8,
            SPACE_DUNK = 10,
            BLINK = 24,
            
            VIP_HAND_OF_GOD = 400,
            VIP_DEEP_FREEZE = 401,
            VIP_TORNADO = 402,
            
            NONE = 777
        }
        
        [HorizontalGroup("row", 50), PreviewField(50, ObjectFieldAlignment.Left), HideLabel]
        public Sprite Icon = null;

        [VerticalGroup("row/right")] public string rewardID;
        [VerticalGroup("row/right")] public Placement placement;
        [VerticalGroup("row/right")] public Position position;
        [VerticalGroup("row/right")] public Bubble bubble;
        [VerticalGroup("row/right")] public int level;
        [VerticalGroup("row/right"), ShowIf("hasCoinReward")] public int coinReward;
        [VerticalGroup("row/right"), ShowIf("hasGemReward")] public int gemReward;
        [Multiline(3)] public string Description = null;
        
        public bool hasCoinReward => coinReward > 0;
        public bool hasGemReward => gemReward > 0;
        
        public enum Placement
        {
            QUEST_BEGINS,
            INGAME,
            LEVEL_COMPLETE,
            REWARD,
            CUSTOM
        }
        
        public enum Bubble
        {
            SQUARE_3,
            SQUARE_2,
            SQUARE_1,
            SKEWED_3,
            SKEWED_2,
            SKEWED_1
        }
        
        public enum Position
        {
            MENU_POS,
            INGAME_POS
        }
    }

    [Serializable]
    public class SuperpowerData : BaseData, ICoreParam<SuperpowerData.ItemID>
    {
        // IMPORTANT:
        // As ItemID is converted to string and is used to send GameAnalytics Design event
        // it SHOULD NOT exceed 16 symbols in length otherwise it will be truncated
        // E.g. this may lead to VIP_SUPERSONIC_PUNCH_1 and VIP_SUPERSONIC_PUNCH_2 be both
        // reported as VIP_SUPERSONIC_P
        public enum ItemID
        {
            DEBRIS = 1,
            GAUNTLET = 2,
            LASER_EYES = 3,
            DEEP_FREEZE = 4,
            TORNADO = 5,
            TSUNAMI = 8,
            SPACE_DUNK = 10,
            BLINK = 24,
            
            VIP_HAND_OF_GOD = 400,
            VIP_DEEP_FREEZE = 401,
            VIP_TORNADO = 402,
            
            NONE = 777
        }

        [Serializable]
        public class SuperpowerProgression
        {
            public int level = 0;
            public bool random = false;
            [HideIf("random")] public ItemID itemID = ItemID.NONE;
            public bool openedForAds = false;
        }

        public ItemID itemId; public ItemID Key => itemId;
        public bool inappPurchase = false;
        public Sprite unlockPreview = null;
        public Sprite inventoryPreview = null;
    }
    
    [Serializable]
    public class BoosterData : BaseData, ICoreParam<BoosterData.ItemID>
    {
        public ItemID itemId;
        public ItemID Key => itemId;
        public enum ItemID
        {
            DAMAGE_ENEMIES = 0,
            SUPERPOWER_FILL = 10,
            REWARD = 20,
            SHIELD = 30,
            ABILITY = 40,
            NONE = 1000,
        }

        public List<float> progression;
        public List<float> fakeProgression;
    }
    
    [Serializable]
    public class TalentData : BaseData, ICoreParam<TalentData.ItemID>
    {
        public enum ItemID : int
        {
            HEALTH = 0, DAMAGE, ATTACK_SPEED
        }

        public ItemID itemId; public ItemID Key => itemId;
        public string modifierUI = "+[[value]]%";
        public int indexInUI = 0;
        public bool availableInDraft = true;
        [HorizontalGroup(400.0f)] [TableList] public TalentProgression[] progression = null;
        [HorizontalGroup(200.0f)] public int[] upgradePrices = null;
        public int cooldownForAds = 0;

        public int GetGamePlusLevel(int currentLevel)
        {
            return currentLevel - (Mathf.Min(progression.Length, upgradePrices.Length) - 1);
        }

        [Serializable]
        public struct TalentProgression
        {
            public float value;
            [TableColumnWidth(100, resizable: false)] public bool blocked;
            [ShowIf("blocked")] public int blockingStage;
        }
    }

    [Serializable]
    public class EnemiesData : ICoreParam<EnemiesData.ItemID>
    {
        public enum ItemID
        {
            Fighter = 0, 
            Archer = 1, 
            BigFighter = 2, 
            MiddleFighter = 3, 
            GrenadeThrower1 = 4, 
            GrenadeThrower3 = 5, 
            Ninja = 6, 
            ArmoredThug = 7,
            RocketThug1 = 8, 
            RocketThug3 = 9, 
            SuperNinja = 10, 
            CrusherThug = 11, 
            NinjaArmed = 12, 
            Gangster = 13, 
            Biker = 14,
            Roller = 15,
            PunkMolotov = 16,
            ArmedBandit = 17,
            ShieldBandit = 18,
            BossBanditLeader = 100,
            BossSharkHead = 101,
            None = 1000,
        }

        [SerializeField] ItemID id = ItemID.Fighter;
        public ItemID Key => id;

        [Serializable]
        public class HPRangeSetting
        {
            [TableColumnWidth(100,false)] public Vector2 hpRange = Vector2.zero;
            [ListDrawerSettings(Expanded = true)] public SkinData[] defaultSkins;
            public OverrideSkinData[] overrideSkins;
            [ListDrawerSettings(Expanded = true)] public SkinData[] goldenSkins;
            [ListDrawerSettings(Expanded = true)] public SkinData[] hardSkins;
        }

        [Serializable]
        public class HardLvlParameterMult
        {
            public ParametrType type = ParametrType.None;
            [MinValue(0.0f)] public float value = 1.0f;
        }

        public enum SkinPart { Body, Weapon, Hat, Hair, Backpack, HandLeft, HandRight, Belt }

        [Serializable]
        public class SkinData
        {
            public SkinContainer[] containers;
        }
        [Serializable]
        public class OverrideSkinData
        {
            public SkinData[] skins;
            public LocationData.ItemID location;
        }

        [Serializable]
        public class SkinContainer
        {
            [HorizontalGroup("cont", 0.15f, LabelWidth=60)]
            [VerticalGroup("cont/l")]
            public SkinPart part;
            [VerticalGroup("cont/m")]
            public Mesh mesh;
            [VerticalGroup("cont/r")]
            public Material material;
            public List<string> bones;

            public bool changeTransform = false; 
            [ShowIf("changeTransform")]
            public Vector3 offset = Vector3.zero;
            [ShowIf("changeTransform")]
            public Vector3 additionalRotation = Vector3.zero;
            [ShowIf("changeTransform")]
            public Vector3 scale = Vector3.one;
        }

        [Serializable]
        public class TalentPowerProgression
        {
            [HorizontalGroup(LabelWidth =70)] public int level;
            [HorizontalGroup()] public Vector2 forceRange;
        }

        [Serializable]
        public class SoundsContainer
        {
            public string hitSound;
            public string deathSound;
        }

        public WorldUIKey hpBarType = WorldUIKey.EnemyBar;
        public GameObject Prefab;
        public SoundsContainer sounds;
        [Header("Tossable hit force & bone")]
        [HorizontalGroup("force", LabelWidth = 60)] public float force;

        [Space(10)]
        [TableList] public HardLvlParameterMult[] hardLevelParameters = null;
        [TableList] public HardLvlParameterMult[] hardEnemyParameters = null;
    }

    [Serializable]
    public class LocationData : BaseData, ICoreParam<LocationData.ItemID>
    {
        public enum ItemID : int { LOCATION_1 = 0, LOCATION_2, LOCATION_3, LOCATION_4, LOCATION_5, BONUS_LOCATION_0 }

        public ItemID itemId; public ItemID Key => itemId;

        [Serializable]
        public class CelebrationVfx
        {
            public Vector3 offset = Vector3.zero;
            public Vector3 rotation = Vector3.zero;
        }
        
        [Header("Visuals")]
        [SerializeField] public GameObject world = null;
        [SerializeField] public float[] positionsCharacter = new float[] { 5, 10, 20, 40 };
        [SerializeField] public GameObject announcer = null;
        [SerializeField] public CelebrationVfx leftCelebrationVfx = null;
        [SerializeField] public CelebrationVfx rightCelebrationVfx = null;
        [SerializeField] public float celebrationVfxTime = 0.0f;
    
        [Header("DirectionalButton")]
        [SerializeField] public Color normalTint = Color.white;
        [SerializeField] public Color darkenedTint = Color.white;

        [Header("fog")]
        [SerializeField] public Color fog = Color.white;
        [SerializeField] public float forStart = -1;
        [SerializeField] public float fogEnd = 10;
        [Space]
        [SerializeField] public AbilityData.ItemID[] abilityPool = null;
        [SerializeField] public VideoClip videoClip = null;
        [SerializeField] public BonusForCompletion bonusForCompletion = new BonusForCompletion(10000, "10K");

        [Header("Cutscene Enemies")]
        public List<EnemiesData.ItemID> availableCSEnemies;

        [Serializable]
        public struct BonusForCompletion
        {
            public int value;
            public string displayValue;

            public BonusForCompletion(int value, string displayValue)
            {
                this.value = value;
                this.displayValue = displayValue;
            }
        }

        public float thirdArcadeMultiplier = 1.5f;
    }

    [Serializable]
    public class DamageData : BaseData, ICoreParam<DamageData.ItemID>
    {
        public enum ItemID : int
        {
            PUNCH_0 = 0,
            PUNCH_1 = 1,
            PUNCH_2 = 2,
            PUNCH_3 = 3,
            PUNCH_4 = 4,
            PUNCH_5 = 5,
            TOSS = 6,
            PUNCH_NONE = 99,
        }
        public ItemID itemId; public ItemID Key => itemId;

        public Vector2 damage;
        public Vector2 criticalDamage;
        public float criticalChance;
    }

    [Serializable]
    public class EquipmentData : BaseData, ICoreParam<EquipmentData.ItemID>
    {
        public enum ItemID : int { SOME_EQUIPMENT_1, SOME_EQUIPMENT_2 }

        public ItemID itemId; public ItemID Key => itemId;
    }
    
    [Serializable]
    public class LaboratoryItemData : BaseData, ICoreParam<LaboratoryItemData.ItemID>
    {
        [Serializable]
        public enum ItemID
        {
            HEARTS,
            BERSERK,
            TOSSABLES,
            WEAPONS,
            
            NONE = 777
        }

        public bool hide = false;
        public ItemID itemId; public ItemID Key => itemId;
        public VideoClip demonstrationImage;
        public List<LaboratoryItemProgression> progression;
    }
    
    [Serializable]
    public class LaboratoryItemProgression
    {
        public string description_key;
        public string description_value;
        public string nextLevelDescr_key;
        public string nextLevelDescr_value;
        public Sprite icon;
        public int tokensToUnlock;
        public float value;
        public float multiplication;
    }

    public class Table<TKey, TItem> : SerializedScriptableObject where TItem : ICoreParam<TKey>
    {
        [SerializeField] protected List<TItem> items = null;
        public List<TItem> Items => items;

        Dictionary<TKey, TItem> itemsDict = null;

        public TItem GetItem(TKey k)
        {
            if (itemsDict == null)
            {
                itemsDict = new Dictionary<TKey, TItem>();
                foreach (var i in items)
                {
                    itemsDict[i.Key] = i;
                }
            }
            return itemsDict[k];
        }
    }

    [Serializable]
    public class LocationGamePlus : ICoreParam<LocationData.ItemID>
    {
        [SerializeField]
        private LocationData.ItemID id;
        public LocationData.ItemID Key => id;
        [SerializeField]
        public List<EnemyModificator> enemyModificator = new List<EnemyModificator>();
    }

    [Serializable]
    public class EnemyModificator
    {
        public EnemiesData.ItemID enemy;
        public List<ParametrModificator> parametrModificators = new List<ParametrModificator>();
    }

    [Serializable]
    public class ParametrModificator
    {
        [HorizontalGroup(LabelWidth = 80)] public ParametrType parametr;
        [HorizontalGroup] public float value;
        [HorizontalGroup] public float step;
    }
}
