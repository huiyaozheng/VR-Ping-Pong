﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BotPersonalities : System.Object
{
    public List<BotPersonality> bots;

    public BotPersonalities()
    {
        bots = new List<BotPersonality>();
    }
}