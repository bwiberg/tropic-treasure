using System;
using cakeslice;
using TouchScript.Behaviors;
using TouchScript.Gestures;
using UnityEngine;

using DG.Tweening;

[RequireComponent(typeof(Transformer),
    typeof(SingleTouchRotationGesture))]
public class CircularTempleLevel : MonoBehaviour {
    
	public Material outlineOnlyMaterial;
	public Material originalMaterial;

	private Outline outline;
	private SingleTouchRotationGesture gesture;

	private CircularTempleLevel clone;

	void Awake() {
		outline = GetComponent<Outline>();
		gesture = GetComponent<SingleTouchRotationGesture>();
	}

	void OnEnable() {
		gesture.TransformStarted += transformStartedHandler;
		gesture.TransformCompleted += transformCompletedHandler;
	}

	void OnDisable() {
		gesture.TransformStarted -= transformStartedHandler;
		gesture.TransformCompleted -= transformCompletedHandler;
	}

    private void transformStartedHandler(object sender, EventArgs e) {
		if (!clone) {
			createClone();
		}

		clone.transform.rotation = transform.rotation;
		clone.gameObject.SetActive(true);
		clone.setRenderOutlineOnly(false);
		clone.transform.DOShakeRotation(DURATION_SHAKE, 4.0f * Vector3.up);
		this.setRenderOutlineOnly(true);
    }

    private void transformCompletedHandler(object sender, EventArgs e) {
		Quaternion target = transform.rotation;
		this.setRenderOutlineOnly(false);
		gameObject.SetActive(false);

		clone.gesture.PreviousAngle = transform.eulerAngles.y;

		clone.animateRotation(target);
    }

	private void createClone() {
		clone = GameObject.Instantiate(gameObject).GetComponent<CircularTempleLevel>();

		// Assign this GameObject as the clone's clone (phew!)
		clone.clone = this;
		clone.transform.SetParent(transform.parent);
		clone.gameObject.SetActive(false);
	}

	private void setRenderOutlineOnly(bool renderOutlineOnly) {
		foreach (Transform child in transform) {
			child.GetComponent<Outline>().enabled = renderOutlineOnly;

			if (renderOutlineOnly) {
				child.GetComponent<MeshRenderer>().material = outlineOnlyMaterial;
			} else {
				child.GetComponent<MeshRenderer>().material = originalMaterial;
			}
		}
	}

	private const float DURATION_SHAKE = 0.4f;
	private const float DURATION_ROTATE = 1.0f;

	private void animateRotation(Quaternion target) {
		var animation = DOTween.Sequence();

		animation
			//.Append(transform.DOShakeRotation(DURATION_SHAKE, 2.0f * Vector3.up))
			.Append(transform.DORotateQuaternion(target, DURATION_ROTATE).SetEase(Ease.Linear))
			.Append(transform.DOShakeRotation(DURATION_SHAKE, 2.0f * Vector3.up))
			.Play();
	}
}