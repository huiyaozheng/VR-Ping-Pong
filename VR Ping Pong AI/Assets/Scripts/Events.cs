using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Events : MonoBehaviour {

	public delegate void VoidVoid();

	/*
	 * Called from GameCollisionTracker.
	 * 
	 */
	public static event VoidVoid rallyEnded;
	public static void eRallyEnded()
	{
		if (rallyEnded != null)
		{
			Debug.Log("Run event: rallyEnded");
			rallyEnded();
		} 
		else
			Debug.LogError("Event is null: rallyEnded");
	}

	public static event VoidVoid setEnded;
	public static void eSetEnded()
	{
		if (setEnded != null)
		{
			Debug.Log("Run event: setEnded");
			setEnded();
		} 
		else
			Debug.LogError("Event is null: setEnded");
	}

	public delegate void VoidGO(GameObject go);
	public static event VoidGO racketHitBall;
	public static void eRacketHitBall(GameObject go)
	{
		if (racketHitBall != null)
		{
			//Debug.Log("Run event: racketHitBall");
			racketHitBall(go);
		} 
		else
			Debug.LogError("Event is null: racketHitBall");
	}
}
