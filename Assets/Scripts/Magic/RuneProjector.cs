using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneProjector : MonoBehaviour {

	void Update () {
		transform.Rotate(Vector3.forward, Time.deltaTime * 50f);
	}

}