using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannonball : MonoBehaviour {
	private void OnCollisionEnter(Collision col) {
		if (col.gameObject.CompareTag(Tags.CircularSegment)) {
			col.gameObject.GetComponent<CircularSegment>().handleCannonballHit(this);
		}
	}
}
