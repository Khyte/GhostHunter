using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntRune : MonoBehaviour {

	public Animation captureBox;
	public Material captureBoxMat;

	private Shader actualShader;
	private Shader standardShader;

	private float sliceValue = 1;
	private bool playAnim = false;


	private void Awake()
	{
		actualShader = Shader.Find("Dissolving");
		standardShader = Shader.Find("Standard");

		captureBoxMat.shader = actualShader;
	}

	void Update () {
		if (sliceValue >= 0.01f)
		{
			sliceValue -= 0.5f * Time.deltaTime;
			captureBoxMat.SetFloat("_SliceAmount", sliceValue);
		}
		else if (!playAnim)
		{
			captureBoxMat.shader = standardShader;
			playAnim = true;
			captureBox.Play();
		}
	}

}