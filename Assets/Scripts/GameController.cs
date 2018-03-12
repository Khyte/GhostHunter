﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public bool startGame;

	private float timer = 0;
	

	void Update () {
		timer += Time.deltaTime;

		if (timer >= 10f && !startGame)
		{
			startGame = true;
		}
	}
}
