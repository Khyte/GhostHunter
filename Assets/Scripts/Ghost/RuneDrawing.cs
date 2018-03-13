using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneDrawing : MonoBehaviour {

	public Transform rune;
	public List<GameObject> waypoints = new List<GameObject>();

	public int waypointHit = 1;

	private List<GameObject> listWaypoints = new List<GameObject>();
	private LineRenderer line;


	void Start () {
		line = GetComponent<LineRenderer>();
	}

	private void OnEnable()
	{
		listWaypoints.Clear();
		waypointHit = 1;
		foreach (Transform child in rune)
		{
			listWaypoints.Add(child.gameObject);
		}
	}

	void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			waypoints.Clear();
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
						go.transform.localPosition = new Vector3(hit.point.x, 30, hit.point.z);
						waypoints.Add(go);
						if (hit.collider.name == "Waypoint" + waypointHit)
						{
							waypointHit++;
							hit.collider.tag = "Untagged";
						}
					}
					else
					{
						Vector3 offset = waypoints[waypoints.Count - 1].transform.localPosition - new Vector3(hit.point.x, 30, hit.point.z);
						float sqrLen = offset.sqrMagnitude;

						if (sqrLen >= 0.3f)
						{
							GameObject go = new GameObject();
							go.transform.parent = transform;
							go.transform.localPosition = new Vector3(hit.point.x, 30, hit.point.z);
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
					Vector3 offset = waypoints[waypoints.Count - 1].transform.localPosition - new Vector3(hit.point.x, 30, hit.point.z);
					float sqrLen = offset.sqrMagnitude;

					if (sqrLen >= 0.3f)
					{
						GameObject go = new GameObject();
						go.transform.parent = transform;
						go.transform.localPosition = new Vector3(hit.point.x, 30, hit.point.z);
						waypoints.Add(go);
					}
				}
			}
		}

		if (Input.GetMouseButtonUp(0) && waypoints.Count > 0)
		{
			// Release = end waypoints
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
			}
			// Bon dessin
			else
			{
				for (int i = 0 ; i < listWaypoints.Count ; i++)
				{
					listWaypoints[i].tag = "Waypoints";
				}
				waypointHit = 1;
				gameObject.SetActive(false);
			}
		}
	}

}