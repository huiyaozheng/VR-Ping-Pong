using UnityEngine;
using PhysicsLibrary;


public class CatcherBot : Catcher
{
    public PPAimAgent trainee;
    public const float mult = 10.0f;
    public Logger logger;
    
    private float expo(float x)
    {
        return Mathf.Exp(x) / (1 + Mathf.Exp(x));
    }
    protected override void hit()
    {

        ball.velocity = PhysicsCalculations.velFromTraj(landPos, ball.position, maxTrajectoryHeight, Physics.gravity.magnitude, false);
        //Vector3 dist = (landPos-opponentRacket.transform.position);


        ////TODO magic numbers FIXME
        float dist = Mathf.Abs(landPos.x-opponentRacket.transform.position.x);
        //Debug.Log("DISTANCE TO OPP. " + dist);
        //Debug.Log("Reward for DISTANCE TO OPP. " + (Mathf.Exp(dist-6f)/(1+Mathf.Exp(dist-6f)))*0.20f);
        //Debug.Log("max height " + maxTrajectoryHeight);
        trainee.reward += expo(dist - 7f) * 0.35f * trainee.multiplier;
        logger.AppendLog(gameObject.transform.position,
                         opponentRacket.transform.position,
                         ball.transform.position,
                         expo(dist - 7f) * 0.01f * trainee.multiplier);
        //Debug.Log(expo((dist - 7f)) * 0.40f * trainee.multiplier);
        trainee.reward += expo(mult * (3f - maxTrajectoryHeight)) * 0.15f * trainee.multiplier;
        logger.AppendLog(gameObject.transform.position,
                         opponentRacket.transform.position,
                         ball.transform.position,
                         expo(mult * (3f - maxTrajectoryHeight)) * 0.15f * trainee.multiplier);
        //Debug.Log(expo(mult*(3f-maxTrajectoryHeight)));
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