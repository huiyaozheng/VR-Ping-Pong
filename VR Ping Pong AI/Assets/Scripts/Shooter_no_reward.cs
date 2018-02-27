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

    private Vector3 racket0DefaultPosition;
    private Vector3 racket1DefaultPosition;
    private Quaternion racket0DefaultRotation;
    private Quaternion racket1DefaultRotation;

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
    private bool firstBounce = true;

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

    /// <summary>
    /// Reset the ball to the default position and add a random velocity to the ball.
    /// </summary>
    public void ShootBall(bool player1Serve)
    {
        racket0.gameObject.transform.rotation = racket0DefaultRotation;
        racket0.gameObject.transform.position = racket0DefaultPosition;
        racket0.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        racket0.gameObject.GetComponent<Catcher>().stopTracking();
        racket1.gameObject.transform.rotation = racket1DefaultRotation;
        racket1.gameObject.transform.position = racket1DefaultPosition;
        racket1.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        racket1.gameObject.GetComponent<Catcher>().stopTracking();

        if (player1Serve) {
        racket1.gameObject.GetComponent<Catcher>().serve();}
        else {
            // TODO handle the case when the player is serving
        }

        firstBounce = true;
    }

void Awake(){
defaultBallPos = gameObject.transform.position;
        racket0DefaultPosition = racket0.gameObject.transform.position;
        racket0DefaultRotation = racket0.gameObject.transform.rotation;
        racket1DefaultPosition = racket1.gameObject.transform.position;
        racket1DefaultRotation = racket1.gameObject.transform.rotation;
        ballBody = gameObject.GetComponent<Rigidbody>();
}
    void Start()
    {
        
    }

    void Update()
    {
    }
}