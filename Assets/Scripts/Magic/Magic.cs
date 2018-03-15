﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Magic : MonoBehaviour {

	// Controllers
	public GameObject steamVR;
	public GameObject leftController;
	public VRTK_BezierPointerRenderer pointer;
	public GameObject hunter;
	public Animation bookAnim;

	// Manoir (light rune)
	public LightRune lightRune;

	// Runes
	public GameObject projector;
	public GameObject[] runeProjectors;
	public GameObject[] runes;

	private GameObject actualRune;

	public float coolDown;

	// Couleur orbe
	private OrbColor orb;
	private SwipeDetector swipeDetector;
	private Color[] colors = new Color[4];
	private Color lastColor;

	private int lastMagicValue = 1;
	public bool activeProjector = false;
	private bool castMagic = false;
	public bool magicIsCasted = false;
	public bool bookOpen = false;

	// Timer
	private float timer;
	private float colorTimer;


	void Start () {
		// VRTK
		leftController.GetComponent<VRTK_ControllerEvents>().GripPressed += new ControllerInteractionEventHandler(DoGripPressed);
		pointer.GetComponent<VRTK_ControllerEvents>().TouchpadPressed += new ControllerInteractionEventHandler(DoTouchpadPressed);
		pointer.GetComponent<VRTK_ControllerEvents>().TriggerClicked += new ControllerInteractionEventHandler(DoTriggerClicked);

		swipeDetector = leftController.GetComponent<SwipeDetector>();
		orb = GetComponent<OrbColor>();

		// Couleurs selon la magie
		colors[0] = new Color(0f, 0.2f, 1f);
		colors[1] = new Color(0.8f, 0f, 0f);
		colors[2] = new Color(0f, 1f, 0f);
		colors[3] = new Color(1f, 1f, 0f);

		orb.orbColor = colors[0];
	}

	private void DoGripPressed(object sender, ControllerInteractionEventArgs e)
	{
		bookOpen = !bookOpen;
		if (bookOpen)
			bookAnim.Play();
		else
		{
			bookAnim["Book"].speed = -1;
			bookAnim["Book"].time = bookAnim["Book"].length;
			bookAnim.Play("Book");
		}
	}

	private void DoTouchpadPressed(object sender, ControllerInteractionEventArgs e)
	{
		if (!magicIsCasted)
		{
			activeProjector = !activeProjector;
			if (activeProjector)
			{
				projector.SetActive(true);
				castMagic = true;
			}
			else
			{
				projector.SetActive(false);
				castMagic = false;
			}
		}
	}

	private void DoTriggerClicked(object sender, ControllerInteractionEventArgs e)
	{
		if (castMagic)
		{
			castMagic = false;
			activeProjector = false;
			projector.SetActive(false);
			pointer.GetComponent<VRTK_Pointer>().currentActivationState = 0;
			timer = 0;
			colorTimer = 0;
			Vector3 runePos = runeProjectors[lastMagicValue - 1].transform.localPosition;
			actualRune = Instantiate(runes[lastMagicValue - 1], new Vector3(runePos.x, 0.04f, runePos.z), runes[lastMagicValue - 1].transform.rotation);
			Destroy(actualRune, coolDown);
			magicIsCasted = true;

			// Teleport rune
			if (lastMagicValue == 1)
			{
				// Distance entre le joueur et la rune
				float distance = Vector3.Distance(new Vector3(runePos.x, 0, runePos.z), new Vector3(hunter.transform.position.x, 0, hunter.transform.position.z));
				// Direction du joueur à la rune
				Vector3 normalize = (new Vector3(runePos.x, 0, runePos.z) - new Vector3(hunter.transform.position.x, 0, hunter.transform.position.z)).normalized;
				// Téléportation
				Vector3 oldPos = steamVR.transform.localPosition;
				steamVR.transform.localPosition = oldPos + (normalize * distance);
			}

			// Light rune
			if (lastMagicValue == 4)
			{
				lightRune.ActivateLights();
			}
		}
	}

	void Update () {
		// Cooldown selon le sort
		switch(lastMagicValue)
		{
			case 1:
				coolDown = 5f;
				break;
			case 2:
				coolDown = 11f;
				break;
			case 3:
				coolDown = 15f;
				break;
			case 4:
				coolDown = 5f;
				break;
		}

		// Projection rune
		if (lastMagicValue != swipeDetector.magicValue)
		{
			castMagic = false;
			timer = 0;
			colorTimer = 0;
			runeProjectors[lastMagicValue - 1].SetActive(false);
			lastMagicValue = swipeDetector.magicValue;
			runeProjectors[lastMagicValue - 1].SetActive(true);
			lastColor = orb.orbColor;
			timer += Time.deltaTime;
		}

		if (timer > 0f)
		{
			timer += Time.deltaTime;
			orb.orbColor = Color.Lerp(lastColor, colors[lastMagicValue - 1], colorTimer);
			if (colorTimer < 1)
			{
				colorTimer += Time.deltaTime / 2f;
			}
			if (timer >= 2f)
			{
				timer = 0;
				castMagic = true;
			}
		}

		// Projecteur
		if (activeProjector && pointer.actualCursor)
		{
			runeProjectors[lastMagicValue - 1].transform.localPosition = pointer.actualCursor.transform.position;
		}

		// Magie lancée
		if (magicIsCasted)
		{
			if (timer == 0)
			{
				lastColor = orb.orbColor;
				// Pas bien, à modifier pour amélioration
				actualRune.transform.GetChild(0).GetComponent<Light>().intensity = 1f;
			}

			timer += Time.deltaTime;

			if (timer > 2f && timer < coolDown - 2f)
			{
				colorTimer = 0;
			}
			else if (colorTimer < 1)
			{
				colorTimer += Time.deltaTime / 2f;
			}

			if (timer >= coolDown - 2f)
			{
				orb.orbColor = Color.Lerp(Color.white, lastColor, colorTimer);
				if (actualRune != null)
				{
					actualRune.transform.GetChild(0).GetComponent<Light>().intensity -= 0.01f;
				}
			}
			else if (timer <= 2f)
			{
				orb.orbColor = Color.Lerp(lastColor, Color.white, colorTimer);
			}

			if (timer >= coolDown)
			{
				timer = 0;
				colorTimer = 0;
				magicIsCasted = false;
			}
		}
	}

}