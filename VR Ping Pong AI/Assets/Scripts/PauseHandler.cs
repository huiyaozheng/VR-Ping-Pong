using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseHandler : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        //Events.OnPause += HideObject();
        //Events.OnPause += ShowObject();
    }

    // Update is called once per frame
    void HideObject()
    {
        GetComponent<Renderer>().enabled = false;
    }

    void ShowObject()
    {
        GetComponent<Renderer>().enabled = true;
    }
}