using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour {

	public GameState game;
	public Transform objectToRotateAsAJokeAfterSetEnds;
	public float rotationSpeed = 1f;

	private float rotX = 0f;

	public void StartGame () {
		game.InitGame();
		rotX = 0f;
	}
		
	public void EndGame () {
		rotX = 0.1f;
	}

	void Update () {
		if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.P))
		{
			StartGame();
		}

		if (rotX >= 0.1f)
		{
			rotX += Time.deltaTime * rotationSpeed;
			objectToRotateAsAJokeAfterSetEnds.rotation.eulerAngles.x = rotX;
		}
	}


	void OnEnable()
	{
		Events.setEnded += EndGame;
	}
	void OnDisable()
	{
		Events.setEnded -= EndGame;
	}
}
