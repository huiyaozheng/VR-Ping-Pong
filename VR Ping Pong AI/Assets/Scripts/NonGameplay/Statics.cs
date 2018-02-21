using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// Hardcoded stuff goes here.
/// </summary>
public class Statics {

	public static string path_botPersonalities()
	{
		return Path.Combine(Application.streamingAssetsPath, "botPersonalities.json");
	}

	public static int bot_winningPlays_listLength()
	{
		return 6;
	}

	public static float bot_winningPlays_replayProb()
	{
		return 0.05f;
	}
}
