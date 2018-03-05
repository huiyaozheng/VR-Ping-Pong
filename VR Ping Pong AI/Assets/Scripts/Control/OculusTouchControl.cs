using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OculusTouchControl : MonoBehaviour
{
    private OVRInput.Controller ActiveController;

    // Update is called once per frame
    void Update()
    {
        ActiveController = ControllerSettings.GetPrimaryController();

        transform.localPosition = OVRInput.GetLocalControllerPosition(ActiveController);
        transform.localRotation = OVRInput.GetLocalControllerRotation(ActiveController);

        if (ActiveController.Equals(OVRInput.Controller.LTouch))
        {
            Vector3 RotationCorrection = new Vector3(0f, -90f, -70f);
            transform.Rotate(RotationCorrection);
        }
        else
        {
            Vector3 RotationCorrection = new Vector3(0f, 90f, 70f);
            transform.Rotate(RotationCorrection);
        }
    }
}