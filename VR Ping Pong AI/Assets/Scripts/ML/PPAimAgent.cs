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
    public float minX, maxX, minY, maxY, minZ, maxZ;
    private Queue<Collision> lastCollisions;

	private Vector3 defaultRacketPos;
	private Quaternion defaultRacketRot;

	void Start()
	{
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

        //TODO to be addet at a later stage when simple training is verified
        // //My racket (not opponents) rotation 
        // state.Add((invertXZ ? -1 : 1 )*gameObject.transform.rotation.eulerAngles.x);
		// state.Add(                     gameObject.transform.rotation.eulerAngles.y);
		// state.Add((invertXZ ? -1 : 1 )*gameObject.transform.rotation.eulerAngles.z);

        //Opponent's racket position
        state.Add((invertXZ ? -1 : 1 )*gameObject.transform.position.x);
		state.Add(                     gameObject.transform.position.y);
		state.Add((invertXZ ? -1 : 1 )*gameObject.transform.position.z);

        //TODO to be addet at a later stage when simple training is verified
        // //Opponent's racket rotation 
        // state.Add((invertXZ ? -1 : 1 )*gameObject.transform.rotation.eulerAngles.x);
        // state.Add(                     gameObject.transform.rotation.eulerAngles.y);
        // state.Add((invertXZ ? -1 : 1 )*gameObject.transform.rotation.eulerAngles.z);

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
        //Ad-hoc 
        //to be experimented
        //An issue we will experience is that agentStep is called much more frequently than we use it.
        //using it less frequently on the other side will require LAAARGE training
        //(Recall training with FramesToSkip set to sth positive)
        gameObject.GetComponent<Catcher>().setTargets(new Vector3(act[0], act[1], act[2]), act[3]);
    }

    // to be implemented by the developer
    public override void AgentReset()
    {
        //Need to test how often academy resets and if
        //that will cause problems if it's affecting our score
		//gameObject.transform.position = defaultRacketPos;
		//gameObject.transform.rotation = defaultRacketRot;
    }
}
