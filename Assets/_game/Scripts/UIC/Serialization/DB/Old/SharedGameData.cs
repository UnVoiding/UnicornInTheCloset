// using System;
// using System.Collections.Generic;
// using UnityEngine;
//
// namespace RomenoCompany
// {
// 	[CreateAssetMenu(menuName = "TSG/Game/SharedGameData")]
// 	public class SharedGameData : ScriptableObject
// 	{
// 		[Header("Enemy")]
// 		public AnimationCurve damageColorCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
// 		public float damageColorDuration = 0.3f;
// 		public Color damageColor = Color.white;
// 		[Header("Enemy Damage")]
// 		public int damageTrimmer = 5;
// 		[Header("X2 reward")]
// 		public int levelStartX2 = 3;
//
// 		[Header("Gray color for ads buttons")]
// 		public Color grayColor = Color.gray;
//
// 		[Header("Rate")]
// 		public int levelShowRate = 4;
// 		public int reserveLevelShowRate = 8;
//
// 		[Header("Ads")]
// 		public int levelStartTalentRV = 5;
// 		public int levelStartRV = 1;
// 		public int levelStartInter = 5;
// 		public int interCooldown = 40;
//
// 		public float delayBetweenWaves = 2.0f;
// 		public float characterFlyDelay = 1.0f;
//
// 		[Header("Booster")]
// 		public GameObject boosterBox = null;
// 		public Vector3 boosterOffset = Vector3.zero;
// 		public float boosterAngle = 45;
// 		public float tossableRadius = 15.0f;
//
// 		[Header("Analytics")]
// 		public int monetaryEventLevelPeriod = 4;
//
// 		[Header("Other")]
// 		public int startTossablesAtLevel = 2;
// 		public float tossRemoveArea = 2;
// 		public int startVerticalizationAtLevel = 2;
//
// 		public float shopStarterTokenDistToCamera = 11;
// 		public float shopStarterCoinDistToCamera = 11;
// 		public float labTokenDistToCamera = 7;
// 		public float arcadeTokenDistToCamera = 2;
// 		public float questDistToCamera = 2;
//
// 		[Header("UI")]
// 		public int mainNavBarUnlockLevel = 4;
//
// 		[Header("FTUE"), Tooltip("Время бездействия игрока в секундах")]
// 		public float ftuiAttackTimeOut = 3f;
// 		public float ftueProgressWidgetTiming = 5f;
// 		public string ftueAbilityLevelText;
// 		public string ftueBossLevelText;
// 		public string ftueBonusLevelText;
//
// 		[Header("FTUI ToolTip settings"), Tooltip("Время появление бабла")]
// 		public float ftueToolTipShowTime = 0.3f;
//
// 		[Tooltip("Время скрытия бабла")]
// 		public float ftueToolTipHideTime = 0.2f;
// 		[Tooltip("Кривая размера бабла при показе")]
// 		public AnimationCurve ftueToolTipShowCurve;
// 		[Tooltip("Кривая размера бабла при скрытии")]
// 		public AnimationCurve ftueToolTipHideCurve;
//
// 		[Header("MainMenu Cutscene")]
// 		public int maxIdle0RepeatTimes = 2;
// 		public string idle0AnimName = "PlayArcade";
// 		public List<string> rareIdleAnimNames = new List<string>()
// 		{
// 			"UpKick",
// 			"Uppercut",
// 			"TreeShotCombo",
// 			"HeroBase_Hearts",
// 		};
// 	}
// }
