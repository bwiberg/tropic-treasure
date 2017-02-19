using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempleTarget : MonoBehaviour {
	void OnTriggerEnter(Collider other) {
		if(other.gameObject.GetComponent<SimpleAgent>())
			GameObject.Destroy(other.gameObject);
	}
}
