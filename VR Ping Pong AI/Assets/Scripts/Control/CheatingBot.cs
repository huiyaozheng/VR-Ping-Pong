using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatingBot : MonoBehaviour {

	public GameState game;
	public Rigidbody racket;
	public Transform racketCentre;

	public Transform target;
	public Transform heightTarget;

	public float adjustmentSpeed = 30f;

	public float minZ, maxZ, minY;

	private Rigidbody ball;

	private float g;
	private float h_fixed;

	private Collider ballCollider;

	void Start()
	{
		ball = game.ball;
		ballCollider = ball.GetComponent<Collider>();
		g = Physics.gravity.magnitude;
		h_fixed = heightTarget.position.y - target.position.y; // how high the height target is above the table
	}

	// Move towards the ball...

	void FixedUpdate () {
		Vector3 velocity = ball.position - racketCentre.position;
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

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject != ball.gameObject)
			return;

		// Cheat and make the ball fly at the target.
		// Formulas: http://keisan.casio.com/exec/system/1225104043

		float h0 = ball.position.y - target.position.y; // current height above the target (=above the table)
		//    h = maximum height during flight
		float h = Mathf.Max(h_fixed, ball.position.y + 0.01f);

		Vector3 dist = ball.position - target.position;
		dist.y = 0;
		float l = 2 * dist.magnitude; // distance to target
									 // 2* above is a hack, don't know what's wrong but without it the balls are short

		float t = (Mathf.Sqrt(2 * g * (h - h0)) + Mathf.Sqrt(2 * g * h))/g; // time of flight - intermediate value

		float v0 = Mathf.Sqrt((l * l) / (t * t) + 2 * g * (h - h0)); // result: speed
		float a0 = Mathf.Atan((t / l) * Mathf.Sqrt(2 * g * (h - h0))); // result: angle


		Vector3 dir = target.position - ball.position;
		dir.y = 0f;
		dir = dir.normalized;
		dir.y = v0 * Mathf.Sin(a0);
		dir.x *= v0 * Mathf.Cos(a0);
		dir.z *= v0 * Mathf.Cos(a0);

//		float a = t * Mathf.Sqrt(2 * g * (h - h0));
//		float b = l;
//
//		float sin = a / Mathf.Sqrt(a * a + b * b);
//		float cos = b / Mathf.Sqrt(a * a + b * b);
//
//		dir.y = v0 * sin;
//		dir.x *= v0 * cos;
//		dir.z *= v0 * cos;


//		Debug.Log("Bot hit ball with v0 = " + dir.ToString() + ", magn = " + dir.magnitude.ToString());
		ball.velocity = dir * 2;

		ballCollider.enabled = false;
		ballColliderDisableTimer += 0.001f;
	}
}
