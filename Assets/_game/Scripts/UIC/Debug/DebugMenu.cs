using System;
using System.ComponentModel;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using RomenoCompany;

public partial class SROptions
{
	[Category("Items")]
	public void UnlockAllItems()
	{
		var states = Inventory.Instance.worldState.Value.gameItemStates;
		foreach (var state in states)
		{
			state.found = true;
		}
		Inventory.Instance.worldState.Save();
	}

	[Category("Companions")]
	public void ResetCompanion()
	{
		if (Companion != CompanionData.ItemID.NONE)
		{
			var comp = Inventory.Instance.worldState.Value.GetCompanion(Companion);
			for (int i = comp.activeDialogue; i >= activeDialogue; i--)
			{
				comp.dialogues[i].path.Clear();
			}
			comp.activeDialogue = activeDialogue;
			comp.lastDialogueTaken = activeDialogue - 1;
		}
	}

	private CompanionData.ItemID companion;
	[Category("Companions")]
	public CompanionData.ItemID Companion
	{
		get
		{
			return companion;
		}
		set
		{
			companion = value;
			UpdateActiveDialogue();
		}
	}

	private int activeDialogue;
	[Category("Companions")]
	public int ActiveDialogue
	{
		get
		{
			return activeDialogue;
		}
		set
		{
			activeDialogue = value;
		}
	}

	private void UpdateActiveDialogue()
	{
		if (companion != CompanionData.ItemID.NONE)
		{
			var cs = Inventory.Instance.worldState.Value.GetCompanion(companion);
			activeDialogue = cs.activeDialogue;
		}
		else
		{
			activeDialogue = 0;
		}

		OnPropertyChanged("ActiveDialogue");
	}

	[Category("Companions")]
	public void UnlockAllCompanions()
	{
		var states = Inventory.Instance.worldState.Value.companionStates;
		foreach (var state in states)
		{
			state.locked = false;
		}
		Inventory.Instance.worldState.Save();

		UIManager.Instance.GetWidget<MainScreenWidget>().UpdateCompanionBtns();
	}

	[Category("Unicorn Advices")]
	public void UnlockAllAdvices()
	{
		var advicesState = Inventory.Instance.worldState.Value.unicornAdvicesState;
		foreach (var state in advicesState.unicornAdviceStates)
		{
			state.found = true;
		}
		Inventory.Instance.worldState.Save();
	}
	
	[Category("General")]
	public void RestartGame()
	{
		GameManager.Instance.RestartGame();
	}

	[Category("General")]
	public void UnlockLawyer()
	{
		Inventory.Instance.worldState.Value.lawyerFinished = true;
		Inventory.Instance.worldState.Save();
	}

	[Category("General")]
	public void DisableWait()
	{
		Inventory.Instance.disableWait.Value = true;
		Inventory.Instance.disableWait.Save();
	}

	[Category("General")]
	public void EnableWait()
	{
		Inventory.Instance.disableWait.Value = false;
		Inventory.Instance.disableWait.Save();
	}

	

	// [Category("Unicorn Advices")]
	// public void UnlockAllUnicornAdvices()
	// {
	// 	
	// }
	
	// [Category("Unicorn Advices")]
	// public void UnlockAllUnicornAdvices()
	// {
	// 	
	// }

	
	
	
	
	
	
	
	
	
	
	
 //
	// [Category("Polygon")]
	// [DisplayName("Go to game")]
	// public void GoToGame()
	// {
	// 	if (SceneLoader.Instance.ActiveScene.name.Equals("Menu")) return;
	// 	SceneLoader.Instance.GoToScene("Menu");
	// }
 //
	// [Category("FTUE")]
	// public void SkipTutorial()
	// {
	// 	Inventory.Instance.ftueState.Value = Inventory.FTUEState.NONE;
	// 	Inventory.Instance.ftueState.Save();
	// 	SceneLoader.Instance.GoToScene("Menu");
	// }
 //
	// [Category("FTUE")]
	// public void RestartGame()
	// {
	// 	Inventory.Instance.ABGroupSprint20.Value = Inventory.defaultAbGroupSprint20;
	// 	Inventory.Instance.ABGroupSprint20.Save();
	// 	OnPropertyChanged("ABGroup");
 //
	// 	PlayerPrefs.DeleteAll();
	// 	DestroyPersistentObjects();
 //
	// 	// Inventory.Instance.ABGroupSprint19.Value = Inventory.defaultAbGroupSprint19;
	// 	// Inventory.Instance.ABGroupSprint19.Save();
	// 	// DB.Instance.Group = Inventory.defaultAbGroupSprint19;
 //
	// 	SceneLoader.Instance.GoToScene("Loading");
	// }
 //
	// [Category("A/B Testing")]
	// [DisplayName("ABGroup")]
	// public int ABGroup => Inventory.Instance.ABGroupSprint20.Value;
 //
	// [Category("A/B Testing")]
	// [DisplayName("New Version B - 1")]
	// public void NewABGroupVersion() => ChangeABGroup(1);
 //
	// [Category("A/B Testing")]
	// [DisplayName("Default Version A - 0")]
	// public void DefaultABGroupVersion() => ChangeABGroup(0);
 //
	// private void ChangeABGroup(int abGroup)
	// {
	// 	if (Inventory.Instance.ABGroupSprint20.Value != abGroup)
	// 	{
	// 		PlayerPrefs.DeleteAll();
 //
	// 		DestroyPersistentObjects();
 //
	// 		PlayerPrefs.SetInt("useSpecificABGroup", abGroup);
	// 		DB.Instance.Group = abGroup;
	// 		
	// 		// DB.Instance.SaveGroup();
	// 		Inventory.Instance.ABGroupSprint20.Value = abGroup;
	// 		OnPropertyChanged("ABGroup");
	// 		SceneLoader.Instance.GoToScene("Loading");
	// 	}
	// }
 //
	// public static void DestroyPersistentObjects()
	// {
	// 	GameObject.Destroy(Inventory.Instance.gameObject);
	// 	Inventory.ResetInstance();
	// }
 //
	// [Category("Abilities")]
	// public void SuperPunch()
	// {
	// }
 //
	// [Category("Abilities")]
	// public void UnlockAllAbilities()
	// {
	// 	List<AbilityData> ad = DB.Instance.abilities.Items;
	// 	for (int i = 0; i < ad.Count; i++)
	// 	{
	// 	}
	// 	SceneLoader.Instance.ReloadActiveScene();
	// }
	// //Cooldown for starterpack button
	// /*[Category("Purchases")]
 //    public string TimeCooldown => TimeManager.TimeToString((long)(DB.Instance.purchases.StarterPackCooldown * 60 - TimeManager.getTimeSecondsNow + Inventory.Instance.starterPackCooldownTime.Value));
 //    [Category("Purchases")]
 //    public void DebugTimeCooldown() 
 //    {
 //        OnPropertyChanged("TimeCooldown");
 //        Debug.Log($"Time cooldown: {TimeManager.TimeToString((long)(DB.Instance.purchases.StarterPackCooldown * 60 - TimeManager.getTimeSecondsNow + Inventory.Instance.starterPackCooldownTime.Value))}");
 //    }
 //    [Category("Purchases")]
 //    public void SkipCooldownPopup() => Inventory.Instance.starterPackCooldownTime.Value -= (long)(DB.Instance.purchases.StarterPackCooldown * 60);*/
 //
	// private int _purchaseNumber = 0;
	// [Category("Purchases")]
	// public int PurchaseNumber
	// {
	// 	get => _purchaseNumber;
	// 	set => _purchaseNumber = value;
	// }
	//
	// [Category("Purchases")]
	// public void Purchase()
	// {
	// }
	//
	// [Category("Superpowers")]
	// public void UnlockAllSuperpowers()
	// {
	// 	List<SuperpowerData> sd = DB.Instance.superpowers.Items;
	// 	Inventory.Instance.superpowersShown.Value = sd.Count;
	// 	for (int i = 0; i < sd.Count; i++)
	// 	{
	// 		Inventory.Instance.SuperpowerAdd(sd[i].itemId);
	// 	}
	// 	SceneLoader.Instance.ReloadActiveScene();
	// }
 //
	// [Category("Superpowers")]
	// public void UnlockGameOnlySuperpowers()
	// {
	// 	List<SuperpowerData> sd = DB.Instance.superpowers.Items.Where(x => !x.inappPurchase).ToList();
	// 	Inventory.Instance.superpowersShown.Value = sd.Count;
	// 	for (int i = 0; i < sd.Count; i++)
	// 	{
	// 		Inventory.Instance.SuperpowerAdd(sd[i].itemId);
	// 	}
	// 	SceneLoader.Instance.ReloadActiveScene();
	// }
 //
	// [Category("Superpowers")]
	// public void UnlockGameOnlyMinusSuperpowers()
	// {
	// 	List<SuperpowerData> sd = DB.Instance.superpowers.Items.Where(x => !x.inappPurchase).ToList();
	// 	Inventory.Instance.superpowersShown.Value = sd.Count - 1;
	// 	for (int i = 0; i < sd.Count - 1; i++)
	// 	{
	// 		Inventory.Instance.SuperpowerAdd(sd[i].itemId);
	// 	}
	// 	SceneLoader.Instance.ReloadActiveScene();
	// }
 //
	// [Category("Skins")]
	// public void UnlockAllSkins()
	// {
	// 	foreach (var skinStatus in Inventory.Instance.skinsStatus.Value)
	// 	{
	// 	}
	// 	Inventory.Instance.skinsStatus.Save();
 //
	// 	SceneLoader.Instance.ReloadActiveScene();
	// }
 //
	// [Category("Resources")]
	// [DisplayName("AddCoins")]
	// public int Coins
	// {
	// 	get => Inventory.Instance.Coins;
	// 	set => Inventory.Instance.Coins.Value += value;
	// }
 //
	// [Category("Resources")]
	// [DisplayName("AddKeys")]
	// public int Keys
	// {
	// 	get => Inventory.Instance.Keys;
	// 	set => Inventory.Instance.Keys.Value += value;
	// }
 //
	// [Category("Resources")]
	// [DisplayName("AddTokens")]
	// public int Tokens
	// {
	// 	get => Inventory.Instance.Tokens;
	// 	set => Inventory.Instance.Tokens.Value += value;
	// }
 //
	// private int _talentLevel = 10;
 //
	// [Category("Talents")]
	// [DisplayName("Level")]
	// [Sort(0)]
	// public int AllTalents
	// {
	// 	get => _talentLevel;
	// 	set => _talentLevel = value;
	// }
 //
	// [Category("Talents")]
	// [DisplayName("Set All")]
	// [Sort(1)]
	// public void SetAllTalentsLevel()
	// {
	// 	Inventory.Instance.TalentSetLevel(TalentData.ItemID.HEALTH, _talentLevel);
	// 	Inventory.Instance.TalentSetLevel(TalentData.ItemID.DAMAGE, _talentLevel);
	// 	Inventory.Instance.TalentSetLevel(TalentData.ItemID.ATTACK_SPEED, _talentLevel);
	// }
 //
	// [Category("Talents")]
	// [DisplayName("Set Health")]
	// [Sort(2)]
	// public void SetHealthTalentLevel()
	// {
	// 	Inventory.Instance.TalentSetLevel(TalentData.ItemID.HEALTH, _talentLevel);
	// }
 //
	// [Category("Talents")]
	// [DisplayName("Set Power")]
	// [Sort(3)]
	// public void Power()
	// {
	// 	Inventory.Instance.TalentSetLevel(TalentData.ItemID.DAMAGE, _talentLevel);
	// }
 //
	// [Category("Talents")]
	// [DisplayName("Set Attack Speed")]
	// [Sort(4)]
	// public void AttackSpeed()
	// {
	// 	Inventory.Instance.TalentSetLevel(TalentData.ItemID.ATTACK_SPEED, _talentLevel);
	// }
 //
	// [Category("Talents")]
	// [DisplayName("SkipAllSeconds")]
	// public void SkipAllSeconds()
	// {
	// 	Inventory.Instance.SetTalentTimestamp(TalentData.ItemID.DAMAGE,
	// 		TimeManager.Instance.CurrentDeviceTimestamp - DB.Instance.talents.GetItem(TalentData.ItemID.DAMAGE).cooldownForAds);
	// 	Inventory.Instance.SetTalentTimestamp(TalentData.ItemID.ATTACK_SPEED,
	// 		TimeManager.Instance.CurrentDeviceTimestamp - DB.Instance.talents.GetItem(TalentData.ItemID.ATTACK_SPEED).cooldownForAds);
	// 	Inventory.Instance.SetTalentTimestamp(TalentData.ItemID.HEALTH,
	// 		TimeManager.Instance.CurrentDeviceTimestamp - DB.Instance.talents.GetItem(TalentData.ItemID.HEALTH).cooldownForAds);
	// }
 //
	// [Category("Talents")]
	// [DisplayName("SkipPower")]
	// public void SkipPower()
	// {
	// 	Inventory.Instance.SetTalentTimestamp(TalentData.ItemID.DAMAGE,
	// 		TimeManager.Instance.CurrentDeviceTimestamp - DB.Instance.talents.GetItem(TalentData.ItemID.DAMAGE).cooldownForAds);
	// }
 //
	// [Category("Talents")]
	// [DisplayName("SkipAttackSpeed")]
	// public void SkipAttackSpeed()
	// {
	// 	Inventory.Instance.SetTalentTimestamp(TalentData.ItemID.ATTACK_SPEED,
	// 		TimeManager.Instance.CurrentDeviceTimestamp - DB.Instance.talents.GetItem(TalentData.ItemID.ATTACK_SPEED).cooldownForAds);
	// }
 //
	// [Category("Talents")]
	// [DisplayName("SkipHealth")]
	// public void SkipHealth()
	// {
	// 	Inventory.Instance.SetTalentTimestamp(TalentData.ItemID.HEALTH,
	// 		TimeManager.Instance.CurrentDeviceTimestamp - DB.Instance.talents.GetItem(TalentData.ItemID.HEALTH).cooldownForAds);
	// }
	//
	// private int _levelIdx = 1;
	// [Category("Completion")]
	// [DisplayName("Level")]
	// public int LocationIdx
	// {
	// 	get => _levelIdx; set
	// 	{
	// 		_levelIdx = value;
	// 		Wave = _waveIdx;
	// 	}
	// }
 //
	// private int _waveIdx = 1;
	// [Category("Completion")]
	// [DisplayName("Wave")]
	// public int Wave
	// {
	// 	get => _waveIdx; set
	// 	{
	// 		var waves = DB.Instance.progression.GetLevel(_levelIdx).waves;
	// 		_waveIdx = Mathf.Clamp(value, 0, waves.Count);
	// 	}
	// }
 //
	// [Category("Completion")]
	// [DisplayName("SetCompletion")]
	// public void SetCompletion()
	// {
	// 	for (int i = 0; i < _levelIdx; i++)
	// 	{
	// 		var level = DB.Instance.progression.GetLevel(i);
	// 	}
	// 	Inventory.Instance.SetCompletion(_levelIdx, _waveIdx, 0);
	// 	SceneLoader.Instance.ReloadActiveScene();
	// }
 //
	// // [Category("Complete location")]
	// // [DisplayName("CompleteLocation")]
	// // public void EndLocation()
	// // {
	// //     var locationidx = (LocationData.ItemID)(LocationIdx-1);
	// //     // var location = DB.Instance.locations.Items.Find(x => x.Key == locationidx);
	// //     // Inventory.Instance.SetCompletion(locationidx, location.waves, true);
	// //     // Inventory.Instance.LocationsCompletion.Save();
	// //
	// //     var a = SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Single);
	// //
	// //     //SceneManager.LoadScene("Main", LoadSceneMode.Additive);
	// //
	// //     //unload.completed += (UnityEngine.AsyncOperation a) => { SceneManager.LoadSceneAsync("Menu"); };
	// // }
 //
	// private bool _enableUI = true;
	// [Category("Profiling")]
	// [DisplayName("EnableUI")]
	// public bool EnableUI
	// {
	// 	get => UIManager.Instance.guiCamera.gameObject.activeSelf;
	// 	set => UIManager.Instance.guiCamera.gameObject.SetActive(value);
	// }
 //
	// [Category("Profiling")]
	// [DisplayName("EnableAllLogs")]
	// public bool EnableAllLogs
	// {
	// 	get => Debug.unityLogger.logEnabled;
	// 	set => Debug.unityLogger.logEnabled = value;
	// }
 //
	// [Category("Profiling")]
	// [DisplayName("EnableShadows")]
	// public bool EnableShadows
	// {
	// 	get => QualitySettings.shadows == ShadowQuality.All;
	// 	set => QualitySettings.shadows = value ? ShadowQuality.All : ShadowQuality.Disable;
	// }
 //
	// [Category("Profiling")]
	// [DisplayName("OcclusionCulling")]
	// public bool OcclusionCulling
	// {
	// 	get => Eye.Instance.Camera.useOcclusionCulling;
	// 	set => Eye.Instance.Camera.useOcclusionCulling = value;
	// }
 //
	// private bool _replacedHUD = false;
	// [Category("Profiling")]
	// [DisplayName("ReplacedHUD")]
	// public bool ReplacedHUD
	// {
	// 	get {return _replacedHUD;}
	// 	set {
	// 		_replacedHUD = value;
	// 		if (value) {
	// 			UIManager.Instance.originalImages = new List<Sprite>();
	// 			for (var i = 0; i < UIManager.Instance.replaceImages.Count; i++) {
	// 				UIManager.Instance.originalImages.Add(UIManager.Instance.replaceImages[i].sprite);
	// 				UIManager.Instance.replaceImages[i].sprite = UIManager.Instance.replaceSprite;
	// 			}
	// 		} else {
	// 			for (var i = 0; i < UIManager.Instance.replaceImages.Count; i++) {
	// 				UIManager.Instance.replaceImages[i].sprite = UIManager.Instance.originalImages[i];
	// 			}
	// 		}
	// 	}
	// }
	
}