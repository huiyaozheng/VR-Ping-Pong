﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {

	public GameObject player0, player1;
	public Rigidbody racket0, racket1;
	public Collider table0, table1, floor;
	public Rigidbody ball;
    public PPAcademy acad;

	public int winningScore = 11;

	private bool player1StartedGame;

	[HideInInspector]
	public int score0 = 0, score1 = 0;

	[HideInInspector]
	/// Guaranteed to be set before the rallyEnded event is called!
	public bool player1WonAPoint;

	private bool veryFirstServe;

	public bool DoesPlayer1Serve()
	{
		int n = (score0 + score1) % 4;
		if (n < 2)
			return player1StartedGame;
		else
			return !player1StartedGame;
	}

	public void InitGame()
	{
        //acad.AcademyReset(); // Dobo Dobrik look what I'm doing 
		score0 = 0;
		score1 = 0;
		player1StartedGame = true;
	}

	public void ResetBall()
	{
		//ball.GetComponent<Shooter_no_reward>().ShootBall(DoesPlayer1Serve());  // Dobo Dobrik look what I'm doing 
	
		if (DoesPlayer1Serve())
		{
			// Bot serves.
			player1.GetComponent<HeuristicBot>().serve();
		}
		else
		{
			// Player serves.

		}
	}

	public void OnEvent_rallyEnded()
	{
		acad.AcademyReset();
        if (player1WonAPoint)
		{
			Debug.Log("P1WAP, current score: " + score0.ToString() + " - " + score1.ToString());
            score1++;
        }
        else
        {
			Debug.Log("P0WAP, current score: " + score0.ToString() + " - " + score1.ToString());
            score0++;
        }

		if(score0 >= winningScore || score1 >= winningScore)
		{
			if (Mathf.Abs(score0-score1) > 1)
			{
				Events.eSetEnded();
			}
		}
		else
		{
			ResetBall();
		}

		//ball.GetComponent<Shooter_no_reward>().ShootBall(DoesPlayer1Serve());  // Dobo Dobrik look what I'm doing 
    }

	// Use this for initialization
	void Start () {
		InitGame();
		veryFirstServe = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (veryFirstServe) {
			veryFirstServe = false;
			ResetBall();
		}
	}

	void OnEnable()
	{
		Events.rallyEnded += OnEvent_rallyEnded;
	}

	void OnDisable()
	{
		Events.rallyEnded -= OnEvent_rallyEnded;
	}
}
