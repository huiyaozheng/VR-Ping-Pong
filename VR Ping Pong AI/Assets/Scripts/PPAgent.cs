﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PPAgent : Agent
{
    [Header("Specific to Tennis")]
    public GameObject ball;
    public bool invertX;
    public float invertMult;
    public int score;
    //public GameObject scoreText;

	// TODO - adapt all these methods to our game

    public override List<float> CollectState()
    {
        Debug.Log("Collect");
        List<float> state = new List<float>();
        state.Add(invertMult * gameObject.transform.position.x);
        Debug.Log(gameObject);
        Debug.Log(GetComponent<Rigidbody>());
        //state.Add(gameObject.transform.position.y);
        //state.Add(invertMult * gameObject.GetComponent<Rigidbody>().velocity.x);
        //state.Add(gameObject.GetComponent<Rigidbody>().velocity.y);

        //state.Add(invertMult * ball.transform.position.x);
        //state.Add(ball.transform.position.y);
        //state.Add(invertMult * ball.GetComponent<Rigidbody>().velocity.x);
        //state.Add(ball.GetComponent<Rigidbody>().velocity.y);
        Debug.Log($"ball is {ball.transform.position.x} {ball.transform.position.y}");
        Debug.Log($"racket is {gameObject.transform.position.x} {gameObject.transform.position.y}");
        //Application.Quit();
        return state;
    }

    // to be implemented by the developer
    public override void AgentStep(float[] act)
    {
        Debug.Log("Step is: "+act[0]);
        int action = Mathf.FloorToInt(act[0]);
        if (action == 0)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            //moveY = 0.0;
        }
        if (action == 1)
        {
            GetComponent<Rigidbody>().position += new Vector3(0f, 0f, 0.1f);
            //GetComponent<Rigidbody>().velocity += new Vector3(0f,0.1f,0f);

            //moveY = 0.1;
        }
        if (action == 2)
        {
            GetComponent<Rigidbody>().position -= new Vector3(0f, 0f, 0.1f);
            //moveY = -0.1;
        }
        //float moveX = 0.0f;
        //float moveY = 0.0f;
        //if (action == 5)
        //{
        //    //MODIFIED
        //    moveX = invertMult * 0.25f;
        //}
        //if (action == 1)
        //{
        //    //MODIFIED
        //    moveX = invertMult * -0.25f;
        //}
        //if (action == 2 && gameObject.transform.position.y + transform.parent.transform.position.y < -1.5f)
        //{
        //    moveY = 0.5f;
        //    gameObject.GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, moveY * 12f, 0f);
        //}
        //if (action == 3)
        //{
        //    GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x * 0.5f, GetComponent<Rigidbody>().velocity.y, 0f);
        //    moveY = 0f;
        //    moveX = 0f;
        //}

        //gameObject.GetComponent<Rigidbody>().velocity = new Vector3(moveX * 50f, GetComponent<Rigidbody>().velocity.y, 0f);


        //if (invertX)
        //{
        //    if (gameObject.transform.position.x + transform.parent.transform.position.x < -(invertMult) * 1f)
        //    {
        //        gameObject.transform.position = new Vector3(-(invertMult) * 1f + transform.parent.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        //    }
        //}
        //else
        //{
        //    if (gameObject.transform.position.x + transform.parent.transform.position.x > -(invertMult) * 1f)
        //    {
        //        gameObject.transform.position = new Vector3(-(invertMult) * 1f + transform.parent.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        //    }
        //}

        //scoreText.GetComponent<Text>().text = score.ToString();
    }

    // to be implemented by the developer
    public override void AgentReset()
    {
        Debug.Log("AgentReset");
        if (invertX)
        {
            invertMult = -1f;
        }
        else
        {
            invertMult = 1f;
        }

        Debug.Log("RESETTTTT");
        //HARCODED
        gameObject.transform.position = new Vector3(2.13f, -1.9f, 15.73f);// + transform.parent.transform.position;
        gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
    }
}
