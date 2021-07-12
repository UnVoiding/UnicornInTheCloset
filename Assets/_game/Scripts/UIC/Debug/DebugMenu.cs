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
		Inventory.Instance.worldState.Value.UnlockLawyer();
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

	[Category("General")]
	public void SkipTutorial()
	{
		Inventory.Instance.SkipTutorial();
	}
	
	[Category("General")]
	public void ShowVideo()
	{
		var videoClip = DB.Instance.videos.items.Get(videoNames[videoName]);
		var videoWidget = UIManager.Instance.GetWidget<VideoWidget>();
		videoWidget.StopVideo();
		UIManager.Instance.Wait(1.5f, () =>
		{
			videoWidget.ShowForVideo(videoClip);
		});
	}

	static SROptions()
	{
		videoNames = new Dictionary<VideoId, string>();
		videoNames[VideoId.church]			= "church";
		videoNames[VideoId.club]			= "club";
		videoNames[VideoId.concat_final]	= "concat_final";
		videoNames[VideoId.friendship]		= "friendship";
		videoNames[VideoId.government]		= "government";
		videoNames[VideoId.hot_home]		= "hot_home";
		videoNames[VideoId.love]			= "love";
		videoNames[VideoId.mother_calling2]	= "mother_calling2";
		videoNames[VideoId.mother_calling3]	= "mother_calling3";
		videoNames[VideoId.priest_home]		= "priest_home";
		videoNames[VideoId.rally]			= "rally";
		videoNames[VideoId.start]			= "start";
		videoNames[VideoId.teenager1]		= "teenager1";
		videoNames[VideoId.teenager2]		= "teenager2";
		videoNames[VideoId.training]		= "training";

	}

	public enum VideoId
	{
		church,
		club,
		concat_final,
		friendship,
		government,
		hot_home,
		love,
		mother_calling2,
		mother_calling3,
		priest_home,
		rally,
		start,
		teenager1,
		teenager2,
		training
	}
	
	private static Dictionary<VideoId, string> videoNames;
	private VideoId videoName;
	[Category("General")]
	public VideoId VideoName
	{
		get
		{
			return videoName;
		}
		set
		{
			videoName = value;
		}
	}
}