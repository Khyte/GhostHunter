using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableProjector : MonoBehaviour {

	public Light[] projector;


	void OnPreCull()
	{
		for (int i = 0 ; i < 4 ; i++)
		{
			if (projector[i] != null)
				projector[i].enabled = false;
		}
	}

	void OnPreRender()
	{
		for (int i = 0 ; i < 4 ; i++)
		{
			if (projector[i] != null)
				projector[i].enabled = false;
		}
	}
	void OnPostRender()
	{
		for (int i = 0 ; i < 4 ; i++)
		{
			if (projector[i] != null)
				projector[i].enabled = true;
		}
	}

}