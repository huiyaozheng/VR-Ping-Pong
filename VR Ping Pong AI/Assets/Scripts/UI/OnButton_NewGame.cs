using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnButton_NewGame : MonoBehaviour
{
    public List<GameObject> objectsToSetInactive, objectsToSetActive;

    public void OnClick()
    {
        foreach (GameObject go in objectsToSetActive)
            go.SetActive(true);
        foreach (GameObject go in objectsToSetInactive)
            go.SetActive(false);
    }
}