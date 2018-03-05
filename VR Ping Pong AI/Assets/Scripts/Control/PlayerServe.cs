using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the human player's serve.
/// </summary>
public class PlayerServe : MonoBehaviour
{
    public GameObject LeftHandAnchor;
    public GameObject RightHandAnchor;

    private static System.Boolean serveAllowed;
    private System.Boolean ServeInProgress;
    private OVRInput.Controller SecondaryController;

    // Use this for initialization
    void Start()
    {
        serveAllowed = true;
        ServeInProgress = false;
    }

    public static void ServeAllowed()
    {
        serveAllowed = true;
    }

    // Update is called once per frame
    void Update()
    {
        SecondaryController = ControllerSettings.GetSecondaryController();
        OVRInput.Button ServeButton = OVRInput.Button.One;

        if (serveAllowed && (OVRInput.Get(ServeButton, SecondaryController)))
        {
            if (SecondaryController.Equals(OVRInput.Controller.LTouch))
            {
                transform.position = LeftHandAnchor.transform.position;
            }
            else
            {
                transform.position = RightHandAnchor.transform.position;
            }
            ServeInProgress = true;
        }
        else
        {
            if (ServeInProgress)
            {
                Vector3 ServeVelocity = new Vector3(0f, 10f, 0f);
                GetComponent<Rigidbody>().velocity = ServeVelocity;
                ServeInProgress = false;
                serveAllowed = false;
                RallyState.playerWillServeNow = true;
            }
        }
    }
}