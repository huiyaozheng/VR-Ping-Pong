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
    public bool ballHasHitOnce=false;
	//public float minX, maxX, minY, maxY, minZ, maxZ;

    //DOBO: I did that (turning these public) so after each shot the racket is placed back, but it
    //may turn out it's better to reset it on academy reset;
	public Vector3 defaultRacketPos;
	public Quaternion defaultRacketRot;

	private float invertXZMult;
    private Vector2[] last2 = new Vector2[2];
    private Vector2 lastBatPos;
    //private float midX, midY, midZ;

	void Start()
	{
		invertXZMult = invertXZ ? -1 : 1;
		defaultRacketPos = gameObject.transform.position;
		defaultRacketRot = gameObject.transform.rotation;
        last2[0] = last2[1] = new Vector2(0,0);
        //midX = (maxX - minX) / 2.0f + minX;
        //midY = (maxY - minY) / 2.0f + minY;
        //midZ = (maxZ - minZ) / 2.0f + minZ;
	}

    public override List<float> CollectState()
    {
        List<float> state = new List<float>();

		// Racket pos:
        //state.Add(invertXZMult * gameObject.transform.position.x);
		state.Add(               gameObject.transform.position.y);
		state.Add(invertXZMult * gameObject.transform.position.z);

        // Racket rot:
        //state.Add(invertXZMult * gameObject.transform.rotation.eulerAngles.x);
        //state.Add(               gameObject.transform.rotation.eulerAngles.y);
        //state.Add(invertXZMult * gameObject.transform.rotation.eulerAngles.z);

        // Ball pos:
        //state.Add(invertXZMult * ball.transform.position.x);
        float con = 5.0f;
        //Debug.Log(Time.deltaTime);
        //Debug.Log(ball.transform.position.y + " " + ball.transform.position.z);
        //Debug.Log((ball.transform.position.y+ball.velocity.y*con*Time.deltaTime) + " " + (ball.transform.position.z+ball.velocity.z*con*Time.deltaTime));

		state.Add(               ball.transform.position.y+ball.velocity.y*con*Time.deltaTime);
		state.Add(invertXZMult * ball.transform.position.z+ball.velocity.z*con*Time.deltaTime);
        for (int i = 0; i < 2; i++)
        {
            state.Add(last2[i].x);
            state.Add(last2[i].y);
        }
        last2[0] = last2[1];
        last2[1] = new Vector2(ball.transform.position.y+ball.velocity.y*con*Time.deltaTime, ball.transform.position.z+ball.velocity.z*con*Time.deltaTime);

		// Ball vel:
		//state.Add(invertXZMult * ball.velocity.x);
		//state.Add(               ball.velocity.y);
		//state.Add(invertXZMult * ball.velocity.z);

        return state;
    }

    // to be implemented by the developer
    public override void AgentStep(float[] act)
    {
        //Debug.Log("POS act: " + 0.0 + " " + act[0] + " " + act[1]);
        //Debug.Log("ROT act: " + act[3] + " " + act[4] + " " + act[5]);
        //Debug.Assert(act[2] == 15.0f);
        //NOTE:
        //exp(x)/(1+exp(x)) always gives number between 0 and 1 and 0.5 at x=0
        //1*(maxX-minX)+minX=maxX
        //0*byTheSameThingIs=minX
        float cons = 1.0f;
        float eps = 5.0f;
        for(int i = 0; i < 2; i++)
        {
            //FIXME for training with X=0.0
            act[i] = (Mathf.Exp(act[i]*cons) / (1.0f + Mathf.Exp(act[i]*cons))) * (max[i+1] - min[i+1]) + min[i+1];
        }
        Vector2 cPos = new Vector2(act[0], act[1]);
        Vector2 offset = cPos - last2[1];
        //Debug.Log(cPos + " " + last2[1]);
        //Debug.Log(Vector2.SqrMagnitude(offset));
        if (Vector2.SqrMagnitude(offset) < eps)
        {
            if (ballHasHitOnce == true) reward += 0.2f;
            else reward -= 0.01f;
            //Debug.Log("HIT ONCE?: "+ballHasHitOnce);
            
            //Debug.Log("CLOSE enough");
        }
        //if(lastBatPos.x!=act[0] && lastBatPos.y != act[1])
        //{
        //    reward += 0.01f;
        //}
        //lastBatPos.x = act[0];
        //lastBatPos.y = act[1];
        //Debug.Log("act clamped: " + 0.0 + " " + act[0] + " " + act[1]);

        //act[0] = (Mathf.Exp(act[0]) / (1 + Mathf.Exp(act[0]))) * (maxX - minX) + minX;
        //act[1] =( Mathf.Exp(act[1]) / (1 + Mathf.Exp(act[1]))) * (maxY - minY) + minY;
        //act[2] =( Mathf.Exp(act[2]) / (1 + Mathf.Exp(act[2]))) * (maxZ - minZ) + minZ;
        //Debug.Assert(act[0] >= minX && act[0] <= maxX);


        //Debug.Log("act012: 0.0 " + act[0] + " " + act[1] );
		Vector3 requestedPos = new Vector3(0.0f, act[0], act[1]);
		//Vector3 requestedRot = new Vector3(act[3], act[4], act[5]);

        // TODO: 
        // Make public variables maxSpeed and maxAngularSpeed so that you can set them from editor.
        // Set new position based on the requests as well as maxSpeed and maxAngularSpeed.

        // Hmm... Perhaps we'll have to be smart here and measure time since the last update or something.
        // Because to use the max velocity limit, we need to know over what time the movement takes place.
        // If there's gonna be a lot of time between the actions, then we'll have to smooth out the motion of the bat,
        // not just have it jump to the new position.

        // So perhaps for now ignore the maxSpeed and maxAngSpeed limits and just teleport the bat! We'll see how this turns out :)

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
