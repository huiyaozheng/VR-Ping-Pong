using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bots that can be chosen as the opponent.
/// </summary>
[System.Serializable]
public class BotPersonalities : System.Object
{
    public List<BotPersonality> bots;

    public BotPersonalities()
    {
        bots = new List<BotPersonality>();
    }
}