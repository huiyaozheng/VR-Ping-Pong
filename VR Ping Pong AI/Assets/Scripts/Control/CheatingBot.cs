using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatingBot : MonoBehaviour {

	public GameState game;
	public Rigidbody racket;

	public Transform target;
	public Transform heightTarget;

	public float adjustmentSpeed = 30f;

	public float minZ, maxZ, minY;

	private Rigidbody ball;

	private float g;

	private Collider ballCollider;

	void Start()
	{
		ball = game.ball;
		ballCollider = ball.GetComponent<Collider>();
		g = Physics.gravity.magnitude;
	}

	// Move towards the ball...

	void FixedUpdate () {
		Vector3 velocity = ball.position - racket.position;
		if (ball.position.z < minZ || maxZ < ball.position.z || ballColliderDisableTimer > 0)
			velocity.z = 0f;
		if (ball.position.y < minY)
			velocity.y = 0f;
		
		racket.velocity = velocity * adjustmentSpeed;
	}


	public float ballColliderDisableTime = 0.2f;
	private float ballColliderDisableTimer = 0.0f;

	// We disable the ball collider for a while after a hit, to avoid multiple hits in quick succession - will need some improvements
	void Update()
	{
		if (ballColliderDisableTimer > 0f)
		{
			ballColliderDisableTimer += Time.deltaTime;
			if (ballColliderDisableTimer > ballColliderDisableTime)
			{
				ballColliderDisableTimer = 0f;
				ballCollider.enabled = true;
				Debug.Log("Enabled ball's collider");
			}
		}
	}

	// Cheat and change the velocity of the ball so that it hits the target and flies at a give maximum height
	// Works given assumption that table is at height 0!

	Vector3 velFromTraj(Vector3 target, Vector3 currPos, float maxHeightTarget, float g)
	{
		float H = Mathf.Max(maxHeightTarget, currPos.y + 0.01f);
		float h0 = currPos.y;

		Vector3 dist = currPos - target;
		dist.y = 0;
		float l = dist.magnitude; // distance to target

		float v_y = Mathf.Sqrt((H - h0) * 2 * g);
		float v_x = l * g / (v_y + Mathf.Sqrt(2 * g * H));

		Vector3 vel = target - currPos;
		vel.y = 0;
		vel = vel.normalized;
		vel.y = v_y;
		vel.x *= v_x;
		vel.z *= v_x;
		return vel;
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject != ball.gameObject)
			return;

		ball.velocity = velFromTraj(target.position, ball.position, heightTarget.position.y, Physics.gravity.magnitude);
		return;
	}
}
