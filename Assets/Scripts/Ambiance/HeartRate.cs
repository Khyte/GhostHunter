using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class HeartRate : MonoBehaviour {

	// Utile sans le MIO uniquement
	public Slider heartRateSlider;

	// MIO capteur cardiaque
	public GameObject MIO;

	public Slider deltaSlider;
	public float delta;

	// Santé mentale
	public Slider lifeSlider;
	public float life;
	public Material[] lifeMat;

	// Matériaux
	public Material[] mat;

	// Vibrations
	public GameObject rightController;
	public GameObject leftController;

	// Sons
	public AudioSource ambiance;
	public AudioSource randomAmbiance;
	public AudioClip[] randomFear;
	public AudioSource randomSpatial;
	public AudioClip[] randomSpatialFear;

	private GameController gameC;
	private ThunderStorm thunderStorm;
	private HeartRateServer heartRate;
	private CalibrationHeartRate baseHeartRate;

	private float value;
	private float baseValue;

	private bool startSoundFear = true;
	private bool startSoundSpatial = true;


	private void Awake()
	{
		life = 100;

		for (int i = 0 ; i < lifeMat.Length ; i++)
		{
			lifeMat[i].color = new Color(0, 1, 0);
		}

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

		ambiance.volume = 0.1f;
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

			// Life delta >= 10
			if ((value - baseValue) * delta >= 10)
			{
				lifeSlider.value -= 0.000009f * ((value - baseValue) * delta) * Mathf.Exp(3);
			}

			// Thunder delta >= 20
			if ((value - baseValue) * delta >= 20)
			{
				thunderStorm.createThunder = true;
				thunderStorm.timerThunder = 1000 / ((value - baseValue) * delta);
			}
			else
			{
				thunderStorm.createThunder = false;
				thunderStorm.thunderLight.intensity = 0;
			}

			// Sounds delta >= 30
			if ((value - baseValue) * delta >= 30)
			{
				randomAmbiance.volume = ((value - baseValue) * delta * 0.01f) - 0.25f;
				if (!randomAmbiance.isPlaying && startSoundFear)
				{
					startSoundFear = false;
					randomAmbiance.clip = randomFear[Random.Range(0, randomFear.Length)];
					randomAmbiance.Play();
					StartCoroutine(StartRandFear());
				}
			}
			else
			{
				randomAmbiance.volume = 0;
			}

			// Sounds delta >= 60
			if ((value - baseValue) * delta >= 60)
			{
				randomSpatial.volume = ((value - baseValue) * delta * 0.01f) - 0.35f;
				if (!randomSpatial.isPlaying && startSoundSpatial)
				{
					startSoundSpatial = false;
					randomSpatial.clip = randomSpatialFear[Random.Range(0, randomSpatialFear.Length)];
					randomSpatial.Play();
					randomSpatial.transform.localPosition = (new Vector3(0, 5, 0)) + Random.insideUnitSphere * 15;
					randomSpatial.transform.localPosition = new Vector3(randomSpatial.transform.localPosition.x, 5, randomSpatial.transform.localPosition.z);
					StartCoroutine(StartRandSpatial());
				}
			}
			else
			{
				randomSpatial.volume = 0;
			}

			// Life
			life = lifeSlider.value;
			float lifeValue = life * 0.01f;
			if (lifeValue >= 0.05f)
			{
				for (int i = 0 ; i < lifeMat.Length ; i++)
				{
					lifeMat[i].color = new Color(1 - lifeValue, lifeValue, 0);
				}
			}
			else
			{
				for (int i = 0 ; i < lifeMat.Length ; i++)
				{
					lifeMat[i].color = new Color(0, 0, 0, 1);
				}
			}
			lifeMat[0].SetColor("_EmissionColor", lifeMat[0].color);

			// Sounds
			ambiance.volume = (value - baseValue) * 0.01f * delta;
			if (ambiance.volume <= 0.1f)
			{
				ambiance.volume = 0.1f;
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

	IEnumerator Rumble()
	{
		VRTK_ControllerReference controllerReference = VRTK_ControllerReference.GetControllerReference(rightController);
		VRTK_ControllerReference controllerReference2 = VRTK_ControllerReference.GetControllerReference(leftController);
		VRTK_ControllerHaptics.TriggerHapticPulse(controllerReference, 0.8f, 0.3f, 0.01f);
		VRTK_ControllerHaptics.TriggerHapticPulse(controllerReference2, 0.8f, 0.3f, 0.01f);
		yield return new WaitForSeconds(0.7f);
		VRTK_ControllerHaptics.TriggerHapticPulse(controllerReference, 0.8f, 0.3f, 0.01f);
		VRTK_ControllerHaptics.TriggerHapticPulse(controllerReference2, 0.8f, 0.3f, 0.01f);
	}

	IEnumerator StartRandFear()
	{
		yield return new WaitForSeconds(Random.Range(randomAmbiance.clip.length + 10f, randomFear.Length + 18f));
		startSoundFear = true;
	}

	IEnumerator StartRandSpatial()
	{
		yield return new WaitForSeconds(Random.Range(randomSpatial.clip.length + 10f, randomSpatialFear.Length + 18f));
		startSoundSpatial = true;
	}

}