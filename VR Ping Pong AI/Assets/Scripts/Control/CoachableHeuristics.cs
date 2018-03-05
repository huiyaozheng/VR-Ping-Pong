using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Collection of a heuristic bot's preference data.
/// </summary>
[System.Serializable]
public class CoachableHeuristics : System.Object
{
    public float reactionTime_mean;
    public float reactionTime_stDev;

    public float errorRate_tooHigh;
    public float errorRate_tooLow;

    /// <summary>
    /// This should be a float between 0 and 0.5.
    /// Corresponds to the probability that neural net
    /// output will be changed to hit to that side.
    /// Will probably want to keep the sum of the numbers not too high!
    /// </summary>
    public float sidePreference_left;

    public float sidePreference_right;


    /// <summary>
    /// This should be a float between 0 and 0.33.
    /// Corresponds to the probability that neural net
    /// output will be changed to hit to that side.
    /// Will probably want to keep the sum of the numbers not too high!
    /// </summary>
    public float heightPreference_high;

    public float heightPreference_medium;
    public float heightPreference_low;

    /// <summary>
    /// Has some fixed maximum length.
    /// Stores the last N winning plays, or fewer.
    /// The format is: 
    ///    (play.x, play.z) represents the landPos, 
    ///    play.y represents the maxHeight.
    /// </summary>
    public List<Vector3> winningShots;


    public CoachableHeuristics()
    {
        reactionTime_mean = 0.3f;
        reactionTime_stDev = 0.05f;

        errorRate_tooHigh = 0.05f;
        errorRate_tooLow = 0.05f;

        sidePreference_left = 0f;
        sidePreference_right = 0f;

        heightPreference_high = 0f;
        heightPreference_medium = 0f;
        heightPreference_low = 0f;

        winningShots = new List<Vector3>();
    }

    public float GetLeftShotOverrideProbability()
    {
        // a function from [0, infty) to [0, c] for some c < 0.5
        // a function of sidePreference_left

        // c = 0.3
        // plot: https://www.wolframalpha.com/input/?i=Plot+0.3-0.5*(e%5E(-x+*+0.1))+for+x+from+0+to+30

        return Mathf.Max(0f, 0.3f - 0.5f * Mathf.Exp(-1 * sidePreference_left * 0.1f));
    }

    public float GetRightShotOverrideProbability()
    {
        return Mathf.Max(0f, 0.3f - 0.5f * Mathf.Exp(-1 * sidePreference_right * 0.1f));
    }


    public float GetLowShotOverrideProbability()
    {
        // a function from [0, infty) to [0, c] for some c < 0.5
        // a function of sidePreference_left

        // c = 0.1
        // plot: https://www.wolframalpha.com/input/?i=Plot+0.1-0.1*(e%5E(-x+*+0.1))+for+x+from+0+to+30

        return Mathf.Max(0f, 0.1f - 0.1f * Mathf.Exp(-1 * heightPreference_low * 0.1f));
    }

    public float GetMediumShotOverrideProbability()
    {
        return Mathf.Max(0f, 0.1f - 0.1f * Mathf.Exp(-1 * heightPreference_medium * 0.1f));
    }

    public float GetHighShotOverrideProbability()
    {
        return Mathf.Max(0f, 0.1f - 0.1f * Mathf.Exp(-1 * heightPreference_high * 0.1f));
    }
}