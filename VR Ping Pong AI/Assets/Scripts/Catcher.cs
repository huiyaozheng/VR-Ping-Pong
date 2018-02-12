using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Catcher : MonoBehaviour
{
    public GameState game;
    public Rigidbody ball;

    /// <summary>
    /// The racket controlled by this script.
    /// </summary>
    public Rigidbody myRacket;

    /// <summary>
    /// The opponent's racket.
    /// </summary>
    public Rigidbody opponentRacket;

    /// <summary>
    /// This side of the table.
    /// </summary>
    public Collider closerTable;

    /// <summary>
    /// Opponent's side of table.
    /// </summary>
    public Collider opponentTable;

    /// <summary>
    /// Z-axis distance between the ball and myRacket. Used to detect whether the ball is flying towards us.
    /// </summary>
    private float prevZDistance;

    /// <summary>
    /// Predict where the ball will hit the closer table.
    /// </summary>
    public void predict()
    {
        //TODO: predict the highest point in the ball's trajectory and move the racket there.
    }

    /// <summary>
    /// Move the racket to catch the ball.
    /// </summary>
    void move()
    {
    }

    /// <summary>
    /// Set where to return the ball to.
    /// </summary>
    Vector3 setTarget()
    {
        return new Vector3();
    }

    /// <summary>
    /// Apply a calculated velocity to the ball to hit the target.
    /// </summary>
    void hit()
    {
        Vector3 target = setTarget();
        ball.AddForce(new Vector3(0, 0, -10), ForceMode.Impulse);
    }

    // Use this for initialization
    void Start()
    {
        prevZDistance = Math.Abs(ball.transform.position.z - myRacket.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        float currentZDistance = Math.Abs(ball.transform.position.z - myRacket.transform.position.z);
        if (currentZDistance < prevZDistance)
        {
            myRacket.transform.position = new Vector3(ball.transform.position.x, myRacket.transform.position.y,
                myRacket.transform.position.z);
        }
        prevZDistance = currentZDistance;
    }

    void OnCollisionExit(Collision col)
    {
        if (col.gameObject == ball.gameObject)
        {
            hit();
        }
    }
}