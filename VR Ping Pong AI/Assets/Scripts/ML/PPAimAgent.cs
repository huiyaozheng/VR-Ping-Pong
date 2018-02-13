using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PPAimAgent : Agent
{
    [Header("Specific to Ping Pong")]
	public Rigidbody ball;

    /// <summary>
    /// The opponent's racket.
    /// </summary>
    public Rigidbody opponentRacket;

    public bool invertXZ;
    public float[] min = new float[3];
    public float[] max = new float[3];
    //public float minX, maxX, minY, maxY, minZ, maxZ;
    //private Queue<Collision> lastCollisions;

    //FIXME temporary fix for the catcher;
	public Vector3 defaultRacketPos;
	private Quaternion defaultRacketRot;

	void Start()
	{
        //Debug.Log("START CALLED");
        //min[0]=-gameObject.GetComponent<CatcherBot>().opponentTable.transform.parent.gameObject.transform.localScale.x;
        //max[0] = -min[0];
        //min[1]=-gameObject.GetComponent<CatcherBot>().opponentTable.transform.parent.gameObject.transform.localScale.z;
        //max[1] = -min[1];
        //min[2] = 0; 
        //max[2] = gameObject.GetComponent<CatcherBot>().maxTrajectoryHeight;
		defaultRacketPos = gameObject.transform.position;
		defaultRacketRot = gameObject.transform.rotation;
	}

    public override List<float> CollectState()
    {
        List<float> state=new List<float>();
        //My racket (not opponents) position
        state.Add((invertXZ ? -1 : 1 )*gameObject.transform.position.x);
		state.Add(                     gameObject.transform.position.y);
		state.Add((invertXZ ? -1 : 1 )*gameObject.transform.position.z);

        //Opponent's racket position
        state.Add((invertXZ ? -1 : 1 )*opponentRacket.transform.position.x);
		state.Add(                     opponentRacket.transform.position.y);
		state.Add((invertXZ ? -1 : 1 )*opponentRacket.transform.position.z);


        //Let's discuss that!
        //
        //It might be a good idea to be able keep track of the last X collisions with the 
        //opponents racket (i.e. also return them as state but to be decided how to convert
        //information to float so that the brain infers to pick a point that is hard to hit.
        //Therefore I leave a queue<collision> on top as public that the collider may need
        //to alter.
        //
        //Do you think there would be a better way to do that?
        return state;
    }

    // to be implemented by the developer
    public override void AgentStep(float[] act)
    {
        //Debug.Log("act0122 " + act[0] + " " + act[1]+ " " + act[2]);
        for(int i = 0; i < 3; i++)
        {
            act[i] = (Mathf.Exp(act[i]) / (1 + Mathf.Exp(act[i]))) * (max[i] - min[i]) + min[i];
        }
        gameObject.GetComponent<Catcher>().setTargets(new Vector3(act[0], 0, act[1]),gameObject.transform.position.y+act[2]);
    }

    // to be implemented by the developer
    public override void AgentReset()
    {
        reward = 0;
    }
}
