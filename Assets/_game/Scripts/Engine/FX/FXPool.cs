using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace RomenoCompany
{
    public enum FXType : int
    {
        None = 0, ThugAttack = 1, Punch0 = 2, Punch1 = 3, Punch2 = 4, Punch3 = 4,
        DMGIncreseSelf = 5, ComboDMGSelf = 100, FinalPunchDMGSelf = 101, Vampirism = 102,
        ComboSpd = 103, FinalPunchStun = 11, CritChance = 12,
        CritDMG = 13, FireDMG = 14, AcidDMG = 15, HPIncrease = 16, Regeneration = 17,
        DirectForceWaveSelf = 18, ReverseForceWave = 19, FinalPunchPowerWave = 20, 
        Grenade = 23, GrenadeThugShoot = 6, GrenadeThugLanding = 7, 
        GrenadeThugExplosion = 8, GrenadeThugShootExplosion = 9,
        RocketThugShoot = 24, RocketThugLanding = 25,
        Stun, HeartTrigger, HeartClaim, VampirismUse, GunProjectile, BombExplosion, SwingPunch0, SwingPunch1, SwingPunch2, SwingPunch3, HeroLanding, LevelUP,
        VampirismSelfBody, AcidSelfBody, FinalPunchWaveBody, DirectForceWaveSelfBody, FinalPunchStunSelfBody, MineOffscrenExplosion, SuperHeroPoseWin,
        Dash, BackDash, OrbitalStrikeRay, FuryAdd, FuryUse, OrbitalStrikeAdd, DeadlyStrikeSelf, DeadlyStrike, FreezeStun, SelfDefensiveClone, SelfAttackClone, PunchNinja,
        SmokeSuperNinja, NinjaLanding, SwingNinja, ShieldPreview, AbsorbingShieldPreview, PunchSuperNinja, SwingSuperNinja, PunchNinjaArmed, FireSwing,
        FinalPunchSwing, FinalPunchPunch, FirePreview, VampirismSwing, StunStacks, MeteorExplosion, HandOfGod_ABGroup_0, ShieldExplosion, TalentsPillar, PunchBonus0, PunchBonus1, PunchBonus2, PunchBonus3,
        KillBonus, PunchCrusher, OrbitalStrikePreview, AttackClonePreview, ForceWavePreview, FinalPunchDamagePreview, VampirismPreview, FinalPunchStunPreview, HeroJump, GangsterLastAttack,
        GangsterSwing, SupersonicPunchPreview, SupersonicPunchAdd, SwingPunch1ComboDamage, SwingPunch2ComboDamage, SwingPunch3ComboDamage, Punch1ComboDamage, Punch2ComboDamage, Punch3ComboDamage, PreviewComboDamage, SelfComboDamage,
        BikeExplosion = 300, SpheresPreview,
        FootworkDash, FootworkBackDash, FootworkHands, FootworkLegs, FootworkPreviewHands, FootworkPreviewLegs, FootworkAddHands, FootworkAddLegs,
        RollerBodyDash, CelebrationLevel01, CelebrationLevel02, CashResult,
        DeepFreezeRay, DeepFreezeFrozen, DeepFreezeBlast, DeepFreezeKnockoff,
        CelebrationLevel03, CelebrationLevel04,
        MolotovExplosion, Tornado_Main, Tornado_Hands, Tornado_Eyes,
        SpaceDunk_Explosion = 110, SpaceDunk_Meteor = 111, LavaRiftBurst = 112, LavaRiftPunch = 113, LavaRiftAdd = 114, LavaRiftMenu = 115, LavaFire = 116,
        FootworkFireball = 120, RiotShield, RiotShieldHit, BossBanditSwing = 125, BossBanditDownHit = 126,
        FrostWaveMenuAdd = 127, FrostWaveIngameAdd = 128, FrostWaveFreeze = 129, FrostWaveMenuHands = 130, FrostWaveIngameHands = 131,
        Tsunamy_Eyes = 135, Tsunami_Puddles = 136, Tsunami_Flash = 137,
        Blink_Dash = 140, Blink_Blink = 141, LockOpen = 142,
        LightsOfMagic_Punch = 170, LightsOfMagic_Swing = 171, LightsOfMagic_Light = 172, LightsOfMagic_Projectile = 173, LightsOfMagic_Hands = 174, LightsOfMagic_Head = 175,
        SharkHeadStrike = 180, SharkHeadJumpWater = 181, SharkHeadTeeth = 182, SharkHeadJumpHit = 183,
        SkinRegular = 190,
        SkinRare = 191,
        SkinLegendary = 192,
        SkinVIP = 211,
        BoosterPickup = 193,
        SwingBerserkPunch0 = 194,
        SwingBerserkPunch1 = 195,
        SwingBerserkPunch2 = 196,
        SwingBerserkPunch3 = 197,
        DestroyWeapon1 = 198,
        DestroyWeapon2 = 199,
        DestroyWeapon3 = 200,
        HeroBaseLabButtonUpgradeItem = 201,
        HeroBaseLabCelebration = 202,
        AttackDroneScanner = 203,
        AttackDroneRayImpact = 204,
        HandOfGod_ABGroup_1 = 205,
        JumpLanding = 206,
        JumpPunch = 207,
        JumpInFlying = 208,
        LavaRiftBurst_ABGroup_0 = 210,
        Upgrade_DMG = 215, Upgrade_HP = 216, Upgrade_SPD = 217,
        VIP_SupersonicPunchAdd = 400, VIP_SupersonicPunchPreview = 401,
        VIP_LightsOfMagic_Punch = 402, VIP_LightsOfMagic_Swing = 403, VIP_LightsOfMagic_Light = 404, VIP_LightsOfMagic_Projectile = 405, VIP_LightsOfMagic_Hands = 406, VIP_LightsOfMagic_Head = 407, VIP_LightsOfMagic_Ingame_Hands = 408, VIP_LightsOfMagic_Ingame_Head = 409,
        VIP_DirectForceWave = 410, VIP_ForceWavePreview = 411,
        VIP_ComboBreakerSelf = 412, VIP_ComboBreakerPreview = 413,
        VIP_ComboBreakerPunch1 = 414, VIP_ComboBreakerPunch2 = 415, VIP_ComboBreakerPunch3 = 416,
        VIP_ComboBreakerSwing1 = 417, VIP_ComboBreakerSwing2 = 418, VIP_ComboBreakerSwing3 = 419,
        VIP_HandOfGod = 420, VIP_HandOfGod_Explosion = 430,
        VIP_DeepFreeze_Ray = 421, VIP_DeepFreeze_Blast = 422, VIP_DeepFreeze_Knockoff = 423, VIP_DeepFreeze_Frozen = 424,
        VIP_Warlord_Add = 425, VIP_Warlord_Preview = 426,
        VIP_Tornado_Main = 427, VIP_Tornado_Hands = 428, VIP_Tornado_Eyes = 429,
                
        VIP_SupersonicPunchPizzaAdd = 440,
        VIP_SupersonicPunchPizzaPreview = 441,

    }

    public enum FXOrientation
    {
        Z = 0, Y = 1, X = 2, MinusZ = 3, MinusY = 4, MinusX = 5,
    }

    public enum FXSpace
    {
        Local = 0, World = 1
    }

    public enum AttackFXType {None, Punch, Swing}

    [System.Serializable]
    public class PunchAttackFXContainer
    {
        public AttackFXType attackFXType = AttackFXType.None;
        public FXType fXType = FXType.None;
        public Vector3 fxOffset = Vector3.zero;
    }

    [System.Serializable]
    public class AttackFXContainer
    {
        public FXType fXType = FXType.None;
        public Vector3 fxOffset = Vector3.zero;
    }

    public class FXPool : StrictSingleton<FXPool>
    {
        [System.Serializable]
        public class Container
        {
            public FXType type;
            public GameObject gameObject;
            public int count;
        }

#pragma warning disable 414
        bool found = false; 
        [Space(10)]
        [Header("Search")]
        [SerializeField] [OnValueChanged("Search")] FXType searchField = FXType.None;
#pragma warning restore 414
        void Search(FXType value)
        {
            found = false;
            searchResult = null;
            for (int i = 0; i < _continers.Count; i++)
            {
                if (_continers[i].type == value)
                {
                    found = true;
                    searchResult = _continers[i];
                    return;
                }
            }

            Debug.Log($"FX with type - {value} does not exist in pool");
        }
        
        [SerializeField] [ShowIf("found")] [OnValueChanged("ChangeValueInContainer")] Container searchResult = null;

        [Space(10)]
        [SerializeField]
        // [ListDrawerSettings(NumberOfItemsPerPage = 10)]
        [ListDrawerSettings(Expanded = true, NumberOfItemsPerPage = 300)]
        private List<Container> _continers = new List<Container>();

        void ChangeValueInContainer(Container c)
        {
            Container mod = _continers.Find(cc => cc.type == c.type);
            if (mod == null) return;

            mod.gameObject = c.gameObject;
        }

        protected override void Setup()
        {
            foreach (var container in _continers)
            {
                Pool.Instance.InitPoolItem(container.gameObject, container.count);
            }
        }

        public FXItem Play(FXType type, Vector3 position, Transform parent)
        {
            var fx = Pop<FXItem>(type);

            Transform t = fx.transform;
            t.SetParent(parent);
            t.position = position;
            t.localScale = Vector3.one;

            fx.Play();

            return fx;
        }

        public FXItem Play(FXType type, Vector3 position, Quaternion rotation, Transform parent)
        {
            var fx = Pop<FXItem>(type);
            if (fx == null)
            {
                return null;
            }

            Transform t = fx.transform;
            t.SetParent(parent);
            t.localPosition = position;
            t.localRotation = rotation;
            t.localScale = Vector3.one;

            fx.Play();

            return fx;
        }

        public FXItem Play(FXType type, Vector3 position, Quaternion rotation, Transform parent, float delay)
        {
            var fx = Pop<FXItem>(type);
            if (fx == null)
            {
                return null;
            }

            var t = fx.transform;

            if (parent != null) t.SetParent(parent);
            t.localPosition = position;
            t.localRotation = rotation;
            t.localScale = Vector3.one;

            fx.Play(delay);

            return fx;
        }

        public FXItem Play(FXType type, Vector3 position, Quaternion rotation)
        {
            var fx = Pop<FXItem>(type);
            if (fx == null)
            {
                return null;
            }

            var t = fx.transform;
            t.position = position;
            t.rotation = rotation;
            t.localScale = Vector3.one;
            fx.Play();

            return fx;
        }

        public FXItem Play(FXType type, Transform parent)
        {
            var fx = Pop<FXItem>(type);
            if (fx == null)
            {
                return null;
            }

            Transform t = fx.transform;
            t.SetParent(parent);
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
            fx.Play();
            return fx;
        }

        public T Pop<T>(FXType type) where T : FXItem
        {
            var c = _continers.Find(x => x.type == type);
            if (c == null)
            {
                return null;
            }

            var p = Pool.Instance.Get(c.gameObject, null);
            return p.GetComponent<T>();
        }

        public void Push(FXItem item)
        {
            Pool.Instance.Return(item);
        }

        public void Push(FXItem item, float delay, Action onComplete = null)
        {
            float t = 0.0f;
            DOTween.To(() => t, x => t = x, 1.0f, delay).OnComplete(() => 
            {
                Pool.Instance.Return(item);
                onComplete?.Invoke();
            });
        }
    }
}
