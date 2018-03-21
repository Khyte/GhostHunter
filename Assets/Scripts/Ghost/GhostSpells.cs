using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSpells : MonoBehaviour {

	public GameController gameC;
	public RuneDrawing runeD1;
	public RuneDrawing runeD2;
	public GhostMagic ghostMagic;

	public int spellNbr = 0;

	private RuneDrawing runeD;
	private int lastSpell = 0;
	private int lastRune = 99;


	private void Update()
	{
		if (gameC.startGame)
		{
			runeD = runeD2;
		}
		else
		{
			runeD = runeD1;
		}
	}

	public void Possession()
	{
		if (ghostMagic.imagesCD[0].fillAmount == 1)
		{
			if (!runeD.drawRune)
			{
				runeD.line.enabled = false;
				runeD.randomRune = Random.Range(0, runeD.rune.Length);
				if (lastRune == runeD.randomRune)
				{
					Possession();
					return;
				}
				lastRune = runeD.randomRune;
				runeD.CreateRune();
				spellNbr = 1;
				lastSpell = 1;
			}
			else if (runeD.drawRune && spellNbr != 0 && lastSpell != 1)
			{
				runeD.rune[runeD.randomRune].gameObject.SetActive(false);
				runeD.animTuto[runeD.randomRune].gameObject.SetActive(false);
				runeD.line.enabled = false;
				runeD.randomRune = Random.Range(0, runeD.rune.Length);
				if (lastRune == runeD.randomRune)
				{
					Possession();
					return;
				}
				lastRune = runeD.randomRune;
				runeD.CreateRune();
				spellNbr = 1;
				lastSpell = 1;
			}
			else
			{
				runeD.drawRune = false;
				runeD.rune[runeD.randomRune].gameObject.SetActive(false);
				runeD.animTuto[runeD.randomRune].gameObject.SetActive(false);
				runeD.line.enabled = false;
				spellNbr = 0;
				lastSpell = 0;
			}
		}
	}

	public void LightDown()
	{
		if (ghostMagic.imagesCD[1].fillAmount == 1)
		{
			if (!runeD.drawRune)
			{
				runeD.line.enabled = false;
				runeD.randomRune = Random.Range(0, runeD.rune.Length);
				if (lastRune == runeD.randomRune)
				{
					LightDown();
					return;
				}
				lastRune = runeD.randomRune;
				runeD.CreateRune();
				spellNbr = 2;
				lastSpell = 2;
			}
			else if (runeD.drawRune && spellNbr != 0 && lastSpell != 2)
			{
				runeD.rune[runeD.randomRune].gameObject.SetActive(false);
				runeD.animTuto[runeD.randomRune].gameObject.SetActive(false);
				runeD.line.enabled = false;
				runeD.randomRune = Random.Range(0, runeD.rune.Length);
				if (lastRune == runeD.randomRune)
				{
					LightDown();
					return;
				}
				lastRune = runeD.randomRune;
				runeD.CreateRune();
				spellNbr = 2;
				lastSpell = 2;
			}
			else
			{
				runeD.drawRune = false;
				runeD.rune[runeD.randomRune].gameObject.SetActive(false);
				runeD.animTuto[runeD.randomRune].gameObject.SetActive(false);
				runeD.line.enabled = false;
				spellNbr = 0;
				lastSpell = 0;
			}
		}
	}

	public void Teleport()
	{
		if (ghostMagic.imagesCD[2].fillAmount == 1)
		{
			if (!runeD.drawRune)
			{
				runeD.line.enabled = false;
				runeD.randomRune = Random.Range(0, runeD.rune.Length);
				if (lastRune == runeD.randomRune)
				{
					Teleport();
					return;
				}
				lastRune = runeD.randomRune;
				runeD.CreateRune();
				spellNbr = 3;
				lastSpell = 3;
			}
			else if (runeD.drawRune && spellNbr != 0 && lastSpell != 3)
			{
				runeD.rune[runeD.randomRune].gameObject.SetActive(false);
				runeD.animTuto[runeD.randomRune].gameObject.SetActive(false);
				runeD.line.enabled = false;
				runeD.randomRune = Random.Range(0, runeD.rune.Length);
				if (lastRune == runeD.randomRune)
				{
					Teleport();
					return;
				}
				lastRune = runeD.randomRune;
				runeD.CreateRune();
				spellNbr = 3;
				lastSpell = 3;
			}
			else
			{
				runeD.drawRune = false;
				runeD.rune[runeD.randomRune].gameObject.SetActive(false);
				runeD.animTuto[runeD.randomRune].gameObject.SetActive(false);
				runeD.line.enabled = false;
				spellNbr = 0;
				lastSpell = 0;
			}
		}
	}

	public void Taunt()
	{
		if (ghostMagic.imagesCD[3].fillAmount == 1)
		{
			if (!runeD.drawRune)
			{
				runeD.line.enabled = false;
				runeD.randomRune = Random.Range(0, runeD.rune.Length);
				if (lastRune == runeD.randomRune)
				{
					Taunt();
					return;
				}
				lastRune = runeD.randomRune;
				runeD.CreateRune();
				spellNbr = 4;
				lastSpell = 4;
			}
			else if (runeD.drawRune && spellNbr != 0 && lastSpell != 4)
			{
				runeD.rune[runeD.randomRune].gameObject.SetActive(false);
				runeD.animTuto[runeD.randomRune].gameObject.SetActive(false);
				runeD.line.enabled = false;
				runeD.randomRune = Random.Range(0, runeD.rune.Length);
				if (lastRune == runeD.randomRune)
				{
					Taunt();
					return;
				}
				lastRune = runeD.randomRune;
				runeD.CreateRune();
				spellNbr = 4;
				lastSpell = 4;
			}
			else
			{
				runeD.drawRune = false;
				runeD.rune[runeD.randomRune].gameObject.SetActive(false);
				runeD.animTuto[runeD.randomRune].gameObject.SetActive(false);
				runeD.line.enabled = false;
				spellNbr = 0;
				lastSpell = 0;
			}
		}
	}

}