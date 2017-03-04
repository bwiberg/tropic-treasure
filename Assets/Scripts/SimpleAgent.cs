﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleAgent : MonoBehaviour {
	private NavMeshAgent agent;

	public Transform target;

	// Use this for initialization
	void OnEnable () {
		agent = GetComponent<NavMeshAgent>();
		agent.SetDestination(target.position);
	}
}
