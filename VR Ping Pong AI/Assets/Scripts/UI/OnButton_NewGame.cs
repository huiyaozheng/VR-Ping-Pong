using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Set relevant objects active when a new game starts.
/// </summary>
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