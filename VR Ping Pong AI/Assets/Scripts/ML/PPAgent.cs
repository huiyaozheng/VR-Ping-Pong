using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ML-agent Agent.
/// </summary>
public class PPAgent : Agent
{
    [Header("Specific to Ping Pong")] public Rigidbody ball;
    public bool invertXZ;
    public float[] min = new float[3];
    public float[] max = new float[3];
    public float minX, maxX, minY, maxY, minZ, maxZ;

    public Vector3 defaultRacketPos;
    public Quaternion defaultRacketRot;

    private float invertXZMult;

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
        state.Add(gameObject.transform.position.y);
        state.Add(invertXZMult * gameObject.transform.position.z);

        // Racket rot:
        state.Add(invertXZMult * gameObject.transform.rotation.eulerAngles.x);
        state.Add(gameObject.transform.rotation.eulerAngles.y);
        state.Add(invertXZMult * gameObject.transform.rotation.eulerAngles.z);

        // Ball pos:
        state.Add(invertXZMult * ball.transform.position.x);
        state.Add(ball.transform.position.y);
        state.Add(invertXZMult * ball.transform.position.z);

        // Ball vel:
        state.Add(invertXZMult * ball.velocity.x);
        state.Add(ball.velocity.y);
        state.Add(invertXZMult * ball.velocity.z);

        return state;
    }

    public override void AgentStep(float[] act)
    {
        Debug.Log("POS act: " + act[0] + " " + act[1] + " " + act[2]);

        //NOTE:
        //exp(x)/(1+exp(x)) always gives number between 0 and 1 and 0.5 at x=0
        //1*(maxX-minX)+minX=maxX
        //0*byTheSameThingIs=minX
        for (int i = 0; i < 3; i++)
        {
            act[i] = (Mathf.Exp(act[i]) / (1 + Mathf.Exp(act[i]))) * (max[i] - min[i]) + min[i];
        }

        Debug.Log("act012: " + act[0] + " " + act[1] + " " + act[2]);
        Vector3 requestedPos = new Vector3(act[0], act[1], act[2]);

        gameObject.transform.position = requestedPos;
    }

    public override void AgentReset()
    {
        gameObject.transform.position = defaultRacketPos;
        gameObject.transform.rotation = defaultRacketRot;
    }
}