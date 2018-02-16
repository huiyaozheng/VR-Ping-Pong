using UnityEngine;
using PhysicsLibrary;


public class CatcherBot : Catcher
{
    public Agent trainee;
    protected override void hit()
    {

        ball.velocity = PhysicsCalculations.velFromTraj(landPos, ball.position, maxTrajectoryHeight, Physics.gravity.magnitude);
        Vector3 dist = (landPos-opponentRacket.transform.position);


        ////TODO magic numbers FIXME
        //float disst = dist.sqrMagnitude/10.0f;
        //Debug.Log("DISTANCE TO OPP." + disst);
        //trainee.reward += (Mathf.Exp(dist.sqrMagnitude-100.0f)/(1+Mathf.Exp(dist.sqrMagnitude-100.0f)))*0.03f;
    }
    protected override void OnCollisionEnter(Collision col)
    {
        if (col.gameObject == ball.gameObject)
        {
            //myRacket.GetComponent<PPAimAgent>().reward += 0.01f;
            //Never delete until the very end. Comment out!
            // //TODO: delete this block after hooked to agent
//             float x = (opponentTable.transform.parent.gameObject.transform.localScale.x) / 2 - 0.5f;
//             float z = (opponentTable.transform.parent.gameObject.transform.localScale.z) / 2 - 0.5f;
//             Debug.Log("OTABLEx " + x);
//             Debug.Log("OTABLEz " + z);
//             x = Random.Range(-x, x);
//             z = Random.Range(2, z);
//             setTargets(new Vector3(x, 0, z) * (invertXZ ? -1f : 1f), maxTrajectoryHeight);
             // end of block

            // Return the ball to the location as specifed by the agent.
            hit();

            // Move the racket back to the default position.
            myRacket.velocity = (myDefPos - myRacket.transform.position).normalized * maxRacketMovingSpeed * 0.2f;
            tracking = false;
        }
    }
}