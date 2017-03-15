using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DollyOutOnStart : MonoBehaviour {
	public Vector3 delta;
	public float time;

	void Start () {
		Vector3 position = transform.position;
		position += delta;
		transform.position = position;

		transform.DOMove(-delta, time).SetEase(Ease.InOutCubic);
	}
}
