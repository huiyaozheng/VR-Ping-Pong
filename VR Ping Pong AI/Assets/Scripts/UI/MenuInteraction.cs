using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuInteraction : MonoBehaviour
{
    public GameObject MainMenu;

    //public GameObject NewGameMenu;
    public GameObject SettingsMenu;

    private GameObject CurrentObject;
    private Color DefaultButtonColor;
    private Color SelectedButtonColor;

    // Use this for initialization
    void Start()
    {
        DefaultButtonColor = new Color(0xB4, 0xBD, 0xF0, 0xFF);
        SelectedButtonColor = new Color(0, 255, 0, 0xFF);
    }

    // Update is called once per frame
    void Update()
    {
        if (MainMenu.activeSelf)
            MainMenuInteraction();
        else if (SettingsMenu.activeSelf)
            SettingsMenuInteraction();
    }

    void MainMenuInteraction()
    {
        RaycastHit Hit;
        GameObject ObjectHit;

        Vector3 direction = transform.TransformDirection(Vector3.forward) * 200;
        Debug.DrawRay(transform.position, direction, Color.red);

        if (Physics.Raycast(transform.position, direction, out Hit))
        {
            ObjectHit = Hit.collider.gameObject;
            // Ensure object is part of the menu interface
            if ((ObjectHit.name.Equals("NewGameButton")) || (ObjectHit.name.Equals("CreditsButton"))
                || (ObjectHit.name.Equals("SettingsButton")) || (ObjectHit.name.Equals("ExitButton")))
            {
                // Reset deselected object to original colour
                if (CurrentObject == null)
                {
                    CurrentObject = ObjectHit;
                    CurrentObject.GetComponent<Image>().color = SelectedButtonColor;
                }
                else
                {
                    if (!(CurrentObject.Equals(ObjectHit)))
                    {
                        CurrentObject.GetComponent<Image>().color = DefaultButtonColor;
                        CurrentObject = ObjectHit;
                        CurrentObject.GetComponent<Image>().color = SelectedButtonColor;
                    }
                }

                if (OVRInput.Get(OVRInput.Button.One, ControllerSettings.GetPrimaryController()))
                {
                    switch (CurrentObject.name)
                    {
                        case ("NewGameButton"):
                            break;
                        case ("SettingsButton"):
                            SettingsMenu.SetActive(true);
                            MainMenu.SetActive(false);
                            break;
                        case ("CreditsButton"):
                            break;
                        case ("ExitButton"):
                            Application.Quit();
                            break;
                    }
                }
            }
            else
            {
                if (CurrentObject != null)
                {
                    CurrentObject.GetComponent<Image>().color = DefaultButtonColor;
                }
            }
        }
        else
        {
            if (CurrentObject != null)
            {
                CurrentObject.GetComponent<Image>().color = DefaultButtonColor;
            }
        }
    }

    void NewGameMenuInteraction()
    {
    }

    void SettingsMenuInteraction()
    {
        RaycastHit Hit;
        GameObject ObjectHit;

        Vector3 direction = transform.TransformDirection(Vector3.forward) * 200;
        Debug.DrawRay(transform.position, direction, Color.red);

        if (Physics.Raycast(transform.position, direction, out Hit))
        {
            ObjectHit = Hit.collider.gameObject;
            // Ensure object is part of the menu interface
            if ((ObjectHit.name.Equals("LeftHandButton")) || (ObjectHit.name.Equals("RightHandButton"))
                || (ObjectHit.name.Equals("SettingsBackButton")))
            {
                // Reset deselected object to original colour
                if (CurrentObject == null)
                {
                    CurrentObject = ObjectHit;
                    Image imageHit = CurrentObject.GetComponent<Image>();
                    if (imageHit != null)
                        imageHit.color = SelectedButtonColor;
                }
                else
                {
                    if (!(CurrentObject.Equals(ObjectHit)))
                    {
                        Image currentImage = CurrentObject.GetComponent<Image>();
                        if (currentImage != null)
                            currentImage.color = DefaultButtonColor;

                        CurrentObject = ObjectHit;

                        Image imageHit = CurrentObject.GetComponent<Image>();
                        if (imageHit != null)
                            imageHit.color = SelectedButtonColor;
                    }
                }

                if (OVRInput.Get(OVRInput.Button.One, ControllerSettings.GetPrimaryController()))
                {
                    switch (CurrentObject.name)
                    {
                        case ("SettingBackButton"):
                            MainMenu.SetActive(true);
                            SettingsMenu.SetActive(false);
                            break;
                        case ("LeftHandButton"):
                            ControllerSettings.SetPrimaryController(OVRInput.Controller.LTouch);
                            break;
                        case ("RightHandButton"):
                            ControllerSettings.SetPrimaryController(OVRInput.Controller.RTouch);
                            break;
                    }
                }

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
            else
            {
                if (CurrentObject != null)
                {
                    CurrentObject.GetComponent<Image>().color = DefaultButtonColor;
                }
            }
        }
        else
        {
            if (CurrentObject != null)
            {
                CurrentObject.GetComponent<Image>().color = DefaultButtonColor;
            }
        }
    }
}