using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour {

	public GameState game;

	// Rotate the table to emphasize end of set.
	public Transform objectToRotateAsAJokeAfterSetEnds;
	public float rotationSpeed = 180f;
	private float rotX = 0f;

	void Awake()
	{
		// deactivate ball and opponent
		game.ball.gameObject.SetActive(false);
		game.player1.SetActive(false);
	}

	public void StartGame () {
		// activate ball and opponent
		game.ball.gameObject.SetActive(true);
		game.player1.SetActive(true);

		// initialize game
		game.InitGame();
		Debug.Log("START GAME() !!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

		// stop rotating table:
		rotX = 0f;
	}
		
	public void EndGame () {
		// start rotating table:
		rotX = 0.1f;

		// deactivate ball and opponent
		game.ball.gameObject.SetActive(false);
		game.player1.SetActive(false);
	}

	void Update () {
		if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.P)) // For debugging purposes.
		{
			StartGame();
		}


		// Rotate table.
		if (rotX >= 0.1f)
		{
			rotX += Time.deltaTime * rotationSpeed;
			Vector3 temp = objectToRotateAsAJokeAfterSetEnds.rotation.eulerAngles;
			temp.x = rotX;
			objectToRotateAsAJokeAfterSetEnds.rotation = Quaternion.Euler(temp);
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
