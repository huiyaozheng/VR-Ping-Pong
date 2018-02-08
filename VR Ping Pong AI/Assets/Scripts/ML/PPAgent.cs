using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PPAgent : Agent
{
    [Header("Specific to Ping Pong")]
	public Rigidbody ball;
    public bool invertXZ;

	private float invertXZMult;
	private Vector3 defaultRacketPos;
	private Quaternion defaultRacketRot;

	void Start()
	{
		invertXZMult = invertXZ ? -1 : 1;
		defaultRacketPos = gameObject.transform.position;
		defaultRacketRot = gameObject.transform.rotation;
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
		Vector3 requestedPos = new Vector3(act[0], act[1], act[2]);
		Vector3 requestedRot = new Vector3(act[3], act[4], act[5]);

		// TODO: 
		// Make public variables maxSpeed and maxAngularSpeed so that you can set them from editor.
		// Set new position based on the requests as well as maxSpeed and maxAngularSpeed.

		// Hmm... Perhaps we'll have to be smart here and measure time since the last update or something.
		// Because to use the max velocity limit, we need to know over what time the movement takes place.
		// If there's gonna be a lot of time between the actions, then we'll have to smooth out the motion of the bat,
		// not just have it jump to the new position.

		// So perhaps for now ignore the maxSpeed and maxAngSpeed limits and just teleport the bat! We'll see how this turns out :)
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
