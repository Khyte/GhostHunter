using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portes : MonoBehaviour {

	public GameObject porte1;
	public GameObject porte2;

	private Quaternion porte1Pos;
	private Quaternion porte2Pos;

	private bool closingDoor;
	private float timer;


	private void Start()
	{
		porte1Pos = porte1.transform.localRotation;
		if (porte2 != null)
		{
			porte2Pos = porte2.transform.localRotation;
		}
	}

	public void CloseDoor()
	{
		closingDoor = true;
	}

	void Update()
	{
		if (closingDoor)
		{
			timer += Time.deltaTime;
			porte1.transform.localRotation = Quaternion.Lerp(porte1.transform.localRotation, porte1Pos, Time.deltaTime * 5f);
			if (porte2 != null)
			{
				porte2.transform.localRotation = Quaternion.Lerp(porte2.transform.localRotation, porte2Pos, Time.deltaTime * 5f);
			}

			if (timer >= 2f)
			{
				closingDoor = false;
				timer = 0;
			}
		}
	}

}