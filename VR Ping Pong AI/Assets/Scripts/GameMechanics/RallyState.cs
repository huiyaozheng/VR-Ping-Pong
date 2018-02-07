using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RallyState : MonoBehaviour {

	/*
	 * Attacker is the player who served in this rally.
	 * ATT_RACK is the event of his racket hitting the ball.
	 * ATT_TAB is the event of his half of the table hitting the ball.
	 * OUT is the event of the ball going out of play (hitting the floor or walls).
	 */
	public GameState game;

	public enum eRallyStateMachineAction{ RSMA_ATT_RACK, RSMA_ATT_TABLE, RSMA_DEF_RACK, RSMA_DEF_TABLE, RSMA_OUT };

	public enum eRallyOutcome{ RO_ATT_WINS, RO_DEF_WINS, RO_NONE };

	private enum eRallyState{ S0, S1, S2, S3, S4 };

	private eRallyState currState = eRallyState.S4; // To bypass serving for now...

	private void OnEvent_rallyEnded()
	{
        //Debug.Log("STate is"+currState);
        //game.GetComponent<GameState>().ResetBall();
		currState = eRallyState.S0;
	}
		
	// I drew out a state machine to control a game, including serves, and implemented it below:

	public eRallyOutcome MakeStep(eRallyStateMachineAction rsma)
	{
		// Debug:
		switch (rsma)
		{
		case eRallyStateMachineAction.RSMA_ATT_RACK:
			Debug.Log("Attacker racket.");
			break;
		case eRallyStateMachineAction.RSMA_DEF_RACK:
			Debug.Log("Defender racket.");
			break;
		case eRallyStateMachineAction.RSMA_ATT_TABLE:
			Debug.Log("Attacker's half of the table.");
			break;
		case eRallyStateMachineAction.RSMA_DEF_TABLE:
			Debug.Log("Defender's half of the table.");
			break;
		case eRallyStateMachineAction.RSMA_OUT:
			Debug.Log("Ball went out.");
			break;
		}


		switch(currState)
		{
		case eRallyState.S0:
			switch (rsma)
			{
			case eRallyStateMachineAction.RSMA_ATT_TABLE:
				currState = eRallyState.S1;
                Debug.Log("Returned S0");
				return eRallyOutcome.RO_NONE;
			case eRallyStateMachineAction.RSMA_DEF_RACK:
                Debug.Log("Returned S0");
				return eRallyOutcome.RO_ATT_WINS;
			default:
                Debug.Log("Returned S0");
				return eRallyOutcome.RO_DEF_WINS;
			}
		case eRallyState.S1:
			switch (rsma)
			{
			case eRallyStateMachineAction.RSMA_DEF_TABLE:
				currState = eRallyState.S2;
                Debug.Log("Returned S1");
				return eRallyOutcome.RO_NONE;
			case eRallyStateMachineAction.RSMA_DEF_RACK:
                Debug.Log("Returned S1");
				return eRallyOutcome.RO_ATT_WINS;
			default:
                Debug.Log("Returned S1");
				return eRallyOutcome.RO_DEF_WINS;
			}
		case eRallyState.S2:
			switch (rsma)
			{
			case eRallyStateMachineAction.RSMA_DEF_RACK:
				currState = eRallyState.S3;
                Debug.Log("Returned S2");
				return eRallyOutcome.RO_NONE;
			case eRallyStateMachineAction.RSMA_ATT_RACK:
                Debug.Log("Returned S2");
				return eRallyOutcome.RO_DEF_WINS;
			default:
                Debug.Log("Returned S2");
				return eRallyOutcome.RO_ATT_WINS;
			}
		case eRallyState.S3:
			switch (rsma)
			{
			case eRallyStateMachineAction.RSMA_ATT_TABLE:
				currState = eRallyState.S4;
                Debug.Log("Returned S3");
				return eRallyOutcome.RO_NONE;
			case eRallyStateMachineAction.RSMA_ATT_RACK:
                Debug.Log("Returned S3");
				return eRallyOutcome.RO_DEF_WINS;
			default:
                Debug.Log("Returned S3");
				return eRallyOutcome.RO_ATT_WINS;
			}
		case eRallyState.S4:
			switch (rsma)
			{
			case eRallyStateMachineAction.RSMA_ATT_RACK:
				currState = eRallyState.S1;
                Debug.Log("Returned S4");
				return eRallyOutcome.RO_NONE;
			case eRallyStateMachineAction.RSMA_DEF_RACK:
                Debug.Log("Returned S4");
				return eRallyOutcome.RO_ATT_WINS;
			default:
                Debug.Log("Returned S4");
				return eRallyOutcome.RO_DEF_WINS;
			}
		default:
			return eRallyOutcome.RO_NONE; // this code should not be reached
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
