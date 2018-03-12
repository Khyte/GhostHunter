using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbColor : MonoBehaviour {

	public Color orbColor;

	public Material orbMat;
	public Light orbLight;


	void Update () {
		//Orb
		float emission = 0.1f + Mathf.PingPong(Time.time * 0.8f, 3f - 0.1f);
		Color baseColor = orbColor; //Replace this with whatever you want for your base color at emission level '1'
		Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);
		orbMat.SetColor("_EmissionColor", finalColor);

		//Light
		float lightEmission = 0.1f + Mathf.PingPong(Time.time * 0.4f, 1.5f - 0.1f);
		orbLight.color = orbColor;
		orbLight.intensity = lightEmission;
	}

}