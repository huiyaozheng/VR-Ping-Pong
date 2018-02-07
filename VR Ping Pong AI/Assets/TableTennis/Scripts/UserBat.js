#pragma strict

private var cp : Vector3;
private var lp : Vector3;

var speed : float;

var lastVelocity : float;

var aiBat : AIBat;

var particleTexture : Texture2D;

var p : Transform;

var firstServe : boolean = true;

var firstPostion : Vector3;

public static var points : int;



function Start () {
	points = 0;
	firstPostion = transform.position;
}

function Update () {

//	transform.eulerAngles.z = transform.position.x*-10;
//	
//	
//	
//	if(Application.platform == RuntimePlatform.IPhonePlayer)
//	{
//		for (var touch : Touch in Input.touches) 
//		{
//			transform.position.x = Mathf.Lerp(transform.position.x,transform.position.x+touch.deltaPosition.x * Time.deltaTime*2,.2);
//			transform.position.y = Mathf.Lerp(transform.position.y,transform.position.y+touch.deltaPosition.y * Time.deltaTime*2,.2);
//		}
//	}
//	else
//	{
//		transform.position.x = Mathf.Lerp(transform.position.x,transform.position.x+Input.GetAxis("Mouse X"),.2);
//		transform.position.y = Mathf.Lerp(transform.position.y,transform.position.y+Input.GetAxis("Mouse Y"),.2);
//	}
//	
//	
//	
//	if(transform.position.x > 4)
//		transform.position.x = 4;
//	if(transform.position.x < -4)
//		transform.position.x = -4;
//		
//	if(transform.position.y < 1.5)
//		transform.position.y = 1.5;
//	if(transform.position.y > 2.5)
//		transform.position.y = 2.5;
	
}

function OnCollisionEnter(other : Collision)
{
	if(other.collider.tag == "ball" && !firstServe)
	{

		cp = transform.position;
		var diff : float = (cp.x-lp.x);
		if(diff < -1.1)
		{
			diff = -1.1;
		}
		if(diff > 1.1)
		{
			diff = 1.1;
		}
		other.rigidbody.GetComponent(TableTennisBall).batStatus = "ubat";
		
		other.rigidbody.velocity = Vector3.zero;
		other.rigidbody.isKinematic = true;
		
		other.transform.position = other.contacts[0].point;
		other.transform.position.z+=0.1;
		other.rigidbody.isKinematic = false;
		other.rigidbody.AddForce(transform.forward*15,ForceMode.Impulse);
		
		other.rigidbody.AddForce(transform.right*diff*2,ForceMode.Impulse);
		if(other.transform.position.y < 1.68)
			other.rigidbody.AddForce(Vector3.up*1.6,ForceMode.Impulse);
		else
			other.rigidbody.AddForce(Vector3.up*1.2,ForceMode.Impulse);
		
		p.GetComponent(ParticleRenderer).materials[0].mainTexture = particleTexture;
		
		AIBat.move = true;
		aiBat.HitDirection(diff);
		
	}
}

function OnTriggerEnter(other : Collider)
{
	if(other.GetComponent.<Collider>().tag == "ball")
	{
		other.transform.SendMessage("Serve",transform);
	}
}

public function RegisterSpin()
{
	lp = transform.position;
}

public function Reset()
{
	//transform.position = firstPostion;
	GetComponent.<Collider>().isTrigger = true;
	GetComponent.<Rigidbody>().isKinematic =false;
	firstServe = true;
}