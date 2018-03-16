using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovingGhost : MonoBehaviour {

	public Waypoints waypoint;

	private NavMeshAgent agent;
	private int pointNbr = 0;


	void Start () {
		agent = GetComponent<NavMeshAgent>();
	}
	
	void Update () {
		if (!waypoint.goToDest)
		{
			agent.speed = 0;
			pointNbr = 0;
		}
		if (waypoint.goToDest)
		{
			agent.speed = 2;
			Vector3 dest = waypoint.waypoints[pointNbr].transform.localPosition;
			agent.destination = new Vector3(dest.x, 0, dest.z);
		}
		if (waypoint.goToDest && agent.remainingDistance <= 0.2f)
		{
			GoToNextPoint();
		}
	}

	void GoToNextPoint()
	{
		if (pointNbr >= waypoint.waypoints.Count - 1 && waypoint.goToDest)
		{
			agent.speed = 0;
			pointNbr = 0;
			waypoint.goToDest = false;
			return;
		}
		else
			pointNbr++;
	}

}