using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleAgent : MonoBehaviour {
	private NavMeshAgent agent;

	public Transform target;

	public static float RemainingDistanceThreshold = 0.5f;

	private NavMeshPathStatus status;

	private GameObject pathEndMarker;

	// Use this for initialization
	void OnEnable () {
		agent = GetComponent<NavMeshAgent>();
		agent.SetDestination(target.position);

		pathEndMarker = GameObject.Find("PathEndMarker");
	}

	void Update() {
		if (checkAndUpdatePathStatus()) {
			transitionToNewPathStatus();
		}

		switch (status) {
			case NavMeshPathStatus.PathComplete:
				UpdateWithCompletePath();
				break;
			case NavMeshPathStatus.PathPartial:
				UpdateWithPartialPath();
				break;
			case NavMeshPathStatus.PathInvalid:
				UpdateWithInvalidPath();
				break;
		}
	}

	private bool checkAndUpdatePathStatus() {
		var oldStatus = status;
		status = agent.pathStatus;

		return oldStatus != status;
	}

	private void transitionToNewPathStatus() {
#if UNITY_EDITOR
		Debug.LogFormat("New path status: {0}, Path end position: {1}", status, agent.pathEndPosition);
		pathEndMarker.transform.position = agent.pathEndPosition + new Vector3(0.0f, 2.0f, 0.0f);
#endif
	}

	private void UpdateWithCompletePath() {
		
	}

	private void UpdateWithPartialPath() {
		if (agent.remainingDistance < RemainingDistanceThreshold) {
			Ray ray = new Ray(transform.position, agent.pathEndPosition - transform.position);
			RaycastHit hit = new RaycastHit();
			Physics.Raycast(ray, out hit, 2 * RemainingDistanceThreshold, 0);
		}
	}

	private void UpdateWithInvalidPath() {

	}
}
