using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingGhost : MonoBehaviour {

	private Vector3 startPosition;
	private float timer;


	void Start () {
		startPosition = transform.localPosition;
	}
	
	void Update () {
		timer += Time.deltaTime;

		transform.localPosition += new Vector3(0, Mathf.Sin(Time.time) * 0.0012f, Time.deltaTime * 0.5f);
		if (timer >= 25f)
		{
			transform.localPosition = startPosition;
			timer = 0;
		}
	}

}