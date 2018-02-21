using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BotPersonality : System.Object {
	
	public string botGivenName;
	public CoachableHeuristics heuristics;
	public string bytesPath;

	public BotPersonality(string name, string bytesPath)
	{
		this.bytesPath = bytesPath;
		this.botGivenName = name;
		this.heuristics = new CoachableHeuristics();
	}
}
