using System.Collections;
using System.Collections.Generic;
using PhysicsLibrary;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Server : MonoBehaviour
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
    /// The half closer to racket.
    /// </summary>
    public Collider closerTable;

    /// <summary>
    /// The half further from racket.
    /// </summary>
    public Collider furtherTable;

    /// <summary>
    /// The bot's racket.
    /// </summary>
    public Collider racket;

    private Vector3 racketDefaultPosition;
    private Quaternion racketDefaultRotation;

    /// <summary>
    /// States in the state machine.
    /// </summary>
    private enum eShooterState
    {
        State0,
        State1,
        State2
    };

    /// <summary>
    /// Inputs to the state machine.
    /// </summary>
    private enum eShooterTransition
    {
        ST_RACK,
        ST_BADTABLE,
        ST_GOODTABLE,
        ST_FLOOR
    };

    /// <summary>
    /// Current state of the state machine.
    /// </summary>
    private eShooterState currState = eShooterState.State0;

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
    /// Step the state machine.
    /// </summary>
    /// <param name="transition">Input of the machine</param>
    void MakeStep(eShooterTransition transition)
    {
        switch (currState)
        {
            case eShooterState.State0:
                switch (transition)
                {
                    case eShooterTransition.ST_BADTABLE:
                        currState = eShooterState.State1;
                        break;
                    case eShooterTransition.ST_RACK:
                        ShootBall();
                        break;
                    default:
                        ShootBall();
                        break;
                }
                break;
            case eShooterState.State1:
                switch (transition)
                {
                    case eShooterTransition.ST_RACK:
                        currState = eShooterState.State2;
                        break;
                    default:
                        ShootBall();
                        break;
                }
                break;
            case eShooterState.State2:
                switch (transition)
                {
                    case eShooterTransition.ST_GOODTABLE:
                        ShootBall();
                        break;
                    default:
                        ShootBall();
                        break;
                }
                break;
        }
    }

    /// <summary>
    /// When the ball collides with something, step the state machine.
    /// </summary>
    /// <param name="col">The collision object</param>
    void OnCollisionEnter(Collision col)
    {
        Debug.Log("Collided with " + col.gameObject.name);
        if (col.gameObject == floor.gameObject)
        {
            MakeStep(eShooterTransition.ST_FLOOR);
        }
        else if (col.gameObject == closerTable.gameObject)
        {
            MakeStep(eShooterTransition.ST_BADTABLE);
        }
        else if (col.gameObject == furtherTable.gameObject)
        {
            MakeStep(eShooterTransition.ST_GOODTABLE);
        }
        else if (col.gameObject == racket.gameObject)
        {
            MakeStep(eShooterTransition.ST_RACK);
        }
    }

    void OnCollisionExit(Collision col)
    {
        if (col.gameObject == closerTable.gameObject)
        {
            racket.gameObject.GetComponent<Catcher>().startTracking();
        }
    }

    /// <summary>
    /// Reset the ball to the default position and add a random velocity to the ball.
    /// </summary>
    void ShootBall()
    {
        racket.gameObject.transform.rotation = racketDefaultRotation;
        racket.gameObject.transform.position = racketDefaultPosition;
        racket.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        racket.gameObject.GetComponent<Catcher>().stopTracking();
        currState = eShooterState.State0;
        gameObject.transform.position = defaultBallPos;
        float x = (closerTable.transform.parent.gameObject.transform.localScale.x) / 2 - 0.5f;
        float z = (closerTable.transform.parent.gameObject.transform.localScale.z) / 2 - 0.5f;
        x = Random.Range(-x, x);
        z = Random.Range(4, z);
        ballBody.velocity = PhysicsCalculations.velFromTraj(new Vector3(x, 0, z) * (invertXZMult ? -1f : 1f), ballBody.position, maxHeight, Physics.gravity.magnitude);
    }

    void Start()
    {
        defaultBallPos = gameObject.transform.position;
        racketDefaultPosition = racket.gameObject.transform.position;
        racketDefaultRotation = racket.gameObject.transform.rotation;
        ballBody = gameObject.GetComponent<Rigidbody>();
        currState = eShooterState.State0;
        ShootBall();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootBall();
        }
    }
}