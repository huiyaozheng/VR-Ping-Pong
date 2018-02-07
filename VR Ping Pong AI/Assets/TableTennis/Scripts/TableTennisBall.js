#pragma strict

var factor : int;

var reset : boolean = false;

var userBatScript : UserBat;
var aiBatScript : AIBat;

var batStatus : String;

private var speed : float = 12;
private var firstpostion : Vector3;
private var firstServe : boolean;
private var batTransform : Transform;
private var groundCount : int;
private var tableSideName : String;

function Start()
{
	firstpostion = transform.position;
}


function Reset()
{
	reset = false;
	
	GetComponent.<Rigidbody>().useGravity = false;
	GetComponent.<Rigidbody>().velocity = Vector3.zero;
	transform.position = firstpostion;
	aiBatScript.Reset();
	userBatScript.Reset();
	
}

function Serve(t : Transform)
{
print ("jago ");
	if(Random.Range(1,3) == 1)
	{
		factor = -1;
	}
	else
	{
		factor = 1;
	}
	reset = true;
	
	GetComponent.<Rigidbody>().useGravity = true;
	GetComponent.<Rigidbody>().AddForce(Vector3.down*2.7,ForceMode.Impulse);
	GetComponent.<Rigidbody>().AddForce(transform.forward*speed,ForceMode.Impulse);
	
	if(Random.Range(1,3) == 1)
		GetComponent.<Rigidbody>().AddForce(transform.right*0.4*2,ForceMode.Impulse);
	else
		GetComponent.<Rigidbody>().AddForce(transform.right*-0.4*2,ForceMode.Impulse);
	
	firstServe = true;
	batTransform = t;
}


function OnCollisionEnter(collisionInfo : Collision) {
	
	if(collisionInfo.collider.name == "UserSideTable")
	{
		userBatScript.RegisterSpin();
		tableSideName = "UserSideTable";
	}
	
	if(collisionInfo.collider.name == "AISideTable")
	{
		if(firstServe)
		{
			batTransform.GetComponent.<Collider>().isTrigger = false;
			batTransform.GetComponent.<Rigidbody>().isKinematic =true;
			batTransform.GetComponent(UserBat).firstServe = false;
			firstServe = false;
		}
			
		tableSideName = "AISideTable";
	}
	
	if(collisionInfo.collider.name == "Wall")
	{
		groundCount++;
		if(groundCount == 2)
		{
			Reset();
			groundCount = 0;
			
			if(batStatus == "abat")
			{
				if(tableSideName == "UserSideTable"){
					AIBat.points++;
					CameraFollow.aiPoints = AIBat.points.ToString();
				}
				if(tableSideName == "AISideTable"){
					UserBat.points++;
					CameraFollow.userPoints = UserBat.points.ToString();
				}
			}
			
			if(batStatus == "ubat")
			{
				if(tableSideName == "AISideTable"){
					UserBat.points++;
					CameraFollow.userPoints = UserBat.points.ToString();
				}
				if(tableSideName == "UserSideTable"){
					AIBat.points++;
					CameraFollow.aiPoints = AIBat.points.ToString();
				}
			}
		}
	}
}