using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public bool startGame;

	// Timer et horloges
	public float globalTimer = 0;
	public GameObject pointer1;
	public GameObject pointer2;
	public Animator[] cogAnim;
	public GameObject smoke;

	// Victoire défaite
	public Image noWinner;
	public Image ghostWinner;
	public Image hunterWinner;
	public Image htcImg;
	public Text htcText;
	public Material htcTextColor;

	public Ghost ghost;
	public HeartRate hRate;

	public bool end = false;

	private float timer = 0;
	private float colorTimer = 0;
	private bool preEnd1 = false;
	private bool preEnd2 = false;
	private bool preEnd3 = false;


	private void Awake()
	{
		Screen.fullScreen = true;
		Screen.SetResolution(1920, 1080, true);
		htcText.text = "";
		htcTextColor.color = new Color(0.5f, 0.5f, 0.5f);
		AudioListener.volume = 1f;
	}

	void Update () {
		timer += Time.deltaTime;

		// Tuto
		if (timer < 10f)
		{
			ghost.life.fillAmount = 1;
		}

		if (timer >= 10f && !startGame)
		{
			startGame = true;
			for (int i = 0 ; i < cogAnim.Length ; i++)
			{
				cogAnim[i].enabled = true;
			}
			smoke.SetActive(true);
		}

		if (timer >= 10f && !preEnd1 && !preEnd2 && !preEnd3)
		{
			globalTimer += Time.deltaTime;
			pointer1.transform.localEulerAngles += new Vector3(0, 0, Time.deltaTime * 1.2f);
			pointer2.transform.localEulerAngles += new Vector3(0, 0, Time.deltaTime * 1.2f);
		}

		// Fin
		if (globalTimer >= 300f && !end && !preEnd2 && !preEnd3)
		{
			AudioListener.volume = 0f;
			preEnd1 = true;
			for (int i = 0 ; i < cogAnim.Length ; i++)
			{
				cogAnim[i].enabled = false;
			}
			smoke.SetActive(false);
			noWinner.color = Color.Lerp(new Color(0, 0, 0 , 0), Color.black, colorTimer);
			htcImg.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, colorTimer);
			if (colorTimer < 2)
			{
				colorTimer += Time.deltaTime / 2f;
			}
			else
			{
				noWinner.transform.GetChild(0).gameObject.SetActive(true);
				end = true;
				htcText.text = "The night has ended... \n You will have to come back next night";
				htcTextColor.color = new Color(0.5f, 0.5f, 0.5f);
			}
		}
		// Victoire fantome
		else if (hRate.life <= 0f && !end && !preEnd1 && !preEnd3)
		{
			AudioListener.volume = 0f;
			preEnd2 = true;
			for (int i = 0 ; i < cogAnim.Length ; i++)
			{
				cogAnim[i].enabled = false;
			}
			smoke.SetActive(false);
			ghostWinner.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, colorTimer);
			htcImg.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, colorTimer);
			if (colorTimer < 2)
			{
				colorTimer += Time.deltaTime / 2f;
			}
			else
			{
				ghostWinner.transform.GetChild(0).gameObject.SetActive(true);
				end = true;
				htcText.text = "The night has ended... \n You have lost your mind, \n and will never come back ...";
				htcTextColor.color = new Color(0.8f, 0.2f, 0.2f);
			}
		}
		// Victoire chasseur
		else if (ghost.life.fillAmount <= 0f && !end && !preEnd1 && !preEnd2)
		{
			AudioListener.volume = 0f;
			preEnd3 = true;
			for (int i = 0 ; i < cogAnim.Length ; i++)
			{
				cogAnim[i].enabled = false;
			}
			smoke.SetActive(false);
			hunterWinner.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, colorTimer);
			htcImg.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, colorTimer);
			if (colorTimer < 2)
			{
				colorTimer += Time.deltaTime / 2f;
			}
			else
			{
				hunterWinner.transform.GetChild(0).gameObject.SetActive(true);
				end = true;
				htcText.text = "The night has ended... \n You have allowed \n your friend to find peace";
				htcTextColor.color = new Color(0.2f, 0.8f, 0.2f);
			}
		}
	}
}
