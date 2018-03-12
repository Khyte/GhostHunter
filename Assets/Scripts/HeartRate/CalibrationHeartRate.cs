using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrationHeartRate : MonoBehaviour
{

	public int calibHeartRate;
	public static CalibrationHeartRate s_singleton;

	private int resHeartRate;
	private float timer;
	private int intTimer;
	private int noValue;

	private bool stopCalibration;

	private void Awake()
	{
		if (s_singleton == null)
		{
			s_singleton = this;
		}
		else
		{
			Destroy(this);
		}

	}

	void Start()
	{
		InvokeRepeating("GetCalibration", 0, 1f);
		Invoke("GetRes", 10f);
	}

	void GetCalibration()
	{
		if (!stopCalibration)
		{
			if (HeartRateServer.singleton.currentHeartRate != 0)
			{
				resHeartRate += HeartRateServer.singleton.currentHeartRate;
			}
			else
			{
				noValue++;
			}
		}
	}

	void GetRes()
	{
		stopCalibration = true;
		calibHeartRate = resHeartRate / (10 - noValue);
		Debug.Log("Valeur calibrée : " + calibHeartRate);
	}

}