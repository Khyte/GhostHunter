using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public bool startGame;

	// Timer et horloges
	public float globalTimer = 0;
	public GameObject pointer1;
	public GameObject pointer2;
	public Animator[] cogAnim;
	public GameObject smoke;
	public AudioSource gongsPlay;
	public AudioClip[] gongs;

	// Victoire défaite
	public Material winnerMat;
	public Image winnerImg;
	public Image htcImg;
	public Text htcText;
	public Material htcTextColor;

	// Tuto
	public Text tutoText;
	public GameObject ghostCam;
	public GameObject tutoObject;
	public NavMeshAgent ghostNav;
	public Material skybox1;
	public Material skybox2;
	public GameObject tutoDone;
	public GameObject ghostTuto;
	public GameObject ghostTutoText;

	public Ghost ghost;
	public HeartRate hRate;

	public bool end = false;

	private float timer = 0;
	private float colorTimer = 0;
	private float colorDuration = 0;
	private bool preEnd1 = false;
	private bool preEnd2 = false;
	private bool preEnd3 = false;


	private void Awake()
	{
		Screen.fullScreen = true;
		Screen.SetResolution(1920, 1080, true);
		htcText.text = "";
		winnerImg.transform.GetChild(0).GetComponent<Text>().text = "";
		htcTextColor.color = new Color(0.5f, 0.5f, 0.5f);
		AudioListener.volume = 1f;
		winnerMat.color = new Color(0, 0, 0, 0);
		htcImg.color = new Color(0, 0, 0, 0);
		RenderSettings.skybox = skybox1;
	}

	void Update () {
		timer += Time.deltaTime;

		// 55 à 60
		//
		//
		if (timer > 55f && timer < 60f)
		{
			winnerImg.transform.GetChild(0).GetComponent<Text>().material.color = new Color(0.5f, 0.5f, 0.5f);
			winnerImg.transform.GetChild(0).GetComponent<Text>().text = "La nuit tombe...";
			tutoText.text = "La nuit tombe...";
			colorDuration += Time.deltaTime / 4f;
			htcImg.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, colorDuration);
			winnerMat.color = Color.Lerp(new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), colorDuration);
		}

		// Vie du fantôme bloquée
		if (timer < 60f)
		{
			ghost.life.fillAmount = 1;
		}

		// Désaffichage du tuto
		if (timer > 40f)
		{
			ghostTuto.SetActive(false);
		}
		if (timer >= 60f)
		{
			ghostTutoText.SetActive(false);
		}

		// >= 60
		//
		//
		// Fin tuto
		if (timer >= 60f && !startGame)
		{
			tutoDone.SetActive(true);
			colorDuration = 0;
			RenderSettings.skybox = skybox2;
			tutoObject.SetActive(false);
			ghostNav.enabled = false;
			ghost.transform.localPosition = new Vector3(0, 0.8f, 0);
			//ghost.transform.localPosition = Random.insideUnitSphere * 10f;
			//ghost.transform.localPosition = new Vector3(transform.localPosition.x, 0.8f, transform.localPosition.z);
			ghostNav.enabled = true;
			ghostCam.transform.localPosition = new Vector3(0, 50, 0.3f);
			tutoText.text = "";
			winnerImg.transform.GetChild(0).GetComponent<Text>().text = "";
			gongsPlay.clip = gongs[0];
			gongsPlay.Play();
			startGame = true;
			for (int i = 0 ; i < cogAnim.Length ; i++)
			{
				cogAnim[i].enabled = true;
			}
			smoke.SetActive(true);
		}

		// >= 60
		//
		//
		if (timer >= 60f && !preEnd1 && !preEnd2 && !preEnd3)
		{
			colorDuration += Time.deltaTime / 3f;
			winnerMat.color = Color.Lerp(Color.black, new Color(0, 0, 0, 0), colorDuration);
			htcImg.color = Color.Lerp(new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), colorDuration);
			globalTimer += Time.deltaTime;
			pointer1.transform.localEulerAngles += new Vector3(0, 0, Time.deltaTime * 1.2f);
			pointer2.transform.localEulerAngles += new Vector3(0, 0, Time.deltaTime * 1.2f);
		}

		// 1 min left
		if (globalTimer >= 240f && globalTimer < 241f && !gongsPlay.isPlaying)
		{
			gongsPlay.clip = gongs[1];
			gongsPlay.Play();
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
			winnerMat.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, colorTimer);
			htcImg.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, colorTimer);
			if (colorTimer < 2)
			{
				colorTimer += Time.deltaTime / 2f;
			}
			else
			{
				winnerImg.transform.GetChild(0).GetComponent<Text>().material.color = new Color(0.5f, 0.5f, 0.5f);
				winnerImg.transform.GetChild(0).GetComponent<Text>().text = "The night has ended... \n The hunter is gone, \n but will return the next night";
				end = true;
				htcText.text = "The night has ended... \n You will have to \n come back next night";
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
			winnerMat.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, colorTimer);
			htcImg.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, colorTimer);
			if (colorTimer < 2)
			{
				colorTimer += Time.deltaTime / 2f;
			}
			else
			{
				winnerImg.transform.GetChild(0).GetComponent<Text>().material.color = new Color(0.1f, 0.3f, 0.1f);
				winnerImg.transform.GetChild(0).GetComponent<Text>().text = "The night has ended... \n The hunter has gone crazy \n and will never come back";
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
			winnerMat.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, colorTimer);
			htcImg.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, colorTimer);
			if (colorTimer < 2)
			{
				colorTimer += Time.deltaTime / 2f;
			}
			else
			{
				winnerImg.transform.GetChild(0).GetComponent<Text>().material.color = new Color(0.6f, 0f, 0f);
				winnerImg.transform.GetChild(0).GetComponent<Text>().text = "The night has ended... \n You have been sent back \n to the afterlife";
				end = true;
				htcText.text = "The night has ended... \n You have allowed \n your friend to find peace";
				htcTextColor.color = new Color(0.2f, 0.8f, 0.2f);
			}
		}
	}
}
