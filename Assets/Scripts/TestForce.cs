using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestForce : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			var rb = GetComponent<Rigidbody>();
			rb.AddTorque(new Vector3(250.0f, 0.0f, 0.0f));
		}
	}
}
