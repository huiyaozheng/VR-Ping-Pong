using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// Hardcoded stuff goes here.
/// </summary>
public class Statics {

	public static string path_botPersonalities
	{
		get{ return Path.Combine(Application.streamingAssetsPath, "botPersonalities.json"); }
	}

	public static int bot_winningPlays_listLength
	{
		get{ return 6; }
	}

	public static float bot_winningPlays_replayProb
	{
		get{ return 0.05f; }
	}
}
