using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class DisableRendererOnStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<MeshRenderer>().enabled = false;	
	}
}
