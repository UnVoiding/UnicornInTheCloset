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
}