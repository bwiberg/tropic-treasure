using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleAgent : MonoBehaviour {
	private NavMeshAgent agent;

	public Transform target;

	// Use this for initialization
	void Awake () {
		agent = GetComponent<NavMeshAgent>();
		agent.SetDestination(target.position);
	}
	
	// Update is called once per frame
	void Update () {
	}
}
