﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartRate : MonoBehaviour {

	// Utile sans le MIO uniquement
	public Slider heartRateSlider;

	// MIO capteur cardiaque
	public GameObject MIO;

	public Slider deltaSlider;
	public float delta;

	// Matériaux
	public Material[] mat;

	private GameController gameC;
	private ThunderStorm thunderStorm;
	private HeartRateServer heartRate;
	private CalibrationHeartRate baseHeartRate;

	private float value;
	private float baseValue;


	private void Awake()
	{
		gameC = GetComponent<GameController>();
		thunderStorm = GetComponent<ThunderStorm>();
		if (MIO.activeInHierarchy)
		{
			heartRate = MIO.GetComponent<HeartRateServer>();
			baseHeartRate = MIO.GetComponent<CalibrationHeartRate>();
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
				baseValue = baseHeartRate.calibHeartRate;
				delta = 100 / (120 - baseValue);
			}
			else
			{
				value = heartRateSlider.value;
				baseValue = 60;
				delta = 100 / (120 - baseValue);
			}

			// Test delta
			if (value >= baseValue)
				deltaSlider.value = (value - baseValue) * delta;

			// Orage
			if ((value - baseValue) * delta >= 10)
			{
				thunderStorm.createThunder = true;
				thunderStorm.timerThunder = 1000 / ((value - baseValue) * delta);
			}
			else
			{
				thunderStorm.createThunder = false;
				thunderStorm.thunderLight.intensity = 0;
			}
		}

		// Materiaux
		if (value > baseValue)
		{
			for (int i = 0 ; i < mat.Length ; i++)
			{
				mat[i].SetColor("_SnowAccumulation", new Vector4((value - baseValue) * 0.01f * delta, 4f, 0, 0));
			}
		}
		else
		{
			for (int i = 0 ; i < mat.Length ; i++)
			{
				mat[i].SetColor("_SnowAccumulation", new Vector4(0, 4f, 0, 0));
			}
		}
	}

}