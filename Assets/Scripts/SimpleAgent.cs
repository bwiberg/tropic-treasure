using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleAgent : MonoBehaviour {
	private NavMeshAgent agent;

	public Transform target;

	public static float RemainingDistanceThreshold = 0.5f;

	// Use this for initialization
	void OnEnable () {
		agent = GetComponent<NavMeshAgent>();
		agent.SetDestination(target.position);
	}

	void Update() {
		if (GameManager.Instance.pirateShip.FireState == PirateShip.CannonFireState.HasNoTarget &&
			agent.pathStatus == NavMeshPathStatus.PathPartial && 
			agent.remainingDistance < RemainingDistanceThreshold) {
			findObstructionWallAndAlertCannon();
		}
	}

	void findObstructionWallAndAlertCannon() {
		Vector3 agentPosition = transform.position;

		GameObject closestSegment = null;
		float closestDistance = float.PositiveInfinity;

		var segments = GameObject.FindGameObjectsWithTag(Tags.CircularSegment);
		foreach (var segment in segments) {
			foreach (Transform obstacleTransform in segment.transform) {
				NavMeshObstacle obstacle = obstacleTransform.GetComponent<NavMeshObstacle>();

				Vector3 position = obstacleTransform.TransformPoint(obstacle.center);

				float distance = (position - agentPosition).magnitude;
				if (distance < closestDistance) {
					closestDistance = distance;
					closestSegment = segment;
				}
			}
		}

		// Hacky way to find middle of segment
		var allObstacles = closestSegment.GetComponentsInChildren<NavMeshObstacle>();
		var middleObstacle = allObstacles[Mathf.FloorToInt(allObstacles.Length / 2)];

		GameManager.Instance.pirateShip.FireCannonballAtSegment(
			closestSegment,
			middleObstacle.transform.TransformPoint(middleObstacle.center)
		);
	}
}
