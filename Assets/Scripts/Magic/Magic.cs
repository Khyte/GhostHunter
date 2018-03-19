using System.Collections;
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
	public GameObject actualRune;

	// Pages
	public Material pageMat;
	public Texture page1;
	public Texture page2;
	public Texture page3;
	public Texture page4;

	public float coolDown;

	// Couleur orbe
	private OrbColor orb;
	private SwipeDetector swipeDetector;
	private Color[] colors = new Color[4];
	private Color lastColor;

	private int lastMagicValue = 1;
	public bool activeProjector = false;
	private bool castMagic = false;
	private bool changingSpell = false;
	public bool magicIsCasted = false;
	public bool bookOpen = false;

	// Timer
	private float timer;
	private float colorTimer;

	private Vector3 currentAngle;


	void Start () {
		// VRTK
		leftController.GetComponent<VRTK_ControllerEvents>().TriggerClicked += new ControllerInteractionEventHandler(DoTriggerClickedLeft);
		pointer.GetComponent<VRTK_ControllerEvents>().TouchpadPressed += new ControllerInteractionEventHandler(DoTouchpadPressed);
		pointer.GetComponent<VRTK_ControllerEvents>().TriggerClicked += new ControllerInteractionEventHandler(DoTriggerClickedRight);

		swipeDetector = leftController.GetComponent<SwipeDetector>();
		orb = GetComponent<OrbColor>();

		// Couleurs selon la magie
		colors[0] = new Color(0f, 0.2f, 1f);
		colors[1] = new Color(0.8f, 0f, 0f);
		colors[2] = new Color(0f, 1f, 0f);
		colors[3] = new Color(1f, 1f, 0f);

		orb.orbColor = colors[0];

		pageMat.SetTexture("_MainTex", page1);
	}

	private void DoTriggerClickedLeft(object sender, ControllerInteractionEventArgs e)
	{
		if (bookOpen)
		{
			bookOpen = false;
			bookAnim["Book"].speed = -1;
			float actualTimeAnim = bookAnim["Book"].time;
			if (actualTimeAnim == 0)
			{
				actualTimeAnim = bookAnim["Book"].length;
			}
			bookAnim["Book"].time = actualTimeAnim;
			bookAnim.Play("Book");
		}
		else
		{
			bookOpen = true;
			bookAnim["Book"].speed = 1;
			float actualTimeAnim = bookAnim["Book"].time;
			bookAnim["Book"].time = actualTimeAnim;
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

	private void DoTriggerClickedRight(object sender, ControllerInteractionEventArgs e)
	{
		if (castMagic && !changingSpell)
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

			// Hunt rune
			if (lastMagicValue == 2)
			{
				actualRune.transform.GetChild(4).transform.localPosition = new Vector3(0, 0, -0.3f);
			}

			// Light rune
			if (lastMagicValue == 4)
			{
				lightRune.ActivateLights();
			}
		}
	}

	void Update () {
		// Pages du livre
		switch (swipeDetector.magicValue) {
			case 1:
				pageMat.SetTexture("_MainTex", page1);
				break;
			case 2:
				pageMat.SetTexture("_MainTex", page2);
				break;
			case 3:
				pageMat.SetTexture("_MainTex", page3);
				break;
			case 4:
				pageMat.SetTexture("_MainTex", page4);
				break;
		}

		// Ouverture livre
		if (bookOpen && bookAnim.transform.localEulerAngles != new Vector3(0, 90, 80))
		{
			currentAngle = new Vector3(
				Mathf.LerpAngle(bookAnim.transform.localEulerAngles.x, 0, Time.deltaTime),
				Mathf.LerpAngle(bookAnim.transform.localEulerAngles.y, 90, Time.deltaTime),
				Mathf.LerpAngle(bookAnim.transform.localEulerAngles.z, 80, Time.deltaTime));

			bookAnim.transform.localEulerAngles = currentAngle;
		}
		else if (!bookOpen && bookAnim.transform.localEulerAngles != new Vector3(80, 0, 0))
		{
			currentAngle = new Vector3(
				Mathf.LerpAngle(bookAnim.transform.localEulerAngles.x, 80, Time.deltaTime),
				Mathf.LerpAngle(bookAnim.transform.localEulerAngles.y, 0, Time.deltaTime),
				Mathf.LerpAngle(bookAnim.transform.localEulerAngles.z, 0, Time.deltaTime));

			bookAnim.transform.localEulerAngles = currentAngle;
		}

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
		if (lastMagicValue != swipeDetector.magicValue && !changingSpell && !magicIsCasted)
		{
			castMagic = false;
			timer = 0;
			colorTimer = 0;
			runeProjectors[lastMagicValue - 1].SetActive(false);
			lastMagicValue = swipeDetector.magicValue;
			runeProjectors[lastMagicValue - 1].SetActive(true);
			lastColor = orb.orbColor;
			timer += Time.deltaTime;
			changingSpell = true;
		}

		if (timer > 0f && changingSpell && !magicIsCasted)
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
				changingSpell = false;
			}
		}

		// Projecteur
		if (activeProjector && pointer.actualCursor)
		{
			runeProjectors[lastMagicValue - 1].transform.localPosition = pointer.actualCursor.transform.position;
		}

		// Magie lancée
		if (magicIsCasted && !changingSpell)
		{
			if (timer == 0)
			{
				lastColor = orb.orbColor;
				// Pas bien, à modifier pour amélioration
				actualRune.transform.GetChild(0).GetComponent<Light>().intensity = 1f;
			}

			timer += Time.deltaTime;

			// Hunt rune
			if (timer <= 2f && lastMagicValue == 2 && actualRune.transform.GetChild(4).transform.localPosition.z <= 0)
			{
				actualRune.transform.GetChild(4).transform.localPosition += new Vector3(0, 0, 0.004f);
			}

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
					// Hunt rune
					if (lastMagicValue == 2 && actualRune.transform.GetChild(4).transform.localPosition.z >= -0.5f)
					{
						actualRune.transform.GetChild(4).transform.localPosition -= new Vector3(0, 0, 0.005f);
					}
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