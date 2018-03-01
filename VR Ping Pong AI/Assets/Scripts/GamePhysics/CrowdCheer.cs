using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdCheer : MonoBehaviour {
	public AudioSource crowdCheer;
	public float maxVolume;

	private void OnEvent_cheer ()
	{
		crowdCheer.volume = maxVolume;
		crowdCheer.Play ();
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
