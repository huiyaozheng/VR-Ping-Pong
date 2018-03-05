using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Updates the height label.
/// </summary>
public class HeightLabel : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = "Height: " + HeightSettings.GetHeight().ToString();
    }
}