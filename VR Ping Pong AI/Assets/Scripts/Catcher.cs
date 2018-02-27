using UnityEngine;
using PhysicsLibrary;


public class Catcher : MonoBehaviour
{
    public GameState game;
    public Rigidbody ball;

    /// <summary>
    /// The racket controlled by this script.
    /// </summary>
    public Rigidbody myRacket;
    protected Vector3 myDefPos;

    /// <summary>
    /// This side of the table.
    /// </summary>
    public Collider closerTable;

    /// <summary>
    /// The opponent's racket.
    /// </summary>
    public Rigidbody opponentRacket;

    /// <summary>
    /// Opponent's side of table.
    /// </summary>
    public Collider opponentTable;

    /// <summary>
    /// The max speed that the racket can move at.
    /// </summary>
    public float maxRacketMovingSpeed;

    /// <summary>
    /// True if the racket has positive Z-coordinate.
    /// </summary>
    public bool invertXZ;

    /// <summary>
    /// If tracking is false, the racket will only track the ball in X-axis. Otherwise it goes towards the ball.
    /// </summary>
    protected bool tracking;

    /// <summary>
    /// Z-axis distance between the ball and myRacket. Used to detect whether the ball is flying towards us.
    /// </summary>
    protected float prevZDistance;

    /// <summary>
    /// Max height of the returned ball's trajectory.
    /// </summary>
    public float maxTrajectoryHeight;

    /// <summary>
    /// The returned ball's landing position.
    /// </summary>
    protected Vector3 landPos;

    /// <summary>
    /// The ball will tell the racket to start to move towards it when it bounces on the closer table.
    /// </summary>
    public virtual void startTracking()
    {
        tracking = true;
    }

    /// <summary>
    /// To tell the racket to stop from moving towards the ball.
    /// </summary>
    public void stopTracking()
    {
        tracking = false;
    }

    /// <summary>
    /// Move the racket to catch the ball.
    /// </summary>
    protected void move(float targetDistance)
    {
        if (tracking)
        {
            // If the ball has bounced on the closer table, move towards the ball.
            Vector3 direction = (ball.transform.position - myRacket.transform.position).normalized;
            myRacket.velocity = direction * maxRacketMovingSpeed;
        }
        else
        {
            // Otherwise, just track the ball in X-direction.
            Vector3 direction = new Vector3(ball.transform.position.x - myRacket.transform.position.x, 0, 0).normalized;
            float xPos =
                maxRacketMovingSpeed * Time.fixedDeltaTime > Mathf.Abs(ball.transform.position.x - myRacket.transform.position.x)
                    ? ball.transform.position.x
                    : (myRacket.position + direction * maxRacketMovingSpeed * Time.fixedDeltaTime).x;
            myRacket.transform.position = new Vector3(xPos, myRacket.transform.position.y,
                myRacket.transform.position.z);
        }
    }

    /// <summary>
    /// Set the trajectory of the returned ball.
    /// </summary>
    /// <param name="_landPos">Landing position on the opponent's side of table</param>
    /// <param name="_maxHeight"></param>
    public void setTargets(Vector3 _landPos, float _maxHeight)
    {
        //Debug.Log("TARGETS SET");
        landPos = _landPos;
        maxTrajectoryHeight = _maxHeight;
    }

    /// <summary>
    /// Apply a calculated velocity to the ball to hit the target.
    /// </summary>
    protected virtual void hit()
    {
        float aimm = Random.Range(2.0f, maxTrajectoryHeight);
        ball.velocity = PhysicsCalculations.velFromTraj(landPos, ball.position, aimm, Physics.gravity.magnitude, false);
    }

	protected virtual void Start()
    {
        myDefPos = myRacket.transform.position;
        prevZDistance = Mathf.Abs(ball.transform.position.z - myRacket.transform.position.z);
        tracking = false;
    }

	protected bool IsRacketCloseToDefaultPos()
	{
		return (myDefPos - myRacket.transform.position).sqrMagnitude < 4;
	}

    protected virtual void Update()
    {
        //Debug.Log("maxTrajH " + maxTrajectoryHeight);
        float currentZDistance = Mathf.Abs(ball.transform.position.z - myRacket.transform.position.z);

        // Move if the ball is incoming.
        if (currentZDistance < prevZDistance)
        {
            move(currentZDistance);
        }
        else
        {
            // If the racket is close to the default position, stop moving.
			if (IsRacketCloseToDefaultPos())
            {
                myRacket.velocity = new Vector3(0,0,0);
            }
        }
        
        prevZDistance = currentZDistance;
    }

    protected virtual void OnCollisionEnter(Collision col)
    {
        if (col.gameObject == ball.gameObject)
        {
            // Return the ball to a random location.
            float x = (opponentTable.transform.localScale.x) / 2 - 0.5f;
            float z = (opponentTable.transform.localScale.z) / 2 - 0.5f;
            x = Random.Range(-x, x);
            z = Random.Range(3, z);
            setTargets(new Vector3(x, 0, z) * (invertXZ ? -1f : 1f), maxTrajectoryHeight);
            hit();
            // Move the racket back to the default position.
            myRacket.velocity = (myDefPos - myRacket.transform.position).normalized * maxRacketMovingSpeed * 0.2f;
            tracking = false;
        }
    }

    public void serve() {
        // float x = (opponentTable.transform.localScale.x) / 2;
        // x = x / 2;
        // x = Random.Range(-x, x);
        // Debug.Log(myRacket.transform.position);
        // myRacket.transform.position = new Vector3(x, myRacket.transform.position.y, myRacket.transform.position.z);
        ball.transform.position = myRacket.transform.position + new Vector3(0,0,0.05f) * (invertXZ ? -1f : 1f);
        float x = (opponentTable.transform.localScale.x) / 2;
        x = x * 0.3f;
        float z = (opponentTable.transform.localScale.z) / 2;
        x = Random.Range(-x, x);
        z = Random.Range(z * 1.2f, z * 1.3f);
        Vector3 target = new Vector3(x, 0, z) * (invertXZ ? 1f : -1f);
        Debug.Log(target);
        Debug.Log(ball.transform.position);
        Debug.Log(myRacket.transform.position);
        ball.velocity = PhysicsCalculations.velFromTraj(target, ball.transform.position, myRacket.transform.position.y, Physics.gravity.magnitude, true);
    }
}