using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Players the sound effect when a rally ends
/// </summary>
public class CrowdCheer : MonoBehaviour
{
    public AudioSource[] crowdCheer;

    private void OnEvent_cheer()
    {
        int play = Random.Range(0, crowdCheer.Length);
        crowdCheer[play].volume = 1;
        crowdCheer[play].Play();
    }

    void OnEnable()
    {
        Events.rallyEnded += OnEvent_cheer;
    }

    void OnDisable()
    {
        Events.rallyEnded -= OnEvent_cheer;
    }
}