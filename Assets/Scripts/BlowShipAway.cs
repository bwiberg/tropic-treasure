using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BlowShipAway : MonoBehaviour {

	private float duration = 10.0f;
	public float currentRotation{ get; set; }
	private float maxRotation = 1.0f;

	private Vector3 startPosition;
	private Vector3 startRotation;
	private Vector3 endPosition;
	private Vector3 endRotation;
	private Vector3 middlePosition;

	private Vector3[] waypointsThere;
	private Vector3[] waypointsBack;

	public bool shipIsGone{ get; set; }

	// Use this for initialization
	void Start () {
		shipIsGone = false;
		currentRotation = 0.0f;

		startPosition = transform.localPosition;
		startRotation = transform.localRotation.eulerAngles;

		middlePosition = new Vector3(8.085118f, 0.34f, 42.54898f);

		endPosition = new Vector3(-9.3f, 0.34f, 41.0f);
		endRotation = new Vector3(0.0f, 359.0f, 0.0f);

		waypointsThere = new Vector3[2];
		waypointsThere[1] = startPosition;
		waypointsThere[0] = endPosition;

		waypointsBack = new Vector3[2];
		waypointsBack[0] = waypointsThere[1];
		waypointsBack[1] = waypointsThere[0];
	}

	void RotateShip(float rotation) {
		var rot = transform.localRotation;
		rot.eulerAngles = startRotation + (endRotation - startRotation) * currentRotation / maxRotation;
		transform.localRotation = rot;
	}

	public void MoveShipAway (float rotation) {
		if(!shipIsGone) {
			if(currentRotation >= maxRotation) {
				currentRotation = maxRotation;
			}
			else {
				currentRotation = rotation;
			}
			RotateShip(currentRotation);

			if(currentRotation == maxRotation) {
				Tween t = transform.DOLocalMove(endPosition, duration);
				t.SetEase(Ease.Linear);
				shipIsGone = true;
			}
		}
	}

	public void MoveShipBack () {
		if(shipIsGone) {
			currentRotation = 0;
			RotateShip(currentRotation);
			transform.position = middlePosition;
			Tween t = transform.DOLocalMove(startPosition, duration * 1.5f);
			t.SetEase(Ease.Linear);
			shipIsGone = false;
		}
	}
}
