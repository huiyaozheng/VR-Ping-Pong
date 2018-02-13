using UnityEngine;
using PhysicsLibrary;


public class CatcherBot : Catcher
{
    protected override void hit()
    {

        ball.velocity = PhysicsCalculations.velFromTraj(landPos, ball.position, maxTrajectoryHeight, Physics.gravity.magnitude);
        myRacket.transform.position = new Vector3(myRacket.transform.position.x,
                                                 myRacket.transform.position.y,
                                                 myDefPos.z);
        Vector3 dist = (landPos-opponentRacket.transform.position);
        //Debug.Log("DISTANCE TO OPP." + dist.sqrMagnitude);
        //TODO magic number FIXME
        myRacket.GetComponent<PPAimAgent>().reward += (Mathf.Exp(dist.sqrMagnitude-100.0f)/(1+Mathf.Exp(dist.sqrMagnitude-100.0f)))*0.1f;
    }
    protected override void OnCollisionEnter(Collision col)
    {
        if (col.gameObject == ball.gameObject)
        {
            myRacket.GetComponent<PPAimAgent>().reward += 0.01f;
            //Never delete until the very end. Comment out!
            // //TODO: delete this block after hooked to agent
            // float x = (opponentTable.transform.parent.gameObject.transform.localScale.x) / 2 - 0.5f;
            // float z = (opponentTable.transform.parent.gameObject.transform.localScale.z) / 2 - 0.5f;
            // Debug.Log("OTABLEx " + x);
            // Debug.Log("OTABLEz " + z);
            // x = Random.Range(-x, x);
            // z = Random.Range(2, z);
            // setTargets(new Vector3(x, 0, z) * (invertXZ ? -1f : 1f), maxTrajectoryHeight);
            // // end of block

            // Return the ball to the location as specifed by the agent.
            hit();
            // After hitting the ball, stop the racket.
            myRacket.velocity = new Vector3(0, 0, 0);
            tracking = false;
        }
    }
}