using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Shooter : MonoBehaviour
{
    /// <summary>
    /// Given when the racket returns the ball successfully or the shooter misses and the racket doesn't hit the ball.
    /// </summary>
    public float REWARD_WIN = 1.0f;
    /// <summary>
    /// Given when the racket hit the ball in the right state, but the ball didn't reach the other side.
    /// </summary>
    public float REWARD_HIT = 0.6f;
    /// <summary>
    /// Given when the racket loses a point, but is not eligible for the HIT reward.
    /// </summary>
    public float REWARD_FAIL = -1.0f;

    /// <summary>
    /// Lower bound of the initial velocity in X-axis (left/right)
    /// </summary>
    public float minX;
    /// <summary>
    /// Upper bound of the initial velocity in X-axis (left/right)
    /// </summary>
    public float maxX;
    /// <summary>
    /// Lower bound of the initial velocity in Z-axis (front/back)
    /// </summary>
    public float minZ;
    /// <summary>
    /// Upper bound of the initial velocity in Z-axis (front/back)
    /// </summary>
    public float maxZ;

    public Agent trainee;

    /// <summary>
    /// The floor.
    /// </summary>
    public Collider floor;
    /// <summary>
    /// The half closer to racket.
    /// </summary>
    public Collider closerTable;
    /// <summary>
    /// The half further from racket.
    /// </summary>
    public Collider furtherTable;
    /// <summary>
    /// The bot's racket.
    /// </summary>
    public Collider racket;

    /// <summary>
    /// States in the state machine.
    /// </summary>
	private enum eShooterState{ State0, State1, State2 };
    /// <summary>
    /// Inputs to the state machine.
    /// </summary>
	private enum eShooterTransition{ ST_RACK, ST_BADTABLE, ST_GOODTABLE, ST_FLOOR };
    /// <summary>
    /// Current state of the state machine.
    /// </summary>
	private eShooterState currState = eShooterState.State0;

    /// <summary>
    /// Default starting position of the ball.
    /// </summary>
	private Vector3 defaultBallPos;
    /// <summary>
    /// The ball's body.
    /// </summary>
	private Rigidbody ballBody;

    /// <summary>
    /// If XZ is inverted, set it to -1f. Otherwise it is 1f.
    /// </summary>
	private float invertXZMult;

	/// <summary>
    /// Give the reward, log it and reset the scene.
    /// </summary>
    /// <param name="reward">Value of the reward</param>
	void RewardAndReset(float reward)
	{
        Debug.Log("I received reward " + reward);
		trainee.reward = reward;
        racket.gameObject.transform.position = racket.gameObject.GetComponent<PPAgent>().defaultRacketPos;
        racket.gameObject.transform.rotation= racket.gameObject.GetComponent<PPAgent>().defaultRacketRot;
		ShootBall();
	}

    /// <summary>
    /// Step the state machine.
    /// </summary>
    /// <param name="transition">Input of the machine</param>
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

    /// <summary>
    /// When the ball collides with something, step the state machine.
    /// </summary>
    /// <param name="col">The collision object</param>
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

    /// <summary>
    /// Reset the ball to the default position and add a random velocity to the ball.
    /// </summary>
	void ShootBall()
	{
		currState = eShooterState.State0;
		gameObject.transform.position = defaultBallPos;
		float x = Random.Range(minX, maxX);
		float z = Random.Range(minZ, maxZ);
		ballBody.velocity = Vector3.zero;
		ballBody.AddForce(new Vector3(x, 0, z) * invertXZMult, ForceMode.Impulse);
	}

    void Start()
    {
        defaultBallPos = gameObject.transform.position;
        ballBody = gameObject.GetComponent<Rigidbody>();
        invertXZMult = trainee.gameObject.GetComponent<PPAgent>().invertXZ ? -1f : 1f;

        currState = eShooterState.State0;
        ShootBall();
    }

    void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			ShootBall();
		}
	}
}
