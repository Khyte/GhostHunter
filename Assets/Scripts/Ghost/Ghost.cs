using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ghost : MonoBehaviour {

	// Manoir et lumières
	public GameObject mansionRoom;
	public LightRune lightRune;

	// Possessions
	public GameObject possession;

	// Vie
	public Image life;

	private bool lightDmg = false;

	// Visibilité
	public Material ghostMat;
	public GameObject ghostEye;
	public ParticleSystem fireParticles;
	public bool invisible = true;
	public Image visibleIcon;
	private Color ghostMatColor;
	public Magic magic;

	private Animation ghostAnim;

	private int roomNbr = 0;
	private bool runeDmg = false;
	private bool runeMarked = false;

	private bool lastVisible;
	private float visibleTimer = 5f;
	private float t = 0;


	private void Start()
	{
		ghostAnim = GetComponent<Animation>();
		ghostMatColor = new Color(0, 0.6f, 1f, 1f);
		ghostMat.color = Color.black;
		ghostEye.SetActive(false);
		lastVisible = invisible;
		fireParticles.emissionRate = 0;
	}

	private void Update()
	{
		// Animation idle quand caché
		if (ghostMat.color == Color.black)
		{
			ghostAnim.Play("Idle");
		}

		// Visiblité du fantome
		if (!invisible)
			visibleIcon.color = Color.Lerp(new Color(1, 1, 1, 0.3f), new Color(1, 1, 1, 1f), Mathf.PingPong(Time.time, 1));
		else
			visibleIcon.color = new Color(1, 1, 1, 0.3f);
		if (lastVisible != invisible)
		{
			lastVisible = invisible;
			visibleTimer = 0;
			t = 0;
		}
		if (visibleTimer <= 3f && invisible)
		{
			visibleTimer += Time.deltaTime;
			t = visibleTimer / 2.5f;
			ghostEye.SetActive(false);
			ghostMat.color = Color.Lerp(ghostMatColor, Color.black, t);
			fireParticles.emissionRate = 0;
		}
		else if (visibleTimer <= 3f && !invisible)
		{
			visibleTimer += Time.deltaTime;
			t = visibleTimer / 2.5f;
			ghostEye.SetActive(true);
			ghostMat.color = Color.Lerp(Color.black, ghostMatColor, t);
			fireParticles.emissionRate = 15;
		}

		// Récupérer la luminosité de la salle
		if (mansionRoom != null)
		{
			if (roomNbr != 0)
			{
				if (lightRune.lights[(roomNbr * 3) - 1]._baseIntensity > 0.6f)
				{
					if (!ghostAnim.IsPlaying("Dmg"))
					{
						ghostAnim.Play("IdleToDmg");
					}
					ghostAnim.PlayQueued("Dmg");
					life.fillAmount -= 0.02f * Time.deltaTime;
					invisible = false;
					lightDmg = true;
				}
				else if (!runeDmg && lightRune.lights[(roomNbr * 3) - 1]._baseIntensity <= 0.6f && !runeMarked)
				{
					invisible = true;
					lightDmg = false;
					return;
				}
			}
			else if (!runeMarked && !runeDmg)
			{
				invisible = true;
				return;
			}
		}

		// Visibilité selon la rune posée
		/*if (magic.actualRune == null && !lightDmg && !runeDmg)
		{
			invisible = true;
			if (!ghostAnim.IsPlaying("Idle"))
			{
				ghostAnim["IdleToDmg"].speed = -1;
				ghostAnim["IdleToDmg"].time = ghostAnim["IdleToDmg"].length;
			}
			ghostAnim.PlayQueued("Idle");
		}*/
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "HuntRune")
		{
			runeDmg = true;
			invisible = false;
		}
		if (other.tag == "MarkRune")
		{
			invisible = false;
			runeMarked = true;
		}
		if (other.tag == "Room")
		{
			mansionRoom = other.gameObject;
			string s1 = mansionRoom.name;
			string s2 = s1.Substring(s1.Length - 1);
			roomNbr = int.Parse(s2);
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.tag == "HuntRune")
		{
			if (!ghostAnim.IsPlaying("Dmg"))
			{
				ghostAnim.Play("IdleToDmg");
			}
			ghostAnim.PlayQueued("Dmg");
			if (life.fillAmount > 0)
			{
				life.fillAmount -= 0.08f * Time.deltaTime;
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "HuntRune")
		{
			runeDmg = false;
			invisible = true;
		}
		if (other.tag == "MarkRune")
		{
			invisible = true;
			runeMarked = false;
		}
	}

}