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

	void OnCollisionEnter(Collision col)
	{
	    if (!game.hasGameStarted) return;
		RallyState.eRallyOutcome outcome = RallyState.eRallyOutcome.RO_NONE;

        if (col.collider == table0)
        {
            outcome = rallyState.MakeStep(game.DoesPlayer1Serve() ? RallyState.eRallyStateMachineAction.RSMA_DEF_TABLE : RallyState.eRallyStateMachineAction.RSMA_ATT_TABLE);
        }

		if (col.collider == table1)
        {
			outcome = rallyState.MakeStep(game.DoesPlayer1Serve() ? RallyState.eRallyStateMachineAction.RSMA_ATT_TABLE : RallyState.eRallyStateMachineAction.RSMA_DEF_TABLE);
        }

        if (col.collider == racket0)
        {
            outcome = rallyState.MakeStep(game.DoesPlayer1Serve() ? RallyState.eRallyStateMachineAction.RSMA_DEF_RACK : RallyState.eRallyStateMachineAction.RSMA_ATT_RACK);
			Events.eRacketHitBall(racket0.gameObject);
        }

		if (col.collider == racket1)
        {
			outcome = rallyState.MakeStep(game.DoesPlayer1Serve() ? RallyState.eRallyStateMachineAction.RSMA_ATT_RACK : RallyState.eRallyStateMachineAction.RSMA_DEF_RACK);
			Events.eRacketHitBall(racket1.gameObject);
        }

		if (col.collider == floor)
        {
			outcome = rallyState.MakeStep(RallyState.eRallyStateMachineAction.RSMA_OUT);
        }

		if (outcome == RallyState.eRallyOutcome.RO_NONE)
			return;

		game.player1WonAPoint = game.DoesPlayer1Serve() ^ (outcome == RallyState.eRallyOutcome.RO_DEF_WINS);
		Events.eRallyEnded();
	}

	void OnEvent_rallyEnded()
	{
		
	}

}
