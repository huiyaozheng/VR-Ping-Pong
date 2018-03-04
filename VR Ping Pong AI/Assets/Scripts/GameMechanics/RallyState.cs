using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A state machine that records the current state of the rally and take appropriate actions.
/// </summary>
public class RallyState : MonoBehaviour
{
    /*
     * Attacker is the player who served in this rally.
     * ATT_RACK is the event of his racket hitting the ball.
     * ATT_TAB is the event of his half of the table hitting the ball.
     * OUT is the event of the ball going out of play (hitting the floor or walls).
     */
    public GameState game;

    public enum eRallyStateMachineAction
    {
        RSMA_ATT_RACK,
        RSMA_ATT_TABLE,
        RSMA_DEF_RACK,
        RSMA_DEF_TABLE,
        RSMA_OUT
    };

    public enum eRallyOutcome
    {
        RO_ATT_WINS,
        RO_DEF_WINS,
        RO_NONE
    };

    private enum eRallyState
    {
        S0, // Bounce on the server's side of table
        S1, // Bounce on the defender's side of table
        S2, // Bounce on the defender's racket
        S3, // Bounce on the server's side of table
        S4  // Bounce on the server's racket
    };

    private eRallyState currState = eRallyState.S0;

    private void OnEvent_rallyEnded()
    {
        currState = eRallyState.S0;
    }

    public PPAimAgent trainee;

    private void Reward(float reward)
    {
        trainee.reward += trainee.multiplier * reward;
        trainee.multiplier = 1f;
    }

    private void Reward(float reward, bool reset)
    {
        trainee.reward = reward;
        trainee.multiplier = 1f;
    }

    public eRallyOutcome MakeStep(eRallyStateMachineAction rsma)
    {
        trainee.multiplier *= trainee.tickRate;

        bool botIsAttacker = game.DoesPlayer1Serve();

        switch (currState)
        {
            case eRallyState.S0:
                switch (rsma)
                {
                    case eRallyStateMachineAction.RSMA_ATT_TABLE:
                        currState = eRallyState.S1;
                        return eRallyOutcome.RO_NONE;
                    case eRallyStateMachineAction.RSMA_DEF_RACK:
                        if (botIsAttacker)
                        {
                            Reward(0.6f);
                        }
                        return eRallyOutcome.RO_ATT_WINS;
                    default:
                        if (!botIsAttacker)
                        {
                            Reward(0.6f);
                        }
                        return eRallyOutcome.RO_DEF_WINS;
                }

            case eRallyState.S1:
                switch (rsma)
                {
                    case eRallyStateMachineAction.RSMA_DEF_TABLE:
                        currState = eRallyState.S2;
                        return eRallyOutcome.RO_NONE;
                    case eRallyStateMachineAction.RSMA_DEF_RACK:
                        if (botIsAttacker)
                        {
                            Reward(0.6f);
                        }
                        return eRallyOutcome.RO_ATT_WINS;
                    default:
                        if (botIsAttacker)
                        {
                            Reward(-0.3f, true);
                        }
                        if (!botIsAttacker)
                        {
                            Reward(0.6f);
                        }
                        return eRallyOutcome.RO_DEF_WINS;
                }

            case eRallyState.S2:
                switch (rsma)
                {
                    case eRallyStateMachineAction.RSMA_DEF_RACK:
                        currState = eRallyState.S3;
                        return eRallyOutcome.RO_NONE;
                    case eRallyStateMachineAction.RSMA_ATT_RACK:
                        if (!botIsAttacker)
                        {
                            Reward(0.6f);
                        }
                        return eRallyOutcome.RO_DEF_WINS;
                    default:
                        if (botIsAttacker)
                        {
                            Reward(1.0f);
                        }
                        return eRallyOutcome.RO_ATT_WINS;
                }

            case eRallyState.S3:
                switch (rsma)
                {
                    case eRallyStateMachineAction.RSMA_ATT_TABLE:
                        currState = eRallyState.S4;
                        return eRallyOutcome.RO_NONE;
                    case eRallyStateMachineAction.RSMA_ATT_RACK:
                        if (!botIsAttacker)
                        {
                            Reward(0.6f);
                        }
                        return eRallyOutcome.RO_DEF_WINS;
                    default:
                        if (!botIsAttacker)
                        {
                            Reward(-0.3f, true);
                        }
                        return eRallyOutcome.RO_ATT_WINS;
                }

            case eRallyState.S4:
                switch (rsma)
                {
                    case eRallyStateMachineAction.RSMA_ATT_RACK:
                        currState = eRallyState.S1;
                        return eRallyOutcome.RO_NONE;
                    case eRallyStateMachineAction.RSMA_DEF_RACK:
                        if (botIsAttacker)
                        {
                            Reward(0.6f);
                        }
                        return eRallyOutcome.RO_ATT_WINS;
                    default:
                        if (!botIsAttacker)
                        {
                            Reward(1.0f);
                        }
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