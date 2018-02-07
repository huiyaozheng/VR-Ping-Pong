using UnityEngine;
using System.Collections;

public class User_Bat : MonoBehaviour {

    public Rigidbody rigidbody;
    public float xScale = 0.1f;
    public float yScale = 0.1f;

    void Start () {
        rigidbody = GetComponent<Rigidbody>() as Rigidbody;
    }

    void Update () {
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");
        rigidbody.MovePosition(rigidbody.position + transform.rotation * new Vector3(h*xScale, 0, v*yScale));
    }

}