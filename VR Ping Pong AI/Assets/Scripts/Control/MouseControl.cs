using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour {

	public GameState game;
	public Rigidbody racket;
	public float speedDepth;
	public float speedWidth;
	public bool flipSides;

	private Rigidbody ball;

	public float maxY;
	public float minY;
	public float yAdjustmentSpeed;

	private Vector3 defaultBallPos;
	
	// Update is called once per frame
	void FixedUpdate () {
		float mouseX = Input.GetAxis("Mouse X");
		float mouseY = Input.GetAxis("Mouse Y");

		Vector3 velocity = new Vector3(mouseX, 0, mouseY).normalized;
		velocity.x *= speedWidth;
		velocity.z *= speedDepth;
		velocity *= flipSides ? -1f : 1f;

		float ballY = ball.position.y;

		if (true)//minY < ballY && ballY < maxY)
		{
			velocity.y = ballY - racket.position.y;
			velocity.y *= yAdjustmentSpeed;
		} 
		else
		{
			velocity.y = 0f;
		}

		racket.velocity = velocity;
	}

	void Start()
	{
		ball = game.ball;
		defaultBallPos = ball.position;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			ball.position = defaultBallPos;
			ball.velocity = Vector3.zero;
		}
	}

	void OnCollisionEnter(Collision col) {
		if (col.gameObject != ball.gameObject)
			return;
		ball.velocity = PhysicsLibrary.PhysicsCalculations.batHit (ball.velocity, racket.velocity, racket.transform.forward.normalized);
	}
}

