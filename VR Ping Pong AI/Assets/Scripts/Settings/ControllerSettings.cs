using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerSettings : MonoBehaviour
{
    private static OVRInput.Controller PrimaryController;

    private static OVRInput.Controller SecondaryController;

    public static OVRInput.Controller GetPrimaryController()
    {
        return PrimaryController;
    }

    public static void SetPrimaryController(OVRInput.Controller C)
    {
        if (C.Equals(OVRInput.Controller.LTouch))
        {
            PrimaryController = OVRInput.Controller.LTouch;
            SecondaryController = OVRInput.Controller.RTouch;
        }
        else
        {
            PrimaryController = OVRInput.Controller.RTouch;
            SecondaryController = OVRInput.Controller.LTouch;
        }
    }


    public static OVRInput.Controller GetSecondaryController()
    {
        return SecondaryController;
    }

    // Use this for initialization
    void Start()
    {
        PrimaryController = OVRInput.Controller.LTouch;
        SecondaryController = OVRInput.Controller.RTouch;
    }

    // Update is called once per frame
    void Update()
    {
    }
}