#pragma strict

private var getCollision : boolean = true;



function OnTriggerEnter(other : Collider)
{
	if(other.GetComponent.<Collider>().tag == "ball")
	{
		if(other.GetComponent.<Rigidbody>().GetComponent(TableTennisBall).batStatus == "abat" && getCollision)
		{
			getCollision = false;
			other.GetComponent.<Rigidbody>().velocity = Vector3.zero;
			other.GetComponent.<Rigidbody>().AddForce(transform.forward*5,ForceMode.Impulse);
			ResetThings(other.transform);
			UserBat.points++;
			CameraFollow.userPoints = UserBat.points.ToString();
		}
		if(other.GetComponent.<Rigidbody>().GetComponent(TableTennisBall).batStatus == "ubat" && getCollision)
		{
			getCollision = false;
			other.GetComponent.<Rigidbody>().velocity = Vector3.zero;
			other.GetComponent.<Rigidbody>().AddForce(-transform.forward*5,ForceMode.Impulse);
			ResetThings(other.transform);
			AIBat.points++;
			CameraFollow.aiPoints = AIBat.points.ToString();
		}
	}
}

function ResetThings(other : Transform)
{
	yield WaitForSeconds(1);
	other.GetComponent.<Rigidbody>().GetComponent(TableTennisBall).Reset();
	getCollision = true;
}