using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBallCollisions : MonoBehaviour {

	public List<Vector3> racketPositions = new List<Vector3>();
	public int positionTrackingDepthInFrames = 10; // 5 or 10 seems reasonable but not completely sure!
	public float speedMultiplier = 10f; // no idea what value will be good here, experiment needed


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
			ballBody.velocity = racketVelocity;
		}
	}
}
