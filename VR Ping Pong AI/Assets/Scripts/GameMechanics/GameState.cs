using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {

	public Player player0, player1;
	public Rigidbody racket0, racket1;
	public Collider table0, table1, floor;
	public Rigidbody ball;
    public PPAcademy acad;

	public int winningScore = 11;

	private bool player1StartedGame;
	public bool player1WonAPoint;
	private int score0, score1;

	private Vector3 defaultBallPos;

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
        acad.AcademyReset();
		score0 = 0;
		score1 = 0;
		player1StartedGame = false;
    }

	public void ResetBall()
	{
		ball.GetComponent<Shooter_no_reward>().ShootBall(DoesPlayer1Serve());
	}

	public void OnEvent_rallyEnded()
	{
        acad.AcademyReset();
        if (player1WonAPoint)
        {
            Debug.Log("P1WAP");
            score1++;
        }
        else
        {
            Debug.Log("P0WAP");
            score0++;
        }
		if(score0 >= winningScore || score1 >= winningScore)
		{
			if (Mathf.Abs(score0-score1) > 1)
			{
				Events.eSetEnded();
			}
		}

	    ball.GetComponent<Shooter_no_reward>().ShootBall(DoesPlayer1Serve());
    }

	// Use this for initialization
	void Start () {
		defaultBallPos = ball.position;
		InitGame();
	}
	
	// Update is called once per frame
	void Update () {
		
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
