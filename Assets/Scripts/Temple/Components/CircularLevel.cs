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

	private List<CircularSegment> segmentObjects = new List<CircularSegment>();

	private Outline outline;
	private SingleTouchRotationGesture gesture;
	private Transformer transformer;

	public CircularLevel Clone {
		get {
			return clone;
		}
	}

	private CircularLevel clone;

	private void Awake() {
		// Has to be in Awake since it is used in OnEnable
		gesture = GetComponent<SingleTouchRotationGesture>();
	}

	private void Start() {
		foreach (Transform child in transform) {
			var segment = child.GetComponent<CircularSegment>();
			if (segment) {
				segmentObjects.Add(segment);
			}
		}

		outline = GetComponent<Outline>();
		transformer = GetComponent<Transformer>();
	}

	private void Destroy() {
		segmentObjects.Clear();
	}

	private void OnEnable() {
		gesture.TransformStarted += transformStartedHandler;
		gesture.TransformCompleted += transformCompletedHandler;
	}

	private void OnDisable() {
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
		clone.setAreObstaclesActive(true);
		clone.transform.DOShakeRotation(DURATION_SHAKE, 4.0f * Vector3.up);

		this.setRenderOutlineOnly(true);
		this.setAreObstaclesActive(false);
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
		foreach (var segment in segmentObjects) {
			segment.renderOutlineOnly(renderOutlineOnly);
		}
	}

	private void setAreObstaclesActive(bool shouldBeActive) {
		foreach (var segment in segmentObjects) {
			segment.setEnableObstacle(shouldBeActive);
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

	public CircularSegment GetSegmentByIndex(int index) {
		return segmentObjects.Find((CircularSegment segment) => {
			return segment.SegmentIndex == index;
		});
	}
}