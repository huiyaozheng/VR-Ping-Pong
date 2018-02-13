using UnityEngine;
using PhysicsLibrary;


public class Catcher : MonoBehaviour
{
    public GameState game;
    public Rigidbody ball;

    private Vector3 landPos;
    private float maxHeight;

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
    /// The max speed that the racket can move at.
    /// </summary>
    public float maxSpeed;

    public bool invertXZ;

    /// <summary>
    /// If tracking is false, the racket will only track the ball in X-axis. Otherwise it goes towards the ball.
    /// </summary>
    private bool tracking;

    /// <summary>
    /// Z-axis distance between the ball and myRacket. Used to detect whether the ball is flying towards us.
    /// </summary>
    private float prevZDistance;

    /// <summary>
    /// Predict where the ball will hit the closer table.
    /// </summary>
    public void startTracking()
    {
        tracking = true;
    }

    public void stopTracking()
    {
        tracking = false;
    }

    /// <summary>
    /// Move the racket to catch the ball.
    /// </summary>
    void move(float targetDistance)
    {
        if (tracking)
        {
            Vector3 direction = (ball.transform.position - myRacket.transform.position).normalized;
            myRacket.velocity = direction * maxSpeed;
        }
        else
        {
            myRacket.transform.position = new Vector3(ball.transform.position.x, myRacket.transform.position.y,
                myRacket.transform.position.z);
        }
    }

    /// <summary>
    /// Set where to return the ball to.
    /// <param>
    /// Landing position (vector3)
    /// </param>
    /// <param>
    /// Point above the net (vector3)
    /// </param>
    /// </summary>
    //Public, so agent's can set it
    public void setTargets(Vector3 _landPos, float _maxHeight)
    {
        //Note the vector3 for the net can be vector2 as net is always on Z=0j

        landPos = _landPos;
        maxHeight = _maxHeight;
    }

    /// <summary>
    /// Apply a calculated velocity to the ball to hit the target.
    /// </summary>
    void hit()
    {
        ball.velocity = PhysicsCalculations.velFromTraj(landPos, ball.position, maxHeight, Physics.gravity.magnitude);
    }

    // Use this for initialization
    void Start()
    {
        prevZDistance = Mathf.Abs(ball.transform.position.z - myRacket.transform.position.z);
        tracking = false;
    }

    // Update is called once per frame
    void Update()
    {
        float currentZDistance = Mathf.Abs(ball.transform.position.z - myRacket.transform.position.z);
        if (currentZDistance < prevZDistance) // The ball is incoming.
        {
            move(currentZDistance);
        }
        prevZDistance = currentZDistance;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject == ball.gameObject)
        {
            float x = (opponentTable.transform.parent.gameObject.transform.localScale.x) / 2 - 0.5f;
            float z = (opponentTable.transform.parent.gameObject.transform.localScale.z) / 2 - 0.5f;
            x = Random.Range(-x, x);
            z = Random.Range(2, z);
            Debug.Log("Target: "+x+", "+z);
            setTargets(new Vector3(x, 0, z) * (invertXZ ? -1f : 1f), 5);
            hit();
            myRacket.velocity = new Vector3(0,0,0);
            tracking = false;
        }
    }
}