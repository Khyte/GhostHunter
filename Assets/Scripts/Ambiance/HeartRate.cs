using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartRate : MonoBehaviour {

	// Utile sans le MIO uniquement
	public Slider heartRateSlider;

	// MIO capteur cardiaque
	public GameObject MIO;

	// Matériaux
	public Material[] mat;

	private GameController gameC;
	private ThunderStorm thunderStorm;
	private HeartRateServer heartRate;

	private float value;


	private void Awake()
	{
		gameC = GetComponent<GameController>();
		thunderStorm = GetComponent<ThunderStorm>();
		if (MIO.activeInHierarchy)
		{
			heartRate = MIO.GetComponent<HeartRateServer>();
		}

		// Materiaux
		for (int i = 0 ; i < mat.Length ; i++)
		{
			mat[i].SetColor("_SnowAccumulation", new Vector4(0, 4f, 0, 0));
		}
	}

	void Update () {
		if (gameC.startGame)
		{
			if (MIO.activeInHierarchy)
			{
				value = heartRate.currentHeartRate;
			}
			else
			{
				value = heartRateSlider.value;
			}

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

		// Materiaux
		for (int i = 0 ; i < mat.Length ; i++)
		{
			mat[i].SetColor("_SnowAccumulation", new Vector4(-(120 - value*2) * 0.007f, 4f, 0, 0));
		}
	}

}