using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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