using System;
using System.Collections.Generic;
using cakeslice;
using TouchScript.Behaviors;
using TouchScript.Gestures;
using UnityEngine;
using UnityEngine.AI;

using DG.Tweening;

[RequireComponent(typeof(Transformer),
    typeof(SingleTouchRotationGesture))]
public class CircularLevel : MonoBehaviour {
    
	public CircularTemple ParentTemple {
		get {
			return transform.parent.GetComponent<CircularTemple>();
		}
	}

	public List<Arc> Segments = new List<Arc>();
	public float InnerRadius;
	public float Thickness;
	public float Height;

	public Material outlineOnlyMaterial;
	public Material originalMaterial;

	private Outline outline;
	private SingleTouchRotationGesture gesture;
	private Transformer transformer;

	private CircularLevel clone;

	void Awake() {
		outline = GetComponent<Outline>();
		gesture = GetComponent<SingleTouchRotationGesture>();
		transformer = GetComponent<Transformer>();
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
		clone.setIsObstacleActive(true);
		clone.transform.DOShakeRotation(DURATION_SHAKE, 4.0f * Vector3.up);

		this.setRenderOutlineOnly(true);
		this.setIsObstacleActive(false);
    }

    private void transformCompletedHandler(object sender, EventArgs e) {
		Quaternion target = transform.rotation;
		this.setRenderOutlineOnly(false);
		gameObject.SetActive(false);

		clone.gesture.PreviousAngle = transform.eulerAngles.y;

		clone.animateRotation(target);
    }

	private void createClone() {
		clone = GameObject.Instantiate(gameObject).GetComponent<CircularLevel>();

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

	private void setIsObstacleActive(bool shouldBeActive) {
		foreach (var obstacle in GetComponentsInChildren<NavMeshObstacle>()) {
			obstacle.enabled = shouldBeActive;
		}
	}

	private static float DURATION_SHAKE = 0.4f;
	private static float DURATION_ROTATE_FACTOR = 1.0f;

	private void animateRotation(Quaternion target) {
		float duration = InnerRadius * ParentTemple.WallRotationDurationFactor;
		var animation = DOTween.Sequence();
		animation
			.Append(transform.DORotateQuaternion(target, duration).SetEase(Ease.Linear))
			.Append(transform.DOShakeRotation(DURATION_SHAKE, 2.0f * Vector3.up))
			.OnComplete(() => {
				// Enable further rotations when complete
				gesture.enabled = true;
			})
			.Play();

		// Disable further rotations until complete
		gesture.enabled = false;
	}
}