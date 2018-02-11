using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catcher : MonoBehaviour
{
    public GameState game;

    private Rigidbody ball;
    private Rigidbody racket;

    private Collider closerTable;

    /// <summary>
    /// Predict where the ball will hit the closer table.
    /// </summary>
    void predict()
    {
        
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
    }

	// Use this for initialization
	void Start ()
	{
	    ball = game.ball;
	    closerTable = game.table0;
	    racket = game.racket0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
