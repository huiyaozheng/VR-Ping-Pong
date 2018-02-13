using UnityEngine;
using PhysicsLibrary;


public class CatcherBot : Catcher
{
    protected override void OnCollisionEnter(Collision col)
    {
        if (col.gameObject == ball.gameObject)
        {
            //TODO: delete this block after hooked to agent
            float x = (opponentTable.transform.parent.gameObject.transform.localScale.x) / 2 - 0.5f;
            float z = (opponentTable.transform.parent.gameObject.transform.localScale.z) / 2 - 0.5f;
            x = Random.Range(-x, x);
            z = Random.Range(2, z);
            setTargets(new Vector3(x, 0, z) * (invertXZ ? -1f : 1f), maxTrajectoryHeight);
            // end of block

            // Return the ball to the location as specifed by the agent.
            hit();
            // After hitting the ball, stop the racket.
            myRacket.velocity = new Vector3(0, 0, 0);
            tracking = false;
        }
    }
}