using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartRate : MonoBehaviour {

	// A supprimer à l'implentation du MIO
	public Slider heartRateSlider;

	private ThunderStorm thunderStorm;


	private void Awake()
	{
		thunderStorm = GetComponent<ThunderStorm>();
	}

	void Update () {
		float value = heartRateSlider.value;

		// Orage
		if (value >= 90)
		{
			thunderStorm.createThunder = true;
			thunderStorm.fearValue = 130 - value;
		}
		else
		{
			thunderStorm.createThunder = false;
		}
	}

}