using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Magic : MonoBehaviour {

	public GameObject leftController;
	public VRTK_BezierPointerRenderer pointer;

	public GameObject projector;
	public GameObject[] runeProjectors;

	private OrbColor orb;
	private SwipeDetector swipeDetector;
	private Color[] colors = new Color[4];

	private int lastMagicValue = 1;
	private bool activeProjector = false;


	void Start () {
		// VRTK
		pointer.GetComponent<VRTK_ControllerEvents>().TouchpadPressed += new ControllerInteractionEventHandler(DoTouchpadPressed);

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
		activeProjector = !activeProjector;
		if (activeProjector)
			projector.SetActive(true);
		else
			projector.SetActive(false);
	}

	void Update () {
		if (lastMagicValue != swipeDetector.magicValue)
		{
			runeProjectors[lastMagicValue - 1].SetActive(false);
			lastMagicValue = swipeDetector.magicValue;
			orb.orbColor = colors[lastMagicValue - 1];
			runeProjectors[lastMagicValue - 1].SetActive(true);
		}

		// Projecteur
		if (pointer.actualCursor)
		{
			runeProjectors[lastMagicValue - 1].transform.localPosition = pointer.actualCursor.transform.position;
		}
	}

}