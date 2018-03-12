using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainSound : MonoBehaviour {

	public AudioSource rainSound;

	public GameObject balcon;

	public float distance;


	void Update () {
		distance = Vector3.Distance(transform.position, balcon.transform.position);
		if (distance > 14f)
		{
			rainSound.volume = 0;
		}
		else {
			rainSound.volume = (1f / distance);
		}
	}

}