using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffreFort : MonoBehaviour {

	private Transform parent;

	private int ingotNbr = 0;


	private void OnEnable()
	{
		Invoke("FlyingIngot", 0.5f);
		parent = transform.parent;
	}

	private void FlyingIngot()
	{
		foreach(Transform child in parent)
		{
			ingotNbr++;
		}
		StartCoroutine(FlyingIngotTimer());
	}

	IEnumerator FlyingIngotTimer()
	{
		if (parent.GetChild(ingotNbr - 1).GetComponent<AudioSource>())
		{
			parent.GetChild(ingotNbr - 1).GetComponent<AudioSource>().Play();
		}
		yield return new WaitForSeconds(Random.Range(0.01f, 0.2f));
		parent.GetChild(ingotNbr - 1).GetComponent<Rigidbody>().useGravity = true;
		parent.GetChild(ingotNbr - 1).GetComponent<Rigidbody>().isKinematic = false;
		parent.GetChild(ingotNbr - 1).transform.localEulerAngles = new Vector3(0, -90, Random.Range(-200f, -140f));
		parent.GetChild(ingotNbr - 1).GetComponent<Rigidbody>().AddForce(parent.GetChild(ingotNbr - 1).transform.right * Random.Range(8f, 13f), ForceMode.Impulse);

		ingotNbr--;
		if (ingotNbr > 0)
		{
			StartCoroutine(FlyingIngotTimer());
		}
	}

}