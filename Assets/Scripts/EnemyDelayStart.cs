using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDelayStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke ("moveDelay",2.0f);	
	}

	void moveDelay(){
		gameObject.GetComponent<SimpleAgent> ().enabled = true;
	}
}
