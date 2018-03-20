using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostMagic : MonoBehaviour {

	public GhostSpells ghostSpell;
	public RuneDrawing runeDraw;

	public GameController gameC;

	// Cooldowns
	public Image[] imagesCD;
	public float[] timerCD;

	// Lumière
	public LightRune lights;
	public Ghost ghost;
	public List<LightFlicker> lightsRoom = new List<LightFlicker>();
	private bool activateLight = false;
	private float timer = 0;


	private void Update()
	{
		if (runeDraw.runeDone)
		{
			runeDraw.runeDone = false;

			if (gameC.startGame)
			{
				// Possession
				if (ghostSpell.spellNbr == 1)
				{
					Possession();
				}
				// Lumière
				else if (ghostSpell.spellNbr == 2)
				{
					LightDown();
				}
				// Teleport
				else if (ghostSpell.spellNbr == 3)
				{
					Teleport();
				}
				// Taunt
				else if (ghostSpell.spellNbr == 4)
				{
					Taunt();
				}
			}
		}

		// Lumière
		if (activateLight)
		{
			if (timer == 0)
				timerCD[1] = 0;

			timer += Time.deltaTime;

			for (int i = 0 ; i < lightsRoom.Count ; i++)
			{
				if (lightsRoom[i]._baseIntensity >= 0.1f)
				{
					lightsRoom[i]._baseIntensity -= Time.deltaTime * 0.3f;
				}
			}

			if (timer >= 3f)
			{
				activateLight = false;
				timer = 0;
				lightsRoom.Clear();
			}
		}

		// Timer CD
		if (timerCD[0] <= 10)
		{
			timerCD[0] += Time.deltaTime;
			float value = timerCD[0] / 10;
			imagesCD[0].fillAmount = value;
		}
		if (timerCD[1] <= 40)
		{
			timerCD[1] += Time.deltaTime;
			float value = timerCD[1] / 40;
			imagesCD[1].fillAmount = value;
		}
		if (timerCD[2] <= 120)
		{
			timerCD[2] += Time.deltaTime;
			float value = timerCD[2] / 120;
			imagesCD[2].fillAmount = value;
		}
		if (timerCD[3] <= 20)
		{
			timerCD[3] += Time.deltaTime;
			float value = timerCD[3] / 20;
			imagesCD[3].fillAmount = value;
		}
	}

	private void Possession()
	{
		if (ghost.possession != null)
		{
			ghost.possession.GetComponent<PossessionTrigger>().planeObject.SetActive(false);
			// Si son
			if (ghost.possession.GetComponent<AudioSource>())
			{
				ghost.possession.GetComponent<AudioSource>().Play();
			}
			// Si animator
			if (ghost.possession.GetComponent<Animator>())
			{
				ghost.possession.GetComponent<Animator>().SetBool("Anim", true);
				timerCD[0] = 0;
				ghost.possession.GetComponent<BoxCollider>().enabled = false;
				ghost.possession = null;
			}
		}
	}

	private void LightDown()
	{
		int roomNbr = 0;
		string s1 = ghost.mansionRoom.name;
		string s2 = s1.Substring(s1.Length - 1);
		roomNbr = int.Parse(s2);

		RoomNumber(roomNbr);
	}

	private void RoomNumber(int roomNbr)
	{
		if (roomNbr != 0)
		{
			for (int i = (roomNbr * 3) - 3 ; i <= (roomNbr * 3) - 1 ; i++)
			{
				lightsRoom.Add(lights.lights[i]);
			}

			activateLight = true;
		}
	}

	private void Teleport()
	{
		ghost.transform.localPosition = new Vector3(0, ghost.transform.localPosition.y, 0);
		timerCD[2] = 0;
	}

	private void Taunt()
	{
		ghost.GetComponent<AudioSource>().Play();
		timerCD[3] = 0;
	}

}