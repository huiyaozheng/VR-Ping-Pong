using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PPAgent : Agent
{
    [Header("Specific to Ping Pong")]
	public Rigidbody ball;
    public bool invertXZ;
    public float[] min = new float[3];
    public float[] max = new float[3];
	public float minX, maxX, minY, maxY, minZ, maxZ;

	public Vector3 defaultRacketPos;
	public Quaternion defaultRacketRot;

	private float invertXZMult;
    //private float midX, midY, midZ;

	void Start()
	{
		invertXZMult = invertXZ ? -1 : 1;
		defaultRacketPos = gameObject.transform.position;
		defaultRacketRot = gameObject.transform.rotation;
        //midX = (maxX - minX) / 2.0f + minX;
        //midY = (maxY - minY) / 2.0f + minY;
        //midZ = (maxZ - minZ) / 2.0f + minZ;
	}

    public override List<float> CollectState()
    {
        List<float> state = new List<float>();

		// Racket pos:
        state.Add(invertXZMult * gameObject.transform.position.x);
		state.Add(               gameObject.transform.position.y);
		state.Add(invertXZMult * gameObject.transform.position.z);

		// Racket rot:
		state.Add(invertXZMult * gameObject.transform.rotation.eulerAngles.x);
		state.Add(               gameObject.transform.rotation.eulerAngles.y);
		state.Add(invertXZMult * gameObject.transform.rotation.eulerAngles.z);

		// Ball pos:
		state.Add(invertXZMult * ball.transform.position.x);
		state.Add(               ball.transform.position.y);
		state.Add(invertXZMult * ball.transform.position.z);

		// Ball vel:
		state.Add(invertXZMult * ball.velocity.x);
		state.Add(               ball.velocity.y);
		state.Add(invertXZMult * ball.velocity.z);

        return state;
    }

    // to be implemented by the developer
    public override void AgentStep(float[] act)
    {
        Debug.Log("POS act: " + act[0] + " " + act[1] + " " + act[2]);
        //Debug.Log("ROT act: " + act[3] + " " + act[4] + " " + act[5]);
        //Debug.Assert(act[2] == 15.0f);
        //NOTE:
        //exp(x)/(1+exp(x)) always gives number between 0 and 1 and 0.5 at x=0
        //1*(maxX-minX)+minX=maxX
        //0*byTheSameThingIs=minX
        for(int i = 0; i < 3; i++)
        {
            act[i] = (Mathf.Exp(act[i]) / (1 + Mathf.Exp(act[i]))) * (max[i] - min[i]) + min[i];
        }

        //act[0] = (Mathf.Exp(act[0]) / (1 + Mathf.Exp(act[0]))) * (maxX - minX) + minX;
        //act[1] =( Mathf.Exp(act[1]) / (1 + Mathf.Exp(act[1]))) * (maxY - minY) + minY;
        //act[2] =( Mathf.Exp(act[2]) / (1 + Mathf.Exp(act[2]))) * (maxZ - minZ) + minZ;
        //Debug.Assert(act[0] >= minX && act[0] <= maxX);


        Debug.Log("act012: " + act[0] + " " + act[1] + " "+act[2]);
		Vector3 requestedPos = new Vector3(act[0], act[1], act[2]);
		//Vector3 requestedRot = new Vector3(act[3], act[4], act[5]);

        // TODO: 
        // Make public variables maxRacketMovingSpeed and maxAngularSpeed so that you can set them from editor.
        // Set new position based on the requests as well as maxRacketMovingSpeed and maxAngularSpeed.

        // Hmm... Perhaps we'll have to be smart here and measure time since the last update or something.
        // Because to use the max velocity limit, we need to know over what time the movement takes place.
        // If there's gonna be a lot of time between the actions, then we'll have to smooth out the motion of the bat,
        // not just have it jump to the new position.

        // So perhaps for now ignore the maxRacketMovingSpeed and maxAngSpeed limits and just teleport the bat! We'll see how this turns out :)

        //DOBO:
        //First train will be with super speed (i.e. just set pos and rot requestedPos and requestedRot)
        //to see if the ml infers at least sth.
        gameObject.transform.position = requestedPos;
        //gameObject.transform.rotation= Quaternion.Euler(requestedRot);
    }

    // to be implemented by the developer
    public override void AgentReset()
    {
		// TODO:
		// Figure out where this is called from and when.
		// Modify this method if necessary.

		gameObject.transform.position = defaultRacketPos;
		gameObject.transform.rotation = defaultRacketRot;
    }
}
