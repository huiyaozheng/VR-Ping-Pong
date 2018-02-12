using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhysicsLibrary;

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

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject != ball.gameObject)
			return;

		ball.velocity = PhysicsCalculations.velFromTraj(target.position, ball.position, heightTarget.position.y, Physics.gravity.magnitude);
		return;
	}
}
