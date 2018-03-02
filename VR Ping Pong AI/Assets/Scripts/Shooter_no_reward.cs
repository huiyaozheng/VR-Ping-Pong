using System.Collections;
using System.Collections.Generic;
using PhysicsLibrary;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Shooter_no_reward : MonoBehaviour
{
    /// <summary>
    /// Lower bound of the initial velocity in X-axis (left/right)
    /// </summary>
    public float minX;

    /// <summary>
    /// Upper bound of the initial velocity in X-axis (left/right)
    /// </summary>
    public float maxX;

    /// <summary>
    /// Lower bound of the initial velocity in Z-axis (front/back)
    /// </summary>
    public float minZ;

    /// <summary>
    /// Upper bound of the initial velocity in Z-axis (front/back)
    /// </summary>
    public float maxZ;

    /// <summary>
    /// The floor.
    /// </summary>
    public Collider floor;

    /// <summary>
    /// The half closer to racket0.
    /// </summary>
    public Collider table0;

    /// <summary>
    /// The half closer to racket1.
    /// </summary>
    public Collider table1;

    /// <summary>
    /// One racket.
    /// </summary>
    public Collider racket0;

    /// <summary>
    /// Another racket.
    /// </summary>
    public Collider racket1;

    /// <summary>
    /// Default starting position of the ball.
    /// </summary>
    private Vector3 defaultBallPos;

    /// <summary>
    /// The ball's body.
    /// </summary>
    private Rigidbody ballBody;

    /// <summary>
    /// If XZ is inverted, set it to -1f. Otherwise it is 1f.
    /// </summary>
    public bool invertXZMult;

    /// <summary>
    /// maxTrajectoryHeight in the ball's trajectory
    /// </summary>
    public float maxHeight = 2;

    /// <summary>
    /// The ball first bounces on the serving side's table. Do not notify the racket in this case.
    /// </summary>
    public bool firstBounce = true;

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject == table0.gameObject)
        {
            if (firstBounce)
            {
                firstBounce = false;
                return;
            }
            racket0.gameObject.GetComponent<Catcher>().startTracking();
        }
        else if (col.gameObject == table1.gameObject)
        {
            if (firstBounce)
            {
                firstBounce = false;
                return;
            }
            racket1.gameObject.GetComponent<Catcher>().startTracking();
        }
    }

    void Start()
    {
    }

    void Update()
    {
    }
}