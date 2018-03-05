using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBallCollisions : MonoBehaviour {

	public List<Vector3> racketPositions = new List<Vector3>();
	public int positionTrackingDepthInFrames = 10; // 5 or 10 seems reasonable but not completely sure!
	public float speedMultiplier = 340f; // no idea what value will be good here, experiment needed
	public float normalInfluence = 0.1f; // We multiply the (unit) normal vector by the length of the racket velocity, and then by this number!

	void Update()
	{
		racketPositions.Insert(0, transform.position);
		if (racketPositions.Count > positionTrackingDepthInFrames)
			racketPositions.RemoveAt(positionTrackingDepthInFrames);
	}

	public GameObject ball;
	public Rigidbody myRacketBody;

	void OnCollisionEnter(Collision col)
	{
		int index = Mathf.Min(racketPositions.Count - 1, positionTrackingDepthInFrames - 1);
		if (index < 0)
			return;
		
		Vector3 racketVelocity = (transform.position - racketPositions[index]) / (float)positionTrackingDepthInFrames; // normalize by dividing by the number of frames in between
		racketVelocity *= speedMultiplier;


		if (col.gameObject == ball)
		{
			Debug.Log("AWESOME COLLISION PHYSICS ACTING NOW");

			Rigidbody ballBody = ball.GetComponent<Rigidbody>();

			Vector3 racketNormal = transform.forward;
			if (Vector3.Dot(racketVelocity, racketNormal) < 0) // We want the normal to face the same way as racket velocity, rather than opposite
				racketNormal *= -1;

			racketNormal = racketNormal.normalized;

			ballBody.velocity = racketVelocity + racketNormal * racketVelocity.magnitude * normalInfluence;
		}
	}
}
