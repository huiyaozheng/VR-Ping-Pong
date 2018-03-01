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

		if (otherVel.magnitude >= 10) { // if we hit the ball hard - hardcode the value
			Debug.Log("True");
			highPitch.volume = maxVolume;
			highPitch.Play ();
		} else {
			float loudnessFactor = (ballVel - otherVel).magnitude;
			//	Debug.Log(loudnessFactor);
			loudnessFactor = Mathf.Clamp (loudnessFactor, minVolumeSpeed, maxVolumeSpeed);
			float volume = (maxVolume - minVolume) * (loudnessFactor - minVolumeSpeed) / (maxVolumeSpeed - minVolumeSpeed) + minVolume;

			audioSource.volume = volume;
			audioSource.Play ();
		}
	}
}
