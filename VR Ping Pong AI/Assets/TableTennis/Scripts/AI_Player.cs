using UnityEngine;
using System.Collections;

public class AI_Player : MonoBehaviour {

	public float batSpeed;

	public Transform ball;
	public float speed;

	public static bool move;

	public float hitDirection;

	public Texture2D particleTexture;

	public Transform p;

	public Vector3 firstPostion;

	public static int points;

	// Use this for initialization
	void Start () {
		points = 0;
		move = true;
		firstPostion = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
		transform.eulerAngles = new Vector3 (transform.eulerAngles.x, transform.eulerAngles.y, transform.position.x*-10); //= new  transform.position.x*-10;

		if (move && transform.position.y >= 1.4f && transform.position.x > -4f && transform.position.x < 4f) {
			if (ball.position.z < 0) {
				transform.position = Vector3.Lerp(transform.position, new Vector3(ball.transform.position.x,transform.position.y,transform.position.z),Time.deltaTime*batSpeed);
			} else {
				transform.position = Vector3.Lerp(transform.position, new Vector3(ball.transform.position.x,ball.transform.position.y+0.2f,transform.position.z),Time.deltaTime*batSpeed);
			}
		}

		if (transform.position.y < 1.4f)
			transform.position = new Vector3(transform.position.x,1.4f,transform.position.z);//transform.position.y = 1.4f;
		
	}

	void OnCollisionEnter(Collision other)
	{
		if (other.collider.tag == "ball")
		{
			if (transform.position.x < -1.7f || transform.position.x > 1.7f)
			{
				hitDirection*= -1;
				hitDirection = hitDirection/2;
			}
			else{

				if (Random.Range(1,3) == 1)
					hitDirection =-hitDirection;
			}

			
			other.rigidbody.GetComponent<PingPong_Ball>().batStatus = "abat";
			speed = 15;
			other.rigidbody.velocity = Vector3.zero;
			other.rigidbody.isKinematic = true;

			other.transform.position = other.contacts[0].point;
			other.transform.position -= new Vector3(0,0,0.1f) ;
			other.rigidbody.isKinematic = false;

			if (transform.position.y < 1.55f)
			{
				other.rigidbody.AddForce(Vector3.up*4,ForceMode.Impulse);
				other.rigidbody.AddForce(-transform.forward*speed/1.5f,ForceMode.Impulse);

				other.rigidbody.AddForce(transform.right*hitDirection*1.5f,ForceMode.Impulse);
			}
			else if (transform.position.y < 1.7f)
			{
				other.rigidbody.AddForce(Vector3.up*2,ForceMode.Impulse);
				other.rigidbody.AddForce(-transform.forward*speed,ForceMode.Impulse);

				other.rigidbody.AddForce(transform.right*hitDirection*2,ForceMode.Impulse);
			}
			else
			{
				other.rigidbody.AddForce(Vector3.up*1.5f,ForceMode.Impulse);	
				other.rigidbody.AddForce(-transform.forward*speed,ForceMode.Impulse);

				other.rigidbody.AddForce(transform.right*hitDirection*2,ForceMode.Impulse);
			}

			p.GetComponent<ParticleRenderer>().materials[0].mainTexture = particleTexture;
			move = false;
			
		}
	}

	public void HitDirection(float x)
	{
		hitDirection = x;
	}

	public void Reset()
	{
		transform.position = firstPostion;
		move = true;
	}
}
