using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuInteraction : MonoBehaviour
{
    public GameObject SettingsMenu;

    public GameObject LeftHandButton;
    public GameObject RightHandButton;

    private GameObject CurrentObject;
    private Color DefaultButtonColor;
    private Color SelectedButtonColor;

    private int frameCount;

    // Use this for initialization
    void Start()
    {
        frameCount = 0;
        DefaultButtonColor = new Color(180f / 255, 189f / 255, 240f / 255);
        SelectedButtonColor = new Color(140f / 255, 149f / 255, 200f / 255);
    }

    // Update is called once per frame
    void Update()
    {
        frameCount = (frameCount + 1) % 21;
        RaycastHit Hit;
        GameObject ObjectHit;

        Vector3 direction = transform.TransformDirection(Vector3.forward) * 200;
        Debug.DrawRay(transform.position, direction, Color.red);

        if (Physics.Raycast(transform.position, direction, out Hit))
        {
            ObjectHit = Hit.collider.gameObject;

            if (ObjectHit.GetComponent<Button>() != null)
            {
                if (CurrentObject == null)
                {
                    CurrentObject = ObjectHit;
                    CurrentObject.GetComponent<Image>().color = SelectedButtonColor;
                }
                else if (!CurrentObject.Equals(ObjectHit))
                {
                    CurrentObject.GetComponent<Image>().color = DefaultButtonColor;
                    CurrentObject = ObjectHit;
                    CurrentObject.GetComponent<Image>().color = SelectedButtonColor;
                }
                else
                {
                    CurrentObject.GetComponent<Image>().color = SelectedButtonColor;
                }

                if ((frameCount % 7 == 0) &&
                    ((OVRInput.Get(OVRInput.Button.One, ControllerSettings.GetPrimaryController())) ||
                     (OVRInput.Get(OVRInput.Button.One, ControllerSettings.GetSecondaryController()))))
                {
                    CurrentObject.GetComponent<Button>().onClick.Invoke();
                }
            }
            else
            {
                //Non button object hit
                if (CurrentObject != null)
                {
                    CurrentObject.GetComponent<Image>().color = DefaultButtonColor;
                }
            }
        }
        else
        {
            // No object hit
            if (CurrentObject != null)
            {
                CurrentObject.GetComponent<Image>().color = DefaultButtonColor;
                CurrentObject = null;
            }
        }

        if ((SettingsMenu.active) && (frameCount == 0))
        {
            if (((OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > 0f) ||
                 ((OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0f))))
            {
                HeightSettings.SetHeight(HeightSettings.GetHeight() + 1);
            }

            if (((OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > 0f) ||
                 ((OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > 0f))))
            {
                HeightSettings.SetHeight(HeightSettings.GetHeight() - 1);
            }
        }
    }
}