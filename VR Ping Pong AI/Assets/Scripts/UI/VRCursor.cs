using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the VR cursor used to interact with the menu.
/// </summary>
public class VRCursor : MonoBehaviour
{
    public GameObject Cursor;
    public GameObject Canvas;

    // Update is called once per frame
    void Update()
    {
        Collider coll = Canvas.GetComponent<Collider>();
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        // Checks if ray hits canvas and only then displays the cursor
        if (coll.Raycast(ray, out hit, 1000f))
        {
            Cursor.SetActive(true);
            Cursor.transform.position = hit.point;
        }
        else
        {
            Cursor.SetActive(false);
        }
    }
}