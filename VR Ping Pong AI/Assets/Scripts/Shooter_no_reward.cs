using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Shooter_no_reward : MonoBehaviour
{
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
    /// The ball first bounces on the serving side's table. Do not notify the racket in this case.
    /// </summary>
    public bool firstBounce = true;

    public GameState game;

    void OnCollisionEnter(Collision col)
    {
        if (!game.hasGameStarted) return;
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
}