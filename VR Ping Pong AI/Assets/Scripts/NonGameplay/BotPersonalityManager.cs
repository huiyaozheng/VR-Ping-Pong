using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TensorFlowSharp;

public class BotPersonalityManager : MonoBehaviour {

	private static BotPersonalities botPersonalities = null;

	private static TextAsset botPreset01 = null;
	private static TextAsset botPreset02 = null;
	private static TextAsset botPreset03 = null;

	// Static fields can't be assigned to from the inspector, so we assign to these and then copy them over to the static fields immediately on Start.
	public TextAsset _botPreset01 = null;
	public TextAsset _botPreset02 = null;
	public TextAsset _botPreset03 = null;

	void Start () {

		// Assign to static variables:
		botPreset01 = _botPreset01;
		botPreset02 = _botPreset02;
		botPreset03 = _botPreset03;

		// Deserialize botPersonalities:
		string filePath = Statics.path_botPersonalities();

		if(File.Exists(filePath))
		{
			// Read the json from the file into a string
			string dataAsJson = File.ReadAllText(filePath); 
			// Pass the json to JsonUtility, and tell it to create a BotPersonalities object from it
			botPersonalities = JsonUtility.FromJson<BotPersonalities>(dataAsJson);
		}
		else
		{
			Debug.LogWarning("Cannot load bot personalities from " + filePath + "\n Creating a new BotPersonalities object.");
			botPersonalities = new BotPersonalities();
		}
	}

	private static void SaveBotPersonalities()
	{
		string dataAsJson = JsonUtility.ToJson (botPersonalities);
		string filePath = Statics.path_botPersonalities();
		File.WriteAllText (filePath, dataAsJson);
	}

	// OnDisable() is called after Alt+F4 and similar, so it's ok to do important finalization stuff here.
	void OnDisable()
	{
		SaveBotPersonalities();
	}

	// Just a getter
	public static BotPersonalities GetBotPersonalities() { return botPersonalities; }

	// Add a new bot personality
	private static void AddBotPersonality(BotPersonality bot) { botPersonalities.bots.Add(bot); }

	// Create a new bot personality from name and preset (neural net model)
	public enum eBotPreset { BP_01, BP_02, BP_03 };
	public static void CreateBotPersonality(string botGivenName, eBotPreset botPreset)
	{
		TextAsset botBytesFile = null;
		switch(botPreset)
		{
		case eBotPreset.BP_01:
			botBytesFile = botPreset01;
			break;
		case eBotPreset.BP_02:
			botBytesFile = botPreset02;
			break;
		case eBotPreset.BP_03:
			botBytesFile = botPreset03;
			break;
		}

		BotPersonality newBotPersonality = new BotPersonality(botGivenName, botBytesFile);
		AddBotPersonality(newBotPersonality);
	}

	// Remove an existing bot personality from the list
	public static void RemoveBotPersonality(BotPersonality bot) { botPersonalities.bots.Remove(bot); }

	public static void ChooseOpponent(BotPersonality bot, GameState game)
	{
		// game.player1 should always be set to a bot object, we will just change its personality, and the brain in control...
		game.player1.GetComponent<HeuristicBot>().bot = bot;
		game.racket1.gameObject.GetComponent<PPAimAgent>().brain.GetComponent<CoreBrainInternal>().graphModel = bot.bytesFile;
		game.racket1.gameObject.GetComponent<PPAimAgent>().brain.GetComponent<CoreBrainInternal>().InitializeCoreBrain();
	}

}
