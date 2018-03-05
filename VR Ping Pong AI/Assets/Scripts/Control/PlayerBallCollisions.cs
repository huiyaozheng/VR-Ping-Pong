using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBallCollisions : MonoBehaviour {

	public GameObject ball;
	public Rigidbody myRacketBody;

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject == ball)
		{
			Debug.Log("AWESOME COLLISION PHYSICS ENABLED NOW");
			Rigidbody ballBody = ball.GetComponent<Rigidbody>();
			ballBody.velocity = myRacketBody.velocity * 1.2f;
		}
	}
}
