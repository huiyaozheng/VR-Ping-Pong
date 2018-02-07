using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script should be attached to the ball.

[RequireComponent(typeof(Collider))]
public class GameCollisionTracker : MonoBehaviour {

	public GameState game;

	private RallyState rallyState;
	private Collider table0, table1, racket0, racket1, floor;

	void Start()
	{
		rallyState = game.GetComponent<RallyState>();
		table0 = game.table0;
		table1 = game.table1;
		floor = game.floor;
		racket0 = game.racket0.GetComponent<Collider>();
		racket1 = game.racket1.GetComponent<Collider>();
	}
    public float eRallyDisableTime = 1.5f;
    public float eRallyDisableTimer = 0.0f;
	void OnCollisionEnter(Collision col)
	{
		//if (ballColliderDisableTimer > 0f)
		//{
		//	ballColliderDisableTimer += Time.deltaTime;
		//	if (ballColliderDisableTimer > ballColliderDisableTime)
		//	{
		//		ballColliderDisableTimer = 0f;
		//		ballCollider.enabled = true;
		//		Debug.Log("Enabled ball's collider");
		//	}
		//}
        Debug.Log("Collision");
		RallyState.eRallyOutcome outcome = RallyState.eRallyOutcome.RO_NONE;

		if (col.collider == table0)
			outcome = rallyState.MakeStep(game.DoesPlayer1Serve() ? RallyState.eRallyStateMachineAction.RSMA_DEF_TABLE : RallyState.eRallyStateMachineAction.RSMA_ATT_TABLE);

		if (col.collider == table1)
			outcome = rallyState.MakeStep(game.DoesPlayer1Serve() ? RallyState.eRallyStateMachineAction.RSMA_ATT_TABLE : RallyState.eRallyStateMachineAction.RSMA_DEF_TABLE);

		if (col.collider == racket0)
			outcome = rallyState.MakeStep(game.DoesPlayer1Serve() ? RallyState.eRallyStateMachineAction.RSMA_DEF_RACK : RallyState.eRallyStateMachineAction.RSMA_ATT_RACK);

		if (col.collider == racket1)
			outcome = rallyState.MakeStep(game.DoesPlayer1Serve() ? RallyState.eRallyStateMachineAction.RSMA_ATT_RACK : RallyState.eRallyStateMachineAction.RSMA_DEF_RACK);

		if (col.collider == floor)
			outcome = rallyState.MakeStep(RallyState.eRallyStateMachineAction.RSMA_OUT);

		if (outcome == RallyState.eRallyOutcome.RO_NONE)
			return;

		game.player1WonAPoint = game.DoesPlayer1Serve() ^ (outcome == RallyState.eRallyOutcome.RO_DEF_WINS);

        Debug.Log("OUTCOME:"+outcome);
        //eRallyDisableTime = 0.0f;
        //Debug.Log("Timers: " + eRallyDisableTimer + " " + eRallyDisableTime);
        //if (eRallyDisableTimer > eRallyDisableTime)
        //{
            Events.eRallyEnded();
            //eRallyDisableTimer = 0;
        //}
        //eRallyDisableTimer += 0.1f;
	}

	void OnEvent_rallyEnded()
	{

	}

}
