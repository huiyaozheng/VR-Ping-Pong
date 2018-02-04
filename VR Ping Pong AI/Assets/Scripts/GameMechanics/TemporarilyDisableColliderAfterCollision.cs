using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TemporarilyDisableColliderAfterCollision : MonoBehaviour {

	private Collider myCollider;
	public float disabledTime = 0.2f;

	private float timer = 0.0f;

	void Start()
	{
		myCollider = gameObject.GetComponent<Collider>();
	}

	void Update()
	{
		if (timer > 0f)
		{
			timer += Time.deltaTime;
			if (timer > disabledTime)
			{
				timer = 0f;
				myCollider.enabled = true;
				Debug.Log("Enabled collider");
			}
		}
	}

	void OnCollisionEnter(Collision col)
	{
		myCollider.enabled = false;
		timer += 0.001f;
		Debug.Log("Disabled collider");
	}

}
