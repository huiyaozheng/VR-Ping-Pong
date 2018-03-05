using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Normal distribution function.
/// </summary>
public class OurMath
{
    public static float sampleNormalDistribution(float mean, float stdDev)
    {
        float u1 = 1.0f - Random.value; //uniform(0,1] random floats
        float u2 = 1.0f - Random.value;
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2); //random normal(0,1)
        return mean + stdDev * randStdNormal; //random normal(mean,stdDev^2)
    }
}