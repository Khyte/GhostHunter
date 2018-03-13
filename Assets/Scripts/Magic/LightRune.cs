using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRune : MonoBehaviour {

	public LightFlicker[] lights;

	public Hunter hunter;

	public List<LightFlicker> lightsRoom = new List<LightFlicker>();
	private bool activateLight = false;
	private float timer = 0;


	public void ActivateLights()
	{
		int roomNbr = 0;
		string s1 = hunter.mansionRoom.name;
		string s2 = s1.Substring(s1.Length - 1);
		roomNbr = int.Parse(s2);

		RoomNumber(roomNbr);
	}

	private void RoomNumber(int roomNbr)
	{
		for (int i = (roomNbr * 3) - 3 ; i <= (roomNbr * 3) - 1 ; i++)
		{
			lightsRoom.Add(lights[i]);
		}

		activateLight = true;
	}

	private void Update()
	{
		if (activateLight)
		{
			timer += Time.deltaTime;

			for (int i = 0 ; i < lightsRoom.Count ; i++)
			{
				if (lightsRoom[i]._baseIntensity <= 1.5f)
				{
					lightsRoom[i]._baseIntensity += Time.deltaTime * 0.4f;
				}
			}

			if (timer >= 3f)
			{
				activateLight = false;
				timer = 0;
				lightsRoom.Clear();
			}
		}
	}

}