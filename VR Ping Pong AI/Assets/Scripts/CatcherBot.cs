using UnityEngine;
using PhysicsLibrary;

/// <summary>
/// Based on Catcher, the CatcherBot returns the ball to a position chosen by the trained model.
/// </summary>
public class CatcherBot : Catcher
{
    public PPAimAgent trainee;
    public const float mult = 10.0f;

    protected float expo(float x)
    {
        return Mathf.Exp(x) / (1 + Mathf.Exp(x));
    }

    protected override void hit()
    {
        ball.velocity = PhysicsCalculations.velFromTraj(landPos, ball.position, maxTrajectoryHeight,
            Physics.gravity.magnitude, false);
        float dist = Mathf.Abs(landPos.x - opponentRacket.transform.position.x);
        trainee.reward += expo(dist - 7f) * 0.35f * trainee.multiplier;
        trainee.reward += expo(mult * (3f - maxTrajectoryHeight)) * 0.15f * trainee.multiplier;
    }

    protected override void OnCollisionEnter(Collision col)
    {
        if (col.gameObject == ball.gameObject)
        {
            // Return the ball to the location as specifed by the agent.
            hit();

            // Move the racket back to the default position.
            myRacket.velocity = (myDefPos - myRacket.transform.position).normalized * maxRacketMovingSpeed * 0.2f;
            tracking = false;
        }
    }
}