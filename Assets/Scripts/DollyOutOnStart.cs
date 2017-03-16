using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DollyOutOnStart : MonoBehaviour {
	public float delta;
	public float waitDuration;
	public float dollyDuration;

	void Awake() {
		
	}

	void Start () {
		Vector3 endPosition = transform.position;
		Vector3 startPosition = endPosition + transform.forward * delta;
		transform.position = startPosition;

		DOTween.Sequence()
			.AppendInterval(waitDuration)
			.Append(transform.DOMove(endPosition, dollyDuration).SetEase(Ease.InOutQuad))
			.Play();
	}
}
