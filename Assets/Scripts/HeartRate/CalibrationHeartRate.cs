using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalibrationHeartRate : MonoBehaviour
{

	public int calibHeartRate;
	public static CalibrationHeartRate s_singleton;
	public Text textCalib;

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
		Invoke("GetRes", 60f);
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
		calibHeartRate = resHeartRate / (60 - noValue);
		Debug.Log("Valeur calibrée : " + calibHeartRate);
		textCalib.text = calibHeartRate.ToString();
	}

}