using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BackButton : MonoBehaviour {

	public UI_Manager UI_manager;

	public void OnClick()
	{
		UI_manager.parent_mainMenu.SetActive(true);
		UI_manager.parent_opponentCreation.SetActive(false);
		UI_manager.parent_opponentSelection.SetActive(false);
		UI_manager.parent_scoreDisplay.SetActive(false);
		UI_manager.parent_credits.SetActive(false);
		UI_manager.parent_settings.SetActive(false);
	}
}
