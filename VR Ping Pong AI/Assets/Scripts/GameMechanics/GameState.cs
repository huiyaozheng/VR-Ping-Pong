using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public GameObject player0, player1;
    public Rigidbody racket0, racket1;
    public Collider table0, table1, floor;
    public Rigidbody ball;
    public PPAcademy acad;

    private Vector3 racket0DefaultPosition;
    private Vector3 racket1DefaultPosition;
    private Quaternion racket0DefaultRotation;
    private Quaternion racket1DefaultRotation;

    public int winningScore = 11;

    private bool player1StartedGame;

    public bool hasGameStarted = false;

    [HideInInspector] public int score0 = 0, score1 = 0;

    [HideInInspector]
    /// Guaranteed to be set before the rallyEnded event is called!
    public bool player1WonAPoint;

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
        score0 = 0;
        score1 = 0;
        player1StartedGame = true;
        ResetBall();
    }

    public void ResetBall()
    {
        hasGameStarted = false;
        //racket0.gameObject.transform.rotation = racket0DefaultRotation;
        //racket0.gameObject.transform.position = racket0DefaultPosition;
        //racket0.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        //racket0.gameObject.GetComponent<Catcher>().stopTracking();
        racket1.gameObject.transform.rotation = racket1DefaultRotation;
        racket1.gameObject.transform.position = racket1DefaultPosition;
        racket1.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        racket1.gameObject.GetComponent<Catcher>().stopTracking();
        if (DoesPlayer1Serve())
        {
			StartServeCountdown();
        }
        else
        {
            // TODO handle the case when the player is serving
            //racket0.gameObject.GetComponent<Catcher>().Serve();
			PlayerServe.ServeAllowed();
        }
        hasGameStarted = true;
        ball.GetComponent<GameCollisionTracker>().firstBounce = true;
    }

    public void OnEvent_rallyEnded()
    {
        acad.AcademyReset();
        if (player1WonAPoint)
        {
            score1++;
            Debug.Log("P1WAP, current score: " + score0.ToString() + " - " + score1.ToString());
        }
        else
        {
            score0++;
            Debug.Log("P0WAP, current score: " + score0.ToString() + " - " + score1.ToString());
        }

        if (score0 >= winningScore || score1 >= winningScore)
        {
            if (Mathf.Abs(score0 - score1) > 1)
            {
                Events.eSetEnded();
            }
        }
        else
        {
            ResetBall();
        }
    }

	private void StartServeCountdown() { serveDelayTimer = 0f; ball.gameObject.SetActive(false);}
	private float serveDelayTimer = -1f;
	public float serveDelayTimerDuration = 2f; // in seconds
	void Update()
	{
		if (serveDelayTimer >= 0f)
		{
			serveDelayTimer += Time.deltaTime;
			if (serveDelayTimer > serveDelayTimerDuration)
			{
				serveDelayTimer = -1f;
				// have the bot serve:
				ball.gameObject.SetActive(true);
				racket1.gameObject.GetComponent<Catcher>().Serve();
			}
		}
	}


    // Use this for initialization
    void Start()
    {
        racket0DefaultPosition = racket0.gameObject.transform.position;
        racket0DefaultRotation = racket0.gameObject.transform.rotation;
        racket1DefaultPosition = racket1.gameObject.transform.position;
        racket1DefaultRotation = racket1.gameObject.transform.rotation;
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