using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannonball : MonoBehaviour {
	public PirateShip firingShip;

	private void Start() {
		
	}

	private void OnCollisionEnter(Collision col) {
		if (col.gameObject.CompareTag(Tags.CircularSegment)) {
			col.gameObject.GetComponent<CircularSegment>().handleCannonballHit();
			GameObject.Destroy(gameObject);

			firingShip.FireState = PirateShip.CannonFireState.TargetDestroyed;
		}
	}
}
