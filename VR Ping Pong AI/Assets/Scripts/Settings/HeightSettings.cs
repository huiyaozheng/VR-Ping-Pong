using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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