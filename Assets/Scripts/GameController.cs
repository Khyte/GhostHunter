using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public bool startGame;

	// Timer et horloges
	public float globalTimer = 0;
	public GameObject pointer1;
	public GameObject pointer2;
	public Animator cogAnim;
	public GameObject smoke;

	private float timer = 0;


	private void Awake()
	{
		Screen.fullScreen = true;
		Screen.SetResolution(1920, 1080, true);
	}

	void Update () {
		timer += Time.deltaTime;

		if (timer >= 10f && !startGame)
		{
			startGame = true;
			cogAnim.enabled = true;
			smoke.SetActive(true);
		}

		if (timer >= 10f)
		{
			globalTimer += Time.deltaTime;
			pointer1.transform.localEulerAngles += new Vector3(0, 0, Time.deltaTime * 1.2f);
			pointer2.transform.localEulerAngles += new Vector3(0, 0, Time.deltaTime * 1.2f);
		}
	}
}
