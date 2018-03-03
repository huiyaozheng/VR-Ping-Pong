using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_HandSelection : MonoBehaviour
{

    public GameObject LeftHandButtonText;
    public GameObject RightHandButtonText;

    public void OnClick_Settings_Left()
    {
        ControllerSettings.SetPrimaryController(OVRInput.Controller.LTouch);
        LeftHandButtonText.GetComponent<TextMeshProUGUI>().text = "Left (active)";
        RightHandButtonText.GetComponent<TextMeshProUGUI>().text = "Right";
    }

    public void OnClick_Settings_Right()
    {
        ControllerSettings.SetPrimaryController(OVRInput.Controller.RTouch);
        LeftHandButtonText.GetComponent<TextMeshProUGUI>().text = "Left";
        RightHandButtonText.GetComponent<TextMeshProUGUI>().text = "Right (active)";
    }
}

