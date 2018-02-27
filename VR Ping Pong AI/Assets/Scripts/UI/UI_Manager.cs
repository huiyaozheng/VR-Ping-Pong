using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Manager : MonoBehaviour {

	public GameObject 
		parent_mainMenu,
		parent_opponentSelection1,
		parent_scoreDisplay;

	public TextMeshProUGUI 
		opponentName0,
		opponentName1,
		opponentName2,
		opponentName3;

	public void OnClick_MainMenu_NewGame()
	{
		parent_mainMenu.SetActive(false);
		parent_opponentSelection1.SetActive(true);

		BotPersonality bot = null;
		if (BotPersonalityManager.GetBotPersonalities().bots.Count <= 0)
			opponentName0.text = "NULL";
		else
		{
			bot = BotPersonalityManager.GetBotPersonalities().bots[0];
			opponentName0.text = (bot == null) ? "NULL" : bot.botGivenName;
		}

		if (BotPersonalityManager.GetBotPersonalities().bots.Count <= 1)
			opponentName1.text = "NULL";
		else
		{
			bot = BotPersonalityManager.GetBotPersonalities().bots[1];
			opponentName1.text = (bot == null) ? "NULL" : bot.botGivenName;
		}

		if (BotPersonalityManager.GetBotPersonalities().bots.Count <= 2)
			opponentName2.text = "NULL";
		else
		{
			bot = BotPersonalityManager.GetBotPersonalities().bots[2];
			opponentName2.text = (bot == null) ? "NULL" : bot.botGivenName;
		}

		if (BotPersonalityManager.GetBotPersonalities().bots.Count <= 3)
			opponentName3.text = "NULL";
		else
		{
			bot = BotPersonalityManager.GetBotPersonalities().bots[3];
			opponentName3.text = (bot == null) ? "NULL" : bot.botGivenName;
		}
	}

	void TransitionToOpponentCreation()
	{

	}

	void TransitionToGame()
	{
		// Start the game
		parent_opponentSelection1.SetActive(false);
		parent_scoreDisplay.SetActive(true);
	}

	public void OnClick_OpponentSelection_Opponent0()
	{
		if (BotPersonalityManager.GetBotPersonalities().bots.Count <= 0)
			TransitionToOpponentCreation();
		else
		{
			BotPersonality bot = BotPersonalityManager.GetBotPersonalities().bots[0];
			if (bot == null)
				TransitionToOpponentCreation();
			else
				TransitionToGame();
		}
	}
}
