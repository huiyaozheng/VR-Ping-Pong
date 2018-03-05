using UnityEngine;

/// <summary>
/// Functions to determine trajectory and normal.
/// </summary>
public static class OurPhysics
{
    /// <summary>
    /// Cheat and change the velocity of the ball so that it hits the target and flies at a give maximum height
    /// Works given assumption that table is at height 0!
    /// </summary>
    /// <param name="target">Target landing position</param>
    /// <param name="currPos">Current starting position of the trajectory</param>
    /// <param name="maxHeightTarget">Maximum height during the flight</param>
    /// <param name="g">value of gravity</param>
    /// <returns></returns>
    public static Vector3 velFromTraj(Vector3 target, Vector3 currPos, float maxHeightTarget, float g,
        bool maxHeightBehind)
    {
        float H = Mathf.Max(maxHeightTarget, currPos.y + 0.01f);
        float h0 = currPos.y;
        Vector3 dist = currPos - target;
        dist.y = 0;
        float l = dist.magnitude; // distance to target
        float v_y = Mathf.Sqrt((H - h0) * 2 * g);
        if (maxHeightBehind) v_y = -v_y;
        // if (maxHeightBehind) v_y = -v_y;
        float v_x = l * g / (v_y + Mathf.Sqrt(2 * g * H));
        Vector3 vel = target - currPos;
        vel.y = 0;
        vel = vel.normalized;
        // if (maxHeightBehind){
        //     vel.y = -v_y;
        // } else {
        vel.y = v_y;
        // }
        vel.x *= v_x;
        vel.z *= v_x;
        return vel;
    }

    /// <summary>
    /// Returns the adjusted normal
    /// </summary>
    /// <param name="batVel"></param>
    /// <param name="batNorm">normalised normal of the bat surface</param>
    /// <returns></returns>
    public static Vector3 batHit(Vector3 batVel, Vector3 batNorm)
    {
        Vector3 vel = batVel + batNorm;
        return vel;
    }
}