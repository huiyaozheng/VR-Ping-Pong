using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour {

	public GameState game;

	public Rigidbody ball;
	public float maxSpeed = -1;

	void FixedUpdate()
	{
		if (maxSpeed > 0)
		{
			float speed = ball.velocity.magnitude;
			if (speed > maxSpeed)
			{
				ball.velocity *= (maxSpeed / speed);
				Debug.LogWarning("Capped ball's speed at " + maxSpeed.ToString());
			}
		}
	}
}
