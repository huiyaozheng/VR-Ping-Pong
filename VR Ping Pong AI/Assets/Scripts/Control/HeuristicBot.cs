using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeuristicBot : CatcherBot {

	//TODO: Save a successful shot's trajectory, sometimes replay it with slight noise.

	public BotPersonality bot; // Needs to be set dynamically
	public float lowHeight = 2.2f, medHeight=3.8f, highHeight=5.6f;

	private Vector3 lastShot = Vector3.zero;

	// Overrides:

	public override void startTracking()
	{
		reactionTimeTimer = 0.001f;
		reactionTimeDuration = OurMath.sampleNormalDistribution(bot.heuristics.reactionTime_mean, bot.heuristics.reactionTime_stDev);
	}
	private float reactionTimeTimer = 0f;
	private float reactionTimeDuration = 0f;

	protected override void Update()
	{
		if (reactionTimeTimer > 0f)
		{
			reactionTimeTimer += Time.deltaTime;
			if (reactionTimeTimer > reactionTimeDuration)
			{
				reactionTimeTimer = 0f;
				tracking = true;
			}
		}

		else
		{
			//Debug.Log("maxTrajH " + maxTrajectoryHeight);
			float currentZDistance = Mathf.Abs(ball.transform.position.z - myRacket.transform.position.z);

			// Move if the ball is incoming.
			if (currentZDistance < prevZDistance)
			{
				move(currentZDistance);
			}
			else
			{
				// If the racket is close to the default position, stop moving.
				if ((myDefPos - myRacket.transform.position).magnitude < 2)
				{
					myRacket.velocity = new Vector3(0,0,0);
				}
			}

			prevZDistance = currentZDistance;
		}
	}

	protected override void hit()
	{
		float aimHeight = Random.Range(2.0f, maxTrajectoryHeight);
		Vector3 aimPos = landPos;

		// Too high or too low - mistakes

		bool tooHigh = Random.value < bot.heuristics.errorRate_tooHigh;
		bool tooLow = Random.value < bot.heuristics.errorRate_tooLow;

		if(tooHigh)
		{
			aimPos *= 2;
			aimHeight *= 2;
			ball.velocity = PhysicsLibrary.PhysicsCalculations.velFromTraj(aimPos, ball.position, aimHeight, Physics.gravity.magnitude, false);
			lastShot = new Vector3(aimPos.x, aimHeight, aimPos.z);
			return;
		}
		if(tooLow)
		{
			aimPos.z *= 0.1f;
			aimHeight /= 2;
			ball.velocity = PhysicsLibrary.PhysicsCalculations.velFromTraj(aimPos, ball.position, aimHeight, Physics.gravity.magnitude, false);
			lastShot = new Vector3(aimPos.x, aimHeight, aimPos.z);
			return;
		}

		// Left/right preference

		bool goLeft = false;
		bool goRight = false;

		float r = Random.value;
		if (r < bot.heuristics.sidePreference_left_max05)
			goLeft = true;
		else if (r < bot.heuristics.sidePreference_left_max05 + bot.heuristics.sidePreference_right_max05)
			goRight = true;

		if (goLeft)
		{
			aimPos.x = (invertXZ ? 1 : -1) * Mathf.Abs(aimPos.x);
		}
		if (goRight)
		{
			aimPos.x = (invertXZ ? -1 : 1) * Mathf.Abs(aimPos.x);
		}

		// Max height preference
		bool goLow = false;
		bool goMedium = false;
		bool goHigh = false;
		float r2 = Random.value;
		if (r2 < bot.heuristics.heightPreference_low_max033)
			goLow = true;
		else if (r2 < bot.heuristics.heightPreference_low_max033 + bot.heuristics.heightPreference_medium_max033)
			goMedium = true;
		else if (r2 < bot.heuristics.heightPreference_low_max033 + bot.heuristics.heightPreference_medium_max033 + bot.heuristics.heightPreference_high_max033)
			goHigh = true;

		if (goLow)
		{
			aimHeight = Random.Range(lowHeight - 0.2f, lowHeight + 1.0f);
		}
		if (goMedium)
		{
			aimHeight = Random.Range(medHeight - 1.0f, medHeight + 1.0f);
		}
		if (goHigh)
		{
			aimHeight = Random.Range(highHeight - 1.0f, highHeight + 1.0f);
		}

		ball.velocity = PhysicsLibrary.PhysicsCalculations.velFromTraj(aimPos, ball.position, aimHeight, Physics.gravity.magnitude, false);
		lastShot = new Vector3(aimPos.x, aimHeight, aimPos.z);
		return;
	}




	private void OnEvent_rallyEnded()
	{
		// Only do this if we win the shot (which holds when we're not tracking - HACK).
		if (!lastShot.Equals(Vector3.zero) && !tracking)
		{
			int n = Statics.bot_winningPlays_listLength();
			while (bot.heuristics.winningShots.Count >= n)
				bot.heuristics.winningShots.RemoveAt(n - 1);
			bot.heuristics.winningShots.Insert(0, lastShot);
		}
		lastShot = Vector3.zero;
	}













}
