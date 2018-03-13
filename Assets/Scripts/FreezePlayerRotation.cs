using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezePlayerRotation : MonoBehaviour {

	private Quaternion initRotation;
 
	void Start()
	{
		initRotation = transform.rotation;
	}

	void LateUpdate()
	{
		transform.rotation = initRotation;
	}

}