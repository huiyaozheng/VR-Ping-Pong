using UnityEngine;
using PhysicsLibrary;


public class CatcherBot : Catcher
{
    public Agent trainee;
    public const float mult = 2.0f;
    protected override void hit()
    {

        ball.velocity = PhysicsCalculations.velFromTraj(landPos, ball.position, maxTrajectoryHeight, Physics.gravity.magnitude, false);
        //Vector3 dist = (landPos-opponentRacket.transform.position);


        ////TODO magic numbers FIXME
        float disst = Mathf.Abs(landPos.x-opponentRacket.transform.position.x);
        //Debug.Log("DISTANCE TO OPP. " + disst);
        //Debug.Log("Reward for DISTANCE TO OPP. " + (Mathf.Exp(disst-6f)/(1+Mathf.Exp(disst-6f)))*0.20f);
        //Debug.Log("max height " + maxTrajectoryHeight);
        trainee.reward += (Mathf.Exp(disst-6f)/(1+Mathf.Exp(disst-6f)))*0.20f;
        //Debug.Log((Mathf.Exp(mult*(3.5f-maxTrajectoryHeight)) / (1 + Mathf.Exp(mult*(3.5f-maxTrajectoryHeight)))) * 0.25f-0.1f)
        trainee.reward+=(Mathf.Exp(mult*(5f-maxTrajectoryHeight)) / (1 + Mathf.Exp(mult*(5f-maxTrajectoryHeight)))) * 0.25f;
        //trainee.reward += (Mathf.Exp(disst - 0.5f) / (1 + Mathf.Exp(disst - 0.5f))) * 0.20f;
        //Debug.Log((Mathf.Exp(mult * (5f - maxTrajectoryHeight)) / (1 + Mathf.Exp(mult * (5f - maxTrajectoryHeight)))));
        //Debug.Assert(!System.Double.IsNaN(trainee.reward));
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