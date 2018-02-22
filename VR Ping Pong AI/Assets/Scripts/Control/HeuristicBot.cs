﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeuristicBot : CatcherBot {

	//TODO: Save a successful shot's trajectory, sometimes replay it with slight noise.

	public BotPersonality bot; // Needs to be set dynamically
	public float lowHeight = 2.2f, medHeight=3.8f, highHeight=5.6f;
	public float shotIsNotCentral = 2.75f;

	private Vector3 lastShot = Vector3.zero;
	private Vector3 lastShot2 = Vector3.zero; // Second-to-last shot
	private Vector3 lastShot3 = Vector3.zero; // Third-to-last shot

	// Overrides:

	private void OnEvent_racketHitBall(GameObject racketThatHitTheBall)
	{
		if (racketThatHitTheBall == gameObject)
		{
			reactionTimeTimer = 0.001f;
			reactionTimeDuration = OurMath.sampleNormalDistribution(bot.heuristics.reactionTime_mean, bot.heuristics.reactionTime_stDev);
		}
	}
	private float reactionTimeTimer = 0f;
	private float reactionTimeDuration = -1f;

	protected override void Update()
	{
		if (reactionTimeTimer > 0f)
		{
			reactionTimeTimer += Time.deltaTime;
			if (reactionTimeTimer > reactionTimeDuration)
			{
				reactionTimeTimer = 0f;
			}
		}

		else
		{
			base.Update();
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
			HitAndSaveShot(aimPos, aimHeight);
			return;
		}
		if(tooLow)
		{
			aimPos.z *= 0.1f;
			aimHeight /= 2;
			HitAndSaveShot(aimPos, aimHeight);
			return;
		}

		bool replayWinningShot = Random.value < Statics.bot_winningPlays_replayProb();
		if (replayWinningShot && bot.heuristics.winningShots.Count > 0)
		{
			int rand_ind = Random.Range(0, bot.heuristics.winningShots.Count - 1);
			aimPos.x = bot.heuristics.winningShots[rand_ind].x;
			aimPos.z = bot.heuristics.winningShots[rand_ind].z;
			aimHeight = bot.heuristics.winningShots[rand_ind].y;

			HitAndSaveShot(aimPos, aimHeight);
			return;
		}

		// Left/right preference

		bool goLeft = false;
		bool goRight = false;

		float r = Random.value;
		if (r < bot.heuristics.GetLeftShotOverrideProbability())
			goLeft = true;
		else if (r < bot.heuristics.GetLeftShotOverrideProbability() + bot.heuristics.GetRightShotOverrideProbability())
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
		if (r2 < bot.heuristics.GetLowShotOverrideProbability())
			goLow = true;
		else if (r2 < bot.heuristics.GetLowShotOverrideProbability() + bot.heuristics.GetMediumShotOverrideProbability())
			goMedium = true;
		else if (r2 < bot.heuristics.GetLowShotOverrideProbability() + bot.heuristics.GetMediumShotOverrideProbability() + bot.heuristics.GetHighShotOverrideProbability)
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

		HitAndSaveShot(aimPos, aimHeight);
		return;
	}

	// Perform a hit with the specified trajectory, and save into the 'lastShot' field.
	private void HitAndSaveShot(Vector3 aimPos, float aimHeight)
	{
		ball.velocity = PhysicsLibrary.PhysicsCalculations.velFromTraj(aimPos, ball.position, aimHeight, Physics.gravity.magnitude, false);
		lastShot3 = lastShot2;
		lastShot2 = lastShot;
		lastShot = new Vector3(aimPos.x, aimHeight, aimPos.z);
	}

	private void OnEvent_rallyEnded()
	{
		// Only do this if we win a point.
		if (!lastShot.Equals(Vector3.zero))
		{
			bool iAmPlayer1 = (game.player1 == gameObject);
			bool iAmPlayer0 = (game.player0 == gameObject);
			bool iWonAPoint = (iAmPlayer1 && game.player1WonAPoint || iAmPlayer0 && !game.player1WonAPoint);

			if (iWonAPoint)
			{
				Debug.Log(":) I (" + bot.botGivenName + ") won a point, let's update heuristics!");
				UpdateHeuristicsAfterWin();
			}
			else
			{
				Debug.Log(":( I (" + bot.botGivenName + ") lost a point, let's update heuristics!");
				UpdateHeuristicsAfterLoss();
			}
		}
		lastShot = Vector3.zero;
	}

	private bool shotIsToTheLeft(Vector3 shot)
	{
		return (invertXZ ^ (shot.x < 0f)) && (Mathf.Abs(shot.x) > shotIsNotCentral);
	}

	private bool shotIsToTheRight(Vector3 shot)
	{
		return (invertXZ ^ (shot.x > 0f)) && (Mathf.Abs(shot.x) > shotIsNotCentral);
	}

	private int shotHeight(Vector3 shot)
	{
		if (shot.y < (lowHeight + medHeight) / 2f)
			return 0; // LOW

		if (shot.y < (medHeight + highHeight) / 2f)
			return 1; // MEDIUM

		return 2; // HIGH
	}

	// Update heuristics after a won point, based on the three last shots: lastShot, lastShot2, lastShot3.
	private void UpdateHeuristicsAfterWin()
	{
		// Add a winning shot to the winning shots list.
		int n = Statics.bot_winningPlays_listLength();
		while (bot.heuristics.winningShots.Count >= n)
			bot.heuristics.winningShots.RemoveAt(n - 1);
		bot.heuristics.winningShots.Insert(0, lastShot);

		// Alter error rates and reaction time.
		// They (bot) won the point, so they won't learn too much. They are supposed to be coached by player, so they learn when player plays well.
		// Could add some random walk like this, but not really necessary, could comment it out:
		bot.heuristics.errorRate_tooHigh += Random.Range(-0.0005f, 0.0005f);
		bot.heuristics.errorRate_tooLow += Random.Range(-0.0005f, 0.0005f);
		bot.heuristics.reactionTime_mean += Random.Range(-0.0005f, 0.0005f);
		bot.heuristics.errorRate_tooHigh = Mathf.Clamp(bot.heuristics.errorRate_tooHigh, 0, 0.2f);
		bot.heuristics.errorRate_tooLow = Mathf.Clamp(bot.heuristics.errorRate_tooHigh, 0, 0.2f);
		bot.heuristics.reactionTime_mean = Mathf.Clamp(bot.heuristics.errorRate_tooHigh, 0, 0.5f);

		UpdateHeuristicsAfterWin_HelperFunction(weight: 1f, shot: lastShot);
		UpdateHeuristicsAfterWin_HelperFunction(weight: 0.5f, shot: lastShot2);
		UpdateHeuristicsAfterWin_HelperFunction(weight: 0.25f, shot: lastShot3);
	}

	// Update heuristics after a lost point, based on the three last shots: lastShot, lastShot2, lastShot3.
	private void UpdateHeuristicsAfterLoss()
	{
		// Alter error rates and reaction time.
		// They (bot) lost the point, so they will learn from the player. They are supposed to be coached by player, so they learn when player plays well.
		bot.heuristics.errorRate_tooHigh += Random.Range(-0.002f, 0.001f);
		bot.heuristics.errorRate_tooLow += Random.Range(-0.002f, 0.001f);
		bot.heuristics.reactionTime_mean += Random.Range(-0.002f, 0.001f);

		UpdateHeuristicsAfterLoss_HelperFunction(weight: 1f, shot: lastShot);
		UpdateHeuristicsAfterLoss_HelperFunction(weight: 0.5f, shot: lastShot2);
		UpdateHeuristicsAfterLoss_HelperFunction(weight: 0.25f, shot: lastShot3);
	}




	private void UpdateHeuristicsAfterLoss_HelperFunction(float weight, Vector3 shot)
	{
		if (!shot.Equals(Vector3.zero)) // -------------- lastShot
		{
			if (shotIsToTheLeft(shot))
			{
				bot.heuristics.sidePreference_left -= weight * 1f;
			}
			else if (shotIsToTheRight(shot)) // to the right
			{
				bot.heuristics.sidePreference_right -= weight *  1f;
			}
			else // neither right or left, shot was centred
			{
				bot.heuristics.sidePreference_right += weight * 0.5f;
				bot.heuristics.sidePreference_right = Mathf.Max(0f, bot.heuristics.sidePreference_right);
				bot.heuristics.sidePreference_left += weight * 0.5f;
				bot.heuristics.sidePreference_left = Mathf.Max(0f, bot.heuristics.sidePreference_left);
			}
			int sh = shotHeight(shot);
			if (sh == 0) // shot was low
			{
				bot.heuristics.heightPreference_low -= weight * 1f;
				bot.heuristics.heightPreference_medium += weight * 0.5f;
				bot.heuristics.heightPreference_medium = Mathf.Max(0f, bot.heuristics.heightPreference_medium);
				bot.heuristics.heightPreference_high += weight * 0.5f;
				bot.heuristics.heightPreference_high = Mathf.Max(0f, bot.heuristics.heightPreference_high);
			}
			else if (sh == 1) // shot was medium
			{
				bot.heuristics.heightPreference_low += weight * 0.5f;
				bot.heuristics.heightPreference_low = Mathf.Max(0f, bot.heuristics.heightPreference_low);
				bot.heuristics.heightPreference_medium -= weight * 1f;
				bot.heuristics.heightPreference_high += weight * 0.5f;
				bot.heuristics.heightPreference_high = Mathf.Max(0f, bot.heuristics.heightPreference_high);
			}
			else if (sh == 2) // shot was high
			{
				bot.heuristics.heightPreference_low += weight * 0.5f;
				bot.heuristics.heightPreference_low = Mathf.Max(0f, bot.heuristics.heightPreference_low);
				bot.heuristics.heightPreference_medium += weight * 0.5f;
				bot.heuristics.heightPreference_medium = Mathf.Max(0f, bot.heuristics.heightPreference_medium);
				bot.heuristics.heightPreference_high -= weight * 1f;
			}
		}
	}

	private void UpdateHeuristicsAfterWin_HelperFunction(float weight, Vector3 shot)
	{
		if (!shot.Equals(Vector3.zero)) // -------------- lastShot
		{
			if (shotIsToTheLeft(shot))
			{
				bot.heuristics.sidePreference_left += weight * 1f;
			}
			else if (shotIsToTheRight(shot)) // to the right
			{
				bot.heuristics.sidePreference_right += weight *  1f;
			}
			else // neither right or left, shot was centred
			{
				bot.heuristics.sidePreference_right -= weight * 0.5f;
				bot.heuristics.sidePreference_right = Mathf.Max(0f, bot.heuristics.sidePreference_right);
				bot.heuristics.sidePreference_left -= weight * 0.5f;
				bot.heuristics.sidePreference_left = Mathf.Max(0f, bot.heuristics.sidePreference_left);
			}
			int sh = shotHeight(shot);
			if (sh == 0) // shot was low
			{
				bot.heuristics.heightPreference_low += weight * 1f;
				bot.heuristics.heightPreference_medium -= weight * 0.5f;
				bot.heuristics.heightPreference_medium = Mathf.Max(0f, bot.heuristics.heightPreference_medium);
				bot.heuristics.heightPreference_high -= weight * 0.5f;
				bot.heuristics.heightPreference_high = Mathf.Max(0f, bot.heuristics.heightPreference_high);
			}
			else if (sh == 1) // shot was medium
			{
				bot.heuristics.heightPreference_low -= weight * 0.5f;
				bot.heuristics.heightPreference_low = Mathf.Max(0f, bot.heuristics.heightPreference_low);
				bot.heuristics.heightPreference_medium += weight * 1f;
				bot.heuristics.heightPreference_high -= weight * 0.5f;
				bot.heuristics.heightPreference_high = Mathf.Max(0f, bot.heuristics.heightPreference_high);
			}
			else if (sh == 2) // shot was high
			{
				bot.heuristics.heightPreference_low -= weight * 0.5f;
				bot.heuristics.heightPreference_low = Mathf.Max(0f, bot.heuristics.heightPreference_low);
				bot.heuristics.heightPreference_medium -= weight * 0.5f;
				bot.heuristics.heightPreference_medium = Mathf.Max(0f, bot.heuristics.heightPreference_medium);
				bot.heuristics.heightPreference_high += weight * 1f;
			}
		}
	}

	private void OnEnable()
	{
		Events.racketHitBall += OnEvent_racketHitBall;
	}
	private void OnDisable()
	{
		Events.racketHitBall -= OnEvent_racketHitBall;
	}

}
