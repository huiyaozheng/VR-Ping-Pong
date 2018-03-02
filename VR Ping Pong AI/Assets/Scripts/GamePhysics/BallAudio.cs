using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class BallAudio : MonoBehaviour {

	public AudioSource audioSource;
	public AudioSource highPitch;
	public float minVolume;
	public float maxVolume;
	public float minVolumeSpeed;
	public float maxVolumeSpeed;

	void OnCollisionEnter(Collision col)
	{
		Vector3 ballVel = gameObject.GetComponent<Rigidbody>().velocity;
		Vector3 otherVel = Vector3.zero;

		Rigidbody otherBody = col.gameObject.GetComponent<Rigidbody>();
		if (otherBody != null)
		{
			otherVel = otherBody.velocity;
		}

		float loudnessFactor = (ballVel - otherVel).magnitude;

		if (false && loudnessFactor > maxVolumeSpeed)
		{
			highPitch.volume = 1;
			highPitch.Play();
		}
		else
		{
			//Debug.Log("loudnessFactor = " + loudnessFactor.ToString());
			loudnessFactor = Mathf.Clamp (loudnessFactor, minVolumeSpeed, maxVolumeSpeed);

			float volume = (maxVolume - minVolume) * (loudnessFactor - minVolumeSpeed) / (maxVolumeSpeed - minVolumeSpeed) + minVolume;

			audioSource.volume = volume;
			audioSource.Play ();
		}
	}
}
