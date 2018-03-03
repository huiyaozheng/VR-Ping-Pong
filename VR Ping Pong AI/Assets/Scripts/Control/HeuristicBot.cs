using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeuristicBotOld : CatcherBot {

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
			reactionTimeDuration = Mathf.Max(0f, OurMath.sampleNormalDistribution(bot.heuristics.reactionTime_mean, bot.heuristics.reactionTime_stDev));
		}
	}
	private float reactionTimeTimer = 0f;
	private float reactionTimeDuration = -1f;

	protected override void Update()
	{
		if (reactionTimeTimer > 0f)
		{
			reactionTimeTimer += Time.deltaTime;

			// Lack of reaction MEANS going back to default position, not just freezing in place anywhere
			if (IsRacketCloseToDefaultPos())
				myRacket.velocity = Vector3.zero;
			else
				myRacket.velocity = (myDefPos - myRacket.transform.position).normalized * maxRacketMovingSpeed * 0.2f;

			if (reactionTimeTimer > reactionTimeDuration)
			{
				reactionTimeTimer = 0f;
				Debug.Log("<HEURISTIC_BOT>: I am allowed to react now! reactionTimeDuration was " + reactionTimeDuration.ToString() + ", ... " + bot.heuristics.reactionTime_mean.ToString());
			}
		}

		else
		{
			base.Update();
		}
	}

	protected override void hit()
	{
        //DOBO: Use AimAgent's min of the third element please
		float aimHeight = maxTrajectoryHeight;
		Vector3 aimPos = landPos;

		// Too high or too low - mistakes

		bool tooHigh = Random.value < bot.heuristics.errorRate_tooHigh;
		bool tooLow = Random.value < bot.heuristics.errorRate_tooLow;

		if(tooHigh)
		{
			Debug.Log("<HEURISTIC_BOT>: I will shoot too high! My error rate for that was " + bot.heuristics.errorRate_tooHigh.ToString());
			aimPos *= 2;
			aimHeight *= 2;
			HitAndSaveShot(aimPos, aimHeight);
			return;
		}
		if(tooLow)
		{
			Debug.Log("<HEURISTIC_BOT>: I will shoot too low! My error rate for that was " + bot.heuristics.errorRate_tooLow.ToString());
			aimPos.z *= 0.1f;
			aimHeight /= 2;
			HitAndSaveShot(aimPos, aimHeight);
			return;
		}

		bool replayWinningShot = Random.value < Statics.bot_winningPlays_replayProb();
		if (replayWinningShot && bot.heuristics.winningShots.Count > 0)
		{
			Debug.Log("<HEURISTIC_BOT>: I will replay a winning shot now! I have that many winning shots stored: " + bot.heuristics.winningShots.Count.ToString() + ". That should be less or equal to " + Statics.bot_winningPlays_listLength().ToString());
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
			Debug.Log("<HEURISTIC_BOT>: I will shoot to the left half of the table! My probability for that override was " + bot.heuristics.GetLeftShotOverrideProbability().ToString());
			aimPos.x = (invertXZ ? 1 : -1) * Mathf.Abs(aimPos.x);
		}
		if (goRight)
		{
			Debug.Log("<HEURISTIC_BOT>: I will shoot to the right half of the table! My probability for that override was " + bot.heuristics.GetRightShotOverrideProbability().ToString());
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
		else if (r2 < bot.heuristics.GetLowShotOverrideProbability() + bot.heuristics.GetMediumShotOverrideProbability() + bot.heuristics.GetHighShotOverrideProbability())
			goHigh = true;

		if (goLow)
		{
			Debug.Log("<HEURISTIC_BOT>: I will shoot a low ball now! My probability for that override was " + bot.heuristics.GetLowShotOverrideProbability().ToString());
			aimHeight = Random.Range(lowHeight - 0.2f, lowHeight + 1.0f);
		}
		if (goMedium)
		{
			Debug.Log("<HEURISTIC_BOT>: I will shoot a medium-height ball now! My probability for that override was " + bot.heuristics.GetMediumShotOverrideProbability().ToString());
			aimHeight = Random.Range(medHeight - 1.0f, medHeight + 1.0f);
		}
		if (goHigh)
		{
			Debug.Log("<HEURISTIC_BOT>: I will shoot a high ball now! My probability for that override was " + bot.heuristics.GetHighShotOverrideProbability().ToString());
			aimHeight = Random.Range(highHeight - 1.0f, highHeight + 1.0f);
		}

		HitAndSaveShot(aimPos, aimHeight);
		return;
	}

	// Perform a hit with the specified trajectory, and save into the 'lastShot' field.
	private void HitAndSaveShot(Vector3 aimPos, float aimHeight)
	{
        float dist = Mathf.Abs(landPos.x-opponentRacket.transform.position.x);
        trainee.reward += expo((dist - 2.40f)) * 0.40f * trainee.multiplier;
        trainee.reward += expo(mult * (1.00f - maxTrajectoryHeight)) * 0.25f * trainee.multiplier;

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
				Debug.Log("<HEURISTIC_BOT>: I (" + bot.botGivenName + ") won a point, let's update heuristics!");
				UpdateHeuristicsAfterWin();
			}
			else
			{
				Debug.Log("<HEURISTIC_BOT>: I (" + bot.botGivenName + ") lost a point, let's update heuristics!");
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
		// Clamp inside a reasonable range.
		bot.heuristics.errorRate_tooHigh = Mathf.Clamp(bot.heuristics.errorRate_tooHigh, 0, 0.2f);
		bot.heuristics.errorRate_tooLow = Mathf.Clamp(bot.heuristics.errorRate_tooLow, 0, 0.2f);
		bot.heuristics.reactionTime_mean = Mathf.Clamp(bot.heuristics.reactionTime_mean, 0, 0.5f);

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
		// Clamp inside a reasonable range.
		bot.heuristics.errorRate_tooHigh = Mathf.Clamp(bot.heuristics.errorRate_tooHigh, 0, 0.2f);
		bot.heuristics.errorRate_tooLow = Mathf.Clamp(bot.heuristics.errorRate_tooLow, 0, 0.2f);
		bot.heuristics.reactionTime_mean = Mathf.Clamp(bot.heuristics.reactionTime_mean, 0, 0.5f);

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
		Events.rallyEnded += OnEvent_rallyEnded;
	}
	private void OnDisable()
	{
		Events.racketHitBall -= OnEvent_racketHitBall;
		Events.rallyEnded -= OnEvent_rallyEnded;
	}

}
