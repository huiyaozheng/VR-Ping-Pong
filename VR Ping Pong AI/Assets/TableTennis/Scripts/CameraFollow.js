#pragma strict

var speed : float;   //60breakfast+80rice+40veg
var bat : Transform;

var yellowBatStyle : GUIStyle;
var redBatStyle : GUIStyle;
var startButtonStyle : GUIStyle;

public static var userPoints : String ;
public static var aiPoints : String ;

function Start()
{
	userPoints = "0";
	aiPoints = "0";
	Cursor.visible = false;
	Cursor.lockState = CursorLockMode.Locked;
}

function Update () {
	if(bat && bat.transform.position.x > -3 && bat.transform.position.x < 3)
		transform.position = Vector3.Lerp(transform.position,Vector3(bat.transform.position.x,transform.position.y,transform.position.z),Time.deltaTime*speed);
		
	if(Input.GetKeyDown(KeyCode.R))
	{
		Application.LoadLevel("TableTennis");
	}
}