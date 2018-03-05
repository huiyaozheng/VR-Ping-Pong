using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object representing a bot's trained model and heuristics
/// </summary>
[System.Serializable]
public class BotPersonality : System.Object
{
    public TextAsset bytesFile;
    public string botGivenName;
    public CoachableHeuristics heuristics;

    public BotPersonality(string name, TextAsset bytesFile)
    {
        this.bytesFile = bytesFile;
        this.botGivenName = name;
        this.heuristics = new CoachableHeuristics();
    }
}