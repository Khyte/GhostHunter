using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStorm : MonoBehaviour {

	public GameObject[] thunderLights;
	public Skybox nightRain;

	public AudioClip[] thunderSound;
	public AudioSource thunder;

	public bool createThunder;

	private float timer = 0;
	

	void Update () {
		if (Input.GetKeyDown(KeyCode.Backspace))
		{
			createThunder = true;
		}
		if (createThunder)
		{
			timer += Time.deltaTime;
			// Audio
			thunder.clip = thunderSound[Random.Range(0, thunderSound.Length)];
			thunder.Play();
			if (timer >= Random.Range(0.5f, 0.6f))
			{
				foreach(GameObject lights in thunderLights)
				{
					lights.SetActive(true);
				}
				RenderSettings.skybox.SetColor("_Tint", new Color(1, 1, 1));
			}
			if (timer >= Random.Range(0.7f, 0.8f))
			{
				foreach (GameObject lights in thunderLights)
				{
					lights.SetActive(false);
				}
				RenderSettings.skybox.SetColor("_Tint", new Color(0.3f, 0.3f, 0.3f));
			}
			if (timer >= Random.Range(0.8f, 0.9f))
			{
				foreach (GameObject lights in thunderLights)
				{
					lights.SetActive(true);
				}
				RenderSettings.skybox.SetColor("_Tint", new Color(1, 1, 1));
			}
			if (timer >= Random.Range(1.0f, 1.1f))
			{
				foreach (GameObject lights in thunderLights)
				{
					lights.SetActive(false);
				}
				RenderSettings.skybox.SetColor("_Tint", new Color(0.3f, 0.3f, 0.3f));
			}
			if (timer >= Random.Range(1.2f, 1.3f))
			{
				foreach (GameObject lights in thunderLights)
				{
					lights.SetActive(true);
				}
				RenderSettings.skybox.SetColor("_Tint", new Color(1, 1, 1));
			}
			if (timer >= Random.Range(1.4f, 1.5f))
			{
				foreach (GameObject lights in thunderLights)
				{
					lights.SetActive(false);
				}
				RenderSettings.skybox.SetColor("_Tint", new Color(0.3f, 0.3f, 0.3f));
				createThunder = false;
				timer = 0;
			}
		}	
	}

}