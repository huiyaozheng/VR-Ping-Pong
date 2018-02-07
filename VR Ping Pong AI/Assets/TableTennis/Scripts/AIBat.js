#pragma strict

var batSpeed : float;

var ball : Transform;
var speed : float;

public static var move : boolean;

var hitDirection : float;

var particleTexture : Texture2D;

var p : Transform;

var firstPostion : Vector3;

public static var points : int;


function Start()
{
	points = 0;
	move = true;
	firstPostion = transform.position;
}


function Update () {

	transform.eulerAngles.z = transform.position.x*-10;
	
	if(move && transform.position.y >= 1.4 && transform.position.x > -4 && transform.position.x < 4)
			transform.position = Vector3.Lerp(transform.position,Vector3(ball.transform.position.x,ball.transform.position.y+0.2,transform.position.z),Time.deltaTime*batSpeed);
		
	if(transform.position.y < 1.4)
		transform.position.y = 1.4;
		
	
}

function OnCollisionEnter(other : Collision)
{
	if(other.collider.tag == "ball")
	{
		if(transform.position.x < -1.7 || transform.position.x > 1.7)
		{
			hitDirection*= -1;
			hitDirection = hitDirection/2;
		}
		else{
		
			if(Random.Range(1,3) == 1)
				hitDirection =-hitDirection;
		}
		
		
		other.rigidbody.GetComponent(TableTennisBall).batStatus = "abat";
		speed = 15;
		other.rigidbody.velocity = Vector3.zero;
		other.rigidbody.isKinematic = true;
		
		other.transform.position = other.contacts[0].point;
		other.transform.position.z-=0.1;
		other.rigidbody.isKinematic = false;
		
		if(transform.position.y < 1.55)
		{
			other.rigidbody.AddForce(Vector3.up*4,ForceMode.Impulse);
			other.rigidbody.AddForce(-transform.forward*speed/1.5,ForceMode.Impulse);
		
			other.rigidbody.AddForce(transform.right*hitDirection*1.5,ForceMode.Impulse);
		}
		else if(transform.position.y < 1.7)
		{
			other.rigidbody.AddForce(Vector3.up*2,ForceMode.Impulse);
			other.rigidbody.AddForce(-transform.forward*speed,ForceMode.Impulse);
		
			other.rigidbody.AddForce(transform.right*hitDirection*2,ForceMode.Impulse);
		}
		else
		{
			other.rigidbody.AddForce(Vector3.up*1.5,ForceMode.Impulse);	
			other.rigidbody.AddForce(-transform.forward*speed,ForceMode.Impulse);
		
			other.rigidbody.AddForce(transform.right*hitDirection*2,ForceMode.Impulse);
		}
			
		p.GetComponent(ParticleRenderer).materials[0].mainTexture = particleTexture;
		move = false;
	}
}

public function HitDirection(x : float)
{
	hitDirection = x;
}

public function Reset()
{
	transform.position = firstPostion;
	move = true;
}

