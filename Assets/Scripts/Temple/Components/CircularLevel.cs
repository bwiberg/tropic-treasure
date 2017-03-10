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

	public float InnerRadius;
	public float Thickness;
	public float Height;

	public CircularSegment[] Segments;
	private List<CircularSegment> segmentObjects = new List<CircularSegment>();

	private Outline outline;
	private SingleTouchRotationGesture gesture;
	private Transformer transformer;

	private Sequence rotationSequence;

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
		outline = GetComponent<Outline>();
		transformer = GetComponent<Transformer>();
		Segments = GetComponentsInChildren<CircularSegment>();
	}

	private void OnDestroy() {
		Debug.Log("OnDestroy");
	}

	private void OnEnable() {
		gesture.TransformStarted += transformStartedHandler;
		gesture.TransformCompleted += transformCompletedHandler;

		if (rotationSequence != null && !rotationSequence.IsComplete()) {
			rotationSequence.Play();
		}
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
		clone = GameObject.Instantiate(gameObject, transform.parent).GetComponent<CircularLevel>();

		// Assign this GameObject as the clone's clone (phew!)
		clone.clone = this;
		clone.gameObject.SetActive(false);
	}

	private void setRenderOutlineOnly(bool renderOutlineOnly) {
		foreach (var segment in Segments) {
			segment.renderOutlineOnly(renderOutlineOnly);
		}
	}

	private void setAreObstaclesActive(bool shouldBeActive) {
		foreach (var segment in Segments) {
			segment.setEnableObstacle(shouldBeActive);
		}
	}

	private static float DURATION_SHAKE = 0.4f;
	private static float DURATION_ROTATE_FACTOR = 1.0f;

	private void animateRotation(Quaternion target) {
		float duration = InnerRadius * ParentTemple.WallRotationDurationFactor;
		rotationSequence = DOTween.Sequence();
		rotationSequence
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

	public void SetRotationsEnabled(bool enabled) {
		this.internalSetRotationsEnabled(enabled);
		if (clone != null) {
			clone.internalSetRotationsEnabled(enabled);
		}
	}

	private void internalSetRotationsEnabled(bool enable) {
		gesture.enabled = enable;

		if (enable && rotationSequence != null && !rotationSequence.IsComplete()) {
			rotationSequence.Play();
		}

		else if (!enable && rotationSequence != null && !rotationSequence.IsComplete()) {
			rotationSequence.Pause();
		}
	}

	public CircularSegment GetSegmentByIndex(int index) {
		for (int i = 0; i < Segments.Length; ++i) {
			if (i == Segments[i].SegmentIndex) {
				return Segments[i];
			}
		}
		throw new IndexOutOfRangeException("GetSegmentByIndex did not find the segment");
	}
}