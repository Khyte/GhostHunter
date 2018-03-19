using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Portes : MonoBehaviour {

	public GameObject porte1;
	public GameObject porte2;

	private Quaternion porte1Pos;
	private Quaternion porte2Pos;

	public Sprite closedDoor;
	public Sprite openDoor;

	private Image img;

	private bool closingDoor;
	private float timer;


	private void Start()
	{
		porte1Pos = porte1.transform.localRotation;
		if (porte2 != null)
		{
			porte2Pos = porte2.transform.localRotation;
		}

		img = GetComponent<Image>();
		img.sprite = closedDoor;
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

		if (porte1.transform.localRotation.z <= porte1Pos.z - 0.002f || porte1.transform.localRotation.z >= porte1Pos.z + 0.002f)
		{
			img.sprite = openDoor;
		}
		else if (porte2 != null)
		{
			if (porte2.transform.localRotation.z <= porte2Pos.z - 0.002f || porte2.transform.localRotation.z >= porte2Pos.z + 0.002f)
			{
				img.sprite = openDoor;
			}
			else
			{
				img.sprite = closedDoor;
			}
		}
		else
		{
			img.sprite = closedDoor;
		}
	}

}