using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntRune : MonoBehaviour {

	public Animation captureBox;

	private bool playAnim = false;


	void Update () {
		if (!playAnim)
		{
			playAnim = true;
			captureBox.Play();
		}
	}

}