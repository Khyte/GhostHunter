using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Magic : MonoBehaviour {

	public GameObject leftController;
	public VRTK_BezierPointerRenderer pointer;

	public GameObject projector;
	public GameObject[] runeProjectors;
	public GameObject[] runes;

	public float coolDown;

	private OrbColor orb;
	private SwipeDetector swipeDetector;
	private Color[] colors = new Color[4];
	private Color lastColor;

	private int lastMagicValue = 1;
	private bool activeProjector = false;
	private bool castMagic = false;
	private bool magicIsCasted = false;

	private float timer;
	private float colorTimer;


	void Start () {
		// VRTK
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

	private void DoTouchpadPressed(object sender, ControllerInteractionEventArgs e)
	{
		Debug.Log("PRESSED");
		if (!magicIsCasted)
		{
			Debug.Log("MAGIC");
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
			GameObject rune = Instantiate(runes[lastMagicValue - 1], new Vector3(runePos.x, 0.04f, runePos.z), runes[lastMagicValue - 1].transform.rotation);
			Destroy(rune, coolDown);
			magicIsCasted = true;
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
		if (activeProjector)
		{
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
			if (pointer.actualCursor)
			{
				runeProjectors[lastMagicValue - 1].transform.localPosition = pointer.actualCursor.transform.position;
			}
		}

		// Magie lancée
		if (magicIsCasted)
		{
			if (timer == 0)
			{
				lastColor = orb.orbColor;
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