using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesColor : MonoBehaviour {

	private OrbColor orb;

	private ParticleSystem particleColor;


	private void Start()
	{
		particleColor = GetComponent<ParticleSystem>();
		orb = GameObject.FindGameObjectWithTag("GameController").GetComponent<OrbColor>();
	}

	void Update () {
		ParticleSystem.MainModule main = particleColor.main;
		main.startColor = orb.orbColor;
	}

}