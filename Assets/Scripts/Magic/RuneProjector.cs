using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneProjector : MonoBehaviour {

	public OrbColor orb;

	private Light lightColor;


	private void Start()
	{
		lightColor = GetComponent<Light>();
	}

	void Update () {
		lightColor.color = orb.orbColor;
		transform.Rotate(Vector3.forward, Time.deltaTime * 50f);
	}

}