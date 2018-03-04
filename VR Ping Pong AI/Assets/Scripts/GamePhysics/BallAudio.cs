using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class BallAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public float minVolume;
    public float maxVolume;
    public float minVolumeSpeed;
    public float maxVolumeSpeed;
	public float highPitch = 1.1f;

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
        loudnessFactor = Mathf.Clamp(loudnessFactor, minVolumeSpeed, maxVolumeSpeed);

		if (loudnessFactor >= maxVolumeSpeed)
		{
			audioSource.volume = 1f;
			audioSource.pitch = highPitch;
			audioSource.Play();
        }
        else
        {
            float volume =
                (maxVolume - minVolume) * (loudnessFactor - minVolumeSpeed) / (maxVolumeSpeed - minVolumeSpeed) +
                minVolume;

			audioSource.volume = volume;
			audioSource.pitch = 1f;
            audioSource.Play();
        }
    }
}