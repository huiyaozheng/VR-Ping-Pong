using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class BotPersonalityManager : MonoBehaviour {

	private static BotPersonalities botPersonalities = null;

	// Use this for initialization
	void Start () {
		
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
	public static void AddBotPersonality(BotPersonality bot) { botPersonalities.bots.Add(bot); }

	// Remove an existing bot personality
	public static void RemoveBotPersonality(BotPersonality bot) { botPersonalities.bots.Remove(bot); }

}
