using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : MonoBehaviour {

	public GameObject mansionRoom;


	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Room")
		{
			mansionRoom = other.gameObject;
		}
	}

}