using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneDrawing : MonoBehaviour {

	public Transform[] rune;
	public List<GameObject> waypoints = new List<GameObject>();

	public GameObject[] animTuto;

	public int randomRune = 0;
	public int waypointHit = 1;

	public List<GameObject> listWaypoints = new List<GameObject>();
	public LineRenderer line;

	public bool drawRune = false;

	public bool runeDone = false;


	void Start () {
		line = GetComponent<LineRenderer>();
	}

	public void CreateRune()
	{
		rune[randomRune].gameObject.SetActive(true);
		animTuto[randomRune].gameObject.SetActive(true);
		listWaypoints.Clear();
		waypointHit = 1;
		foreach (Transform child in rune[randomRune])
		{
			listWaypoints.Add(child.gameObject);
		}
		drawRune = true;
	}

	void Update () {
		if (drawRune)
		{
			if (Input.GetMouseButtonDown(0))
			{
				line.positionCount = 0;
				for (int i = 0 ; i < waypoints.Count ; i++)
				{
					Destroy(waypoints[i]);
				}
				waypoints.Clear();
				animTuto[randomRune].gameObject.SetActive(false);
			}
			if (Input.GetMouseButton(0) && waypoints.Count <= 100)
			{
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, out hit))
				{
					if (hit.collider.tag == "Waypoints")
					{
						if (waypoints.Count == 0)
						{
							GameObject go = new GameObject();
							go.transform.parent = transform;
							go.transform.localPosition = new Vector3(hit.point.x, 40, hit.point.z);
							waypoints.Add(go);
							if (hit.collider.name == "Waypoint" + waypointHit)
							{
								waypointHit++;
								hit.collider.tag = "Untagged";
							}
						}
						else
						{
							Vector3 offset = waypoints[waypoints.Count - 1].transform.localPosition - new Vector3(hit.point.x, 40, hit.point.z);
							float sqrLen = offset.sqrMagnitude;

							if (sqrLen >= 0.4f)
							{
								GameObject go = new GameObject();
								go.transform.parent = transform;
								go.transform.localPosition = new Vector3(hit.point.x, 40, hit.point.z);
								waypoints.Add(go);
							}
							if (hit.collider.name == "Waypoint" + waypointHit)
							{
								waypointHit++;
								hit.collider.tag = "Untagged";
							}
						}
					}
					if (waypoints.Count > 0)
					{
						Vector3 offset = waypoints[waypoints.Count - 1].transform.localPosition - new Vector3(hit.point.x, 40, hit.point.z);
						float sqrLen = offset.sqrMagnitude;

						if (sqrLen >= 0.3f)
						{
							GameObject go = new GameObject();
							go.transform.parent = transform;
							go.transform.localPosition = new Vector3(hit.point.x, 40, hit.point.z);
							waypoints.Add(go);
						}
					}
				}
			}

			if (Input.GetMouseButtonUp(0) && waypoints.Count > 0)
			{
				// Release = end waypoints
				line.enabled = true;
				line.positionCount = waypoints.Count;
				for (int i = 0 ; i < waypoints.Count ; i++)
				{
					line.SetPosition(i, waypoints[i].transform.localPosition);
				}

				// Erreur
				if (waypointHit < 10)
				{
					for (int i = 0 ; i < listWaypoints.Count ; i++)
					{
						listWaypoints[i].tag = "Waypoints";
					}
					waypointHit = 1;
					animTuto[randomRune].gameObject.SetActive(true);
				}
				// Bon dessin
				else
				{
					for (int i = 0 ; i < listWaypoints.Count ; i++)
					{
						listWaypoints[i].tag = "Waypoints";
					}
					waypoints.Clear();
					line.enabled = false;
					waypointHit = 1;
					rune[randomRune].transform.gameObject.SetActive(false);
					animTuto[randomRune].gameObject.SetActive(false);
					drawRune = false;
					runeDone = true;
				}
			}
		}
	}

}