using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTrees : MonoBehaviour {


	public List<GameObject> trees;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < trees.Count; i++) 
			{
				float torque = MicInput.loudness * 10;
				var rb = trees[i].GetComponent<Rigidbody>();
				rb.AddTorque(new Vector3(torque, torque, torque));
			}
	}
}
