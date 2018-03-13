using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour {

	public GameObject ghost;
	public List<GameObject> waypoints = new List<GameObject>();

	public bool goToDest = false;

	private LineRenderer line;


	private void Start()
	{
		line = GetComponent<LineRenderer>();
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			waypoints.Clear();
			goToDest = false;
		}
		if (Input.GetMouseButton(0) && waypoints.Count <= 100)
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit))
			{
				if (hit.collider.tag == "Ghost")
				{
					if (waypoints.Count == 0)
					{
						GameObject go = new GameObject();
						go.transform.parent = transform;
						go.transform.localPosition = new Vector3(hit.point.x, 20, hit.point.z);
						waypoints.Add(go);
					}
				}
				if (waypoints.Count > 0)
				{
					Vector3 offset = waypoints[waypoints.Count - 1].transform.localPosition - new Vector3(hit.point.x, 20, hit.point.z);
					float sqrLen = offset.sqrMagnitude;

					if (sqrLen >= 0.3f)
					{
						GameObject go = new GameObject();
						go.transform.parent = transform;
						go.transform.localPosition = new Vector3(hit.point.x, 20, hit.point.z);
						waypoints.Add(go);
					}
				}
			}
		}

		if (Input.GetMouseButtonUp(0) && waypoints.Count > 0)
		{
			// Release = end waypoints
			line.positionCount = waypoints.Count;
			line.SetPosition(0, new Vector3(ghost.transform.localPosition.x, 20, ghost.transform.localPosition.z));
			for (int i = 1 ; i < waypoints.Count ; i++)
			{
				line.SetPosition(i, waypoints[i].transform.localPosition);
			}
			goToDest = true;
		}
	}

}