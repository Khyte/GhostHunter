using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStorm : MonoBehaviour {

	// Directional light
	public Light thunderLight;

	// Sons
	public AudioClip[] thunderSound;
	public AudioSource thunder;

	public bool createThunder;
	public float fearValue;

	private float timer = 0;
	

	void Update () {
		if (createThunder)
		{
			timer += Time.deltaTime;

			if (timer >= Random.Range(0.5f, 0.6f))
			{
				thunderLight.intensity = 1f;
				RenderSettings.skybox.SetColor("_Tint", new Color(1, 1, 1));
			}
			if (timer >= Random.Range(0.7f, 0.8f))
			{
				thunderLight.intensity = 0f;
				RenderSettings.skybox.SetColor("_Tint", new Color(0.3f, 0.3f, 0.3f));
			}
			if (timer >= Random.Range(0.8f, 0.9f))
			{
				thunderLight.intensity = 1f;
				RenderSettings.skybox.SetColor("_Tint", new Color(1, 1, 1));
			}
			if (timer >= Random.Range(1.0f, 1.1f))
			{
				thunderLight.intensity = 0f;
				RenderSettings.skybox.SetColor("_Tint", new Color(0.3f, 0.3f, 0.3f));
			}
			if (timer >= Random.Range(1.2f, 1.3f))
			{
				thunderLight.intensity = 1f;
				RenderSettings.skybox.SetColor("_Tint", new Color(1, 1, 1));
			}
			if (timer >= Random.Range(1.4f, 1.5f))
			{
				thunderLight.intensity = 0f;
				RenderSettings.skybox.SetColor("_Tint", new Color(0.3f, 0.3f, 0.3f));
			}

			// Audio
			if (timer >= 2f && timer <= 2.1f && !thunder.isPlaying)
			{
				thunder.clip = thunderSound[Random.Range(0, thunderSound.Length)];
				thunder.Play();
			}

			if (timer >= Random.Range(fearValue, fearValue + 5f))
			{
				timer = 0;
			}
		}	
	}

}