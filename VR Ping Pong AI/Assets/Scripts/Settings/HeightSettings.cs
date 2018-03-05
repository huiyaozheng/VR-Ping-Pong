using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Records height of the player (from Oculus).
/// </summary>
public class HeightSettings : MonoBehaviour
{
    private static int Height;

    public static int GetHeight()
    {
        return Height;
    }

    public static void SetHeight(int h)
    {
        Height = h;
    }
}