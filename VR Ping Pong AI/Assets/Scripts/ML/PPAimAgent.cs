using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ML-agent Agent for aimming.
/// </summary>
public class PPAimAgent : Agent
{
    [Header("Specific to Ping Pong")] public Rigidbody ball;

    public Rigidbody opponentRacket;

    public bool invertXZ;
    public float[] min = new float[3];
    public float[] max = new float[3];
    public float tickRate = 0.99f;
    public float multiplier = 1.0f;

    public Vector3 defaultRacketPos;

    private Quaternion defaultRacketRot;

    private float expo(float x)
    {
        return Mathf.Exp(x) / (1 + Mathf.Exp(x));
    }

    void Start()
    {
        defaultRacketPos = gameObject.transform.position;
        defaultRacketRot = gameObject.transform.rotation;
        if (System.Double.IsNaN(reward) == true)
            reward = 0.0f;
    }

    public override List<float> CollectState()
    {
        List<float> state = new List<float>();
        //My racket (not opponents) position
        state.Add((invertXZ ? -1 : 1) * gameObject.transform.position.x);
        state.Add(gameObject.transform.position.y);
        state.Add((invertXZ ? -1 : 1) * gameObject.transform.position.z);

        //Opponent's racket position
        state.Add((invertXZ ? -1 : 1) * opponentRacket.transform.position.x);
        state.Add(opponentRacket.transform.position.y);
        state.Add((invertXZ ? -1 : 1) * opponentRacket.transform.position.z);

        //ball's position
        state.Add(ball.transform.position.x);
        state.Add(ball.transform.position.y);
        state.Add(ball.transform.position.z);

        return state;
    }

    public override void AgentStep(float[] act)
    {
        for (int i = 0; i < 3; i++)
        {
            act[i] = (Mathf.Exp(act[i]) / (1 + Mathf.Exp(act[i]))) * (max[i] - min[i]) + min[i];
        }
        gameObject.GetComponent<Catcher>().setTargets(new Vector3(act[0], 0, act[1]), act[2]);
        float dist = Mathf.Abs(act[0] - opponentRacket.transform.position.x);
        reward += expo(dist - 2.40f) * 0.02f * multiplier;
    }

    public override void AgentReset()
    {
    }
}