using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CoachableHeuristics : System.Object {

	public float reactionTime_mean;
	public float reactionTime_stDev;

//	public float errorRate_tooHigh_mean;
//	public float errorRate_tooHigh_stDev;

//	public float errorRate_tooLow_mean;
//	public float errorRate_tooLow_stDev;

	public float errorRate_tooHigh;
	public float errorRate_tooLow;

	/// <summary>
	/// This should be a float between 0 and 0.5.
	/// Corresponds to the probability that neural net
	/// output will be changed to hit to that side.
	/// Will probably want to keep the sum of the numbers not too high!
	/// </summary>
	public float sidePreference_left_max05;
	public float sidePreference_right_max05;


	/// <summary>
	/// This should be a float between 0 and 0.33.
	/// Corresponds to the probability that neural net
	/// output will be changed to hit to that side.
	/// Will probably want to keep the sum of the numbers not too high!
	/// </summary>
	public float heightPreference_high_max033;
	public float heightPreference_medium_max033;
	public float heightPreference_low_max033;

	/// <summary>
	/// Has some fixed maximum length.
	/// Stores the last N winning plays, or fewer.
	/// The format is: 
	///    (play.x, play.z) represents the landPos, 
	///    play.y represents the maxHeight.
	/// </summary>
	public List<Vector3> winningShots;
}
