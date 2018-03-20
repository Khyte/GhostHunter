using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossessionTrigger : MonoBehaviour {

	public GameObject planeObject;


	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Ghost")
		{
			other.GetComponent<Ghost>().possession = gameObject;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Ghost")
		{
			other.GetComponent<Ghost>().possession = null;
		}
	}

}