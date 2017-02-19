using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempleTarget : MonoBehaviour {
	void OnTriggerEnter(Collider other) {
		GameObject.Destroy(other.gameObject);
	}
}
