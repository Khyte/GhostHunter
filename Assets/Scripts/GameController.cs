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
		winnerImg.transform.GetChild(0).GetComponent<Text>().text = "";
		htcTextColor.color = new Color(0.5f, 0.5f, 0.5f);
		AudioListener.volume = 1f;
		winnerMat.color = new Color(0, 0, 0, 0);
	}

	void Update () {
		timer += Time.deltaTime;

		// Tuto
		if (timer <= 9f)
		{
			tutoText.text = "Prenez vos marques dans le manoir, le temps que votre peur soit calibrée. Le premier gong sonne le début des 5 minutes.";
		}
		else if (timer > 9f && timer <= 10f)
		{
			tutoText.material.color = Color.Lerp(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), 1);
		}
		// Tuto 2
		else if (timer > 10f && timer <= 20f)
		{
			tutoText.text = "Le grimoire permet de choisir la magie, et l'orbe permet de lancer le sort.";
			tutoText.material.color = Color.Lerp(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 1);
		}
		else if (timer > 20f && timer <= 21f)
		{
			tutoText.material.color = Color.Lerp(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), 1);
		}
		// Tuto 3
		else if (timer > 21f && timer <= 30f)
		{
			tutoText.text = "Glissez votre doigt de gauche à droite ou inversement sur le pad tactile de la manette qui tient le grimoire pour changer de sort.";
			tutoText.material.color = Color.Lerp(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 1);
		}
		else if (timer > 30f && timer <= 31f)
		{
			tutoText.material.color = Color.Lerp(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), 1);
		}
		// Tuto 4
		else if (timer > 31f && timer <= 40f)
		{
			tutoText.text = "Utilisez le bouton à l'arrière (trigger) pour ouvrir le livre et avoir des informations sur les sorts.";
			tutoText.material.color = Color.Lerp(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 1);
		}
		else if (timer > 40f && timer <= 41f)
		{
			tutoText.material.color = Color.Lerp(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), 1);
		}
		// Tuto 5
		else if (timer > 41f && timer <= 50f)
		{
			tutoText.text = "Cliquez sur le pad tactile de la manette qui tient l'orbe pour préparer le sort, et utilisez le trigger pour le lancer.";
			tutoText.material.color = Color.Lerp(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 1);
		}
		else if (timer > 50f && timer <= 51f)
		{
			tutoText.material.color = Color.Lerp(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), 1);
		}
		// Tuto 6
		else if (timer > 51f && timer < 60f)
		{
			tutoText.text = "Utilisez les boutons sur les côtés (grip) pour ouvrir les portes. Préparez-vous, ça va commencer...";
			tutoText.material.color = Color.Lerp(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 1);
		}

		// Vie du fantôme bloquée
		if (timer < 60f)
		{
			ghost.life.fillAmount = 1;
		}

		if (timer >= 60f && !startGame)
		{
			tutoText.material.color = Color.Lerp(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), 1);
			tutoText.text = "";
			gongsPlay.clip = gongs[0];
			gongsPlay.Play();
			startGame = true;
			for (int i = 0 ; i < cogAnim.Length ; i++)
			{
				cogAnim[i].enabled = true;
			}
			smoke.SetActive(true);
		}

		if (timer >= 60f && !preEnd1 && !preEnd2 && !preEnd3)
		{
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
