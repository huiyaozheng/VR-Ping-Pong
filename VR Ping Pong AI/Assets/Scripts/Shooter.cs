using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Shooter : MonoBehaviour {

	private const float	REWARD_WIN  =  1.0f, // Given when the racket returns the ball successfully or the shooter misses and the racket doesn't hit the ball.
						REWARD_HIT  =  0.1f, // Given when the racket hit the ball in the right state, but the ball didn't go in.
						REWARD_FAIL = -1.0f; // Given when the racket loses a point, but is not eligible for the HIT reward.

	public Agent trainee;
	public Collider	floor, 
					closerTable, 			// The half closer to racket.
					furtherTable, 			// The half further from racket.
					racket;					// The racket of the bot being trained.

	private enum eShooterState{ State0, State1, State2 };
	private enum eShooterTransition{ ST_RACK, ST_BADTABLE, ST_GOODTABLE, ST_FLOOR };
	private eShooterState currState = eShooterState.State0;

	private Vector3 defaultBallPos;
	private Rigidbody ballBody;

	private float invertXZMult;

	void Start()
	{
		defaultBallPos = gameObject.transform.position;
		ballBody = gameObject.GetComponent<Rigidbody>();
		invertXZMult = trainee.gameObject.GetComponent<PPAgent>().invertXZ ? -1f : 1f;

		currState = eShooterState.State0;
		ShootBall();
	}

	void RewardAndReset(float reward)
	{
		trainee.reward = reward;
		ShootBall();
	}

	void MakeStep(eShooterTransition transition)
	{
		switch(currState)
		{
		case eShooterState.State0:
			switch (transition)
			{
			case eShooterTransition.ST_BADTABLE:
				currState = eShooterState.State1;
				break;
			case eShooterTransition.ST_RACK:
				RewardAndReset(REWARD_FAIL);
				break;
			default:
				RewardAndReset(REWARD_WIN);
				break;
			}
			break;
		case eShooterState.State1:
			switch (transition)
			{
			case eShooterTransition.ST_RACK:
				currState = eShooterState.State2;
				break;
			default:
				RewardAndReset(REWARD_FAIL);
				break;
			}
			break;
		case eShooterState.State2:
			switch (transition)
			{
			case eShooterTransition.ST_GOODTABLE:
				RewardAndReset(REWARD_WIN);
				break;
			default:
				RewardAndReset(REWARD_HIT);
				break;
			}
			break;
		}
	}

	void OnCollisionEnter(Collision col)
	{
		Debug.Log("Collided with " + col.gameObject.name);
		if (col.gameObject == floor.gameObject)
		{
			MakeStep(eShooterTransition.ST_FLOOR);
		} 
		else if (col.gameObject == closerTable.gameObject)
		{
			MakeStep(eShooterTransition.ST_BADTABLE);
		}
		else if (col.gameObject == furtherTable.gameObject)
		{
			MakeStep(eShooterTransition.ST_GOODTABLE);
		}
		else if (col.gameObject == racket.gameObject)
		{
			MakeStep(eShooterTransition.ST_RACK);
		}
	}




	public float minX, maxX, minZ, maxZ;

	void ShootBall()
	{
		currState = eShooterState.State0;
		gameObject.transform.position = defaultBallPos;
		float x = Random.Range(minX, maxX);
		float z = Random.Range(minZ, maxZ);
		ballBody.velocity = Vector3.zero;
		ballBody.AddForce(new Vector3(x, 0, z) * invertXZMult, ForceMode.Impulse);
	}


	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			currState = eShooterState.State0;
			ShootBall();
		}
	}
}
