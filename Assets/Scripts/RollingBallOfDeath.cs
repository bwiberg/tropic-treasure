using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using cakeslice;
using TouchScript;
using TouchScript.Behaviors;
using TouchScript.Gestures;

public class RollingBallOfDeath : MonoBehaviour {
	public enum BallState {
		Disabled,
		AbilityActivated,
		Placing_ValidPosition,
		Placing_InvalidPosition,
		Rolling_HasTarget,
		Rolling_NoTarget,
		Finished_ParticlesAlive
	}

	public float SteerMultiplier = 5.0f;
	public float VelocityDampeningPerSecond = 0.5f;

	public Material InvisibleMaterial;
	public Material BallMaterial;

	public RollingBallPowerUp controller;

	private ParticleSystem particles;
	private ParticleSystemRenderer particleRenderer;
	public int DestroyedParticleCount = 25;
	private float emitTime;

	public float ValidPlacementRadius = 10.0f;

	public BallState State {
		get { return state; }
	}
	private BallState state = BallState.Disabled;

	public TouchPoint placingTouchPoint;

	private MeshRenderer meshRenderer;
	private Rigidbody rb;
	private SphereCollider sphereCollider;
	private BoxCollider boxCollider;
	private Outline outline;
	private TransformGesture gesture;

	private float rollStartTime;

	private Vector2 rollTargetXZ = Vector2.zero;

	public float MaxRollDuration = 5.0f;

	private readonly int OUTLINE_PLACEMENT_INVALID = 1;
	private readonly int OUTLINE_PLACEMENT_VALID = 2;

	private void Awake() {
		meshRenderer = GetComponent<MeshRenderer>();
		rb = GetComponent<Rigidbody>();
		sphereCollider = GetComponent<SphereCollider>();
		boxCollider = GetComponent<BoxCollider>();
		outline = GetComponent<Outline>();
		gesture = GetComponent<TransformGesture>();
		particles = GetComponent<ParticleSystem>();
		particleRenderer = GetComponent<ParticleSystemRenderer>();
	}

	private void OnEnable() {
		state = BallState.AbilityActivated;
		ResetTransform();

		gesture.enabled = true;
		gesture.ForceNoApplyTransform = false;
		gesture.TransformStarted += transformStartedHandler;
		gesture.Transformed += transformedHandler;
		gesture.TransformCompleted += transformCompletedHandler;

		makeFullyInvisible();
		setColliderToInterceptTouches(true);
	}

	private void OnDisable() {
		state = BallState.Disabled;
		boxCollider.enabled = false;
		sphereCollider.enabled = false;

		gesture.TransformStarted -= transformStartedHandler;
		gesture.Transformed -= transformedHandler;
		gesture.TransformCompleted -= transformCompletedHandler;
	}

	private void Update() {
		if (isPlacing()) {
			PlacingUpdate();
		}
		else if (isRolling()) {
			RollingUpdate();
		}
		else if (state == BallState.Finished_ParticlesAlive) {
			this.enabled = false;
//			float t = (Time.time - emitTime) / particles.main.startLifetime.constant;
//			if (t < 1.0f) {
//				var alpha = particles.colorOverLifetime.color.Evaluate(t).a;
//
//				var color = particleRenderer.material.color;
//				color.a = alpha;
//				particleRenderer.material.color = color;
//			}
//
//			else {
//				this.enabled = false;
//			}
		}
	}

	private void StartRolling() {
		gesture.ForceNoApplyTransform = true;
		rollStartTime = Time.time;
		rb.isKinematic = false;

		controller.HandleBallPlaced();
	}

	private void HandleInvalidPlacement() {
		makeFullyInvisible();
		boxCollider.enabled = false;

		controller.Reset();
	}
		
	private void PlacingUpdate() {
		Vector2 xz = transform.position.xz();
		if (xz.magnitude <= ValidPlacementRadius) {
			state = BallState.Placing_ValidPosition;
			outline.color = OUTLINE_PLACEMENT_VALID;
		} else {
			state = BallState.Placing_InvalidPosition;
			outline.color = OUTLINE_PLACEMENT_INVALID;
		}
	}

	private void RollingUpdate() {
		if (Time.time - rollStartTime >= MaxRollDuration) {
			Debug.Log("Time's up for Rolling Ball of Death");
			state = BallState.Finished_ParticlesAlive;
//			particles.Emit(DestroyedParticleCount);
//			emitTime = Time.time;
//			var color = particleRenderer.material.color;
//			color.a = 1.0f;
//			particleRenderer.material.color = color;

			rb.isKinematic = true;
			makeFullyInvisible();
			sphereCollider.enabled = false;

			controller.HandleAbilityFinished();
		}

		if (state == BallState.Rolling_HasTarget) {
			Vector3 relativePosition = new Vector3(rollTargetXZ.x, transform.position.y, rollTargetXZ.y) - transform.position;
			float magnitude = relativePosition.magnitude;
			Vector3 direction = relativePosition.normalized;

			Vector3 force = SteerMultiplier * direction * Mathf.Pow(magnitude, 0.5f);
			rb.AddForce(force);
		}

		Vector3 velocity = rb.velocity;
		float velmag = velocity.magnitude;
		velmag = velmag - Time.deltaTime * VelocityDampeningPerSecond * velmag;
		rb.velocity = velmag * velocity.normalized;
	}

	private void OnCollisionEnter(Collision col) {
		if (col.gameObject.CompareTag(Tags.Enemy)) {
			var enemy = col.gameObject;
			enemy.GetComponent<SimpleAgent>().handleHitByRollingBall(this);

			Debug.Log("Rolling Ball of Death killed an enemy pirate");
		}
	}

	private bool isPlacing() {
		return state == BallState.Placing_InvalidPosition || state == BallState.Placing_ValidPosition;
	}

	private bool isRolling() {
		return state == BallState.Rolling_NoTarget || state == BallState.Rolling_HasTarget;
	}

	private void transformStartedHandler(object sender, EventArgs e) {
		if (state == BallState.AbilityActivated) {
			state = BallState.Placing_ValidPosition;

			var position = transform.position;
			position.x = gesture.Touch_WorldPosition.x;
			position.z = gesture.Touch_WorldPosition.z;
			transform.position = position;

			makeOutlineVisibleOnly();
			setColliderToInterceptTouches(false);
		}

		else if (isRolling()) {
			state = BallState.Rolling_HasTarget;
			rollTargetXZ = gesture.Touch_WorldPosition.xz();
		}
	}

	private void transformedHandler(object sender, EventArgs e) {
		if (!isRolling()) {
			return;
		}
			
		rollTargetXZ = gesture.Touch_WorldPosition.xz();
	}

	private void transformCompletedHandler(object sender, EventArgs e) {
		if (isPlacing()) {
			if (state == BallState.Placing_ValidPosition) {
				makeVisible();
				state = BallState.Rolling_NoTarget;
				StartRolling();
			}
			else {
				HandleInvalidPlacement();
			}
		}

		else if (isRolling()) {
			state = BallState.Rolling_NoTarget;
		}
	}

	private void setColliderToInterceptTouches(bool shouldInterceptTouches) {
		if (shouldInterceptTouches) {
			sphereCollider.enabled = false;
			boxCollider.enabled = true;
		}

		else {
			sphereCollider.enabled = true;
			boxCollider.enabled = false;
		}
	}

	private void makeFullyInvisible() {
		meshRenderer.enabled = false;
	}

	private void makeOutlineVisibleOnly() {
		outline.enabled = true;
		meshRenderer.enabled = true;
		meshRenderer.material = InvisibleMaterial;
		meshRenderer.receiveShadows = false;
		meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
	}

	private void makeVisible() {
		outline.enabled = false;
		meshRenderer.enabled = true;
		meshRenderer.material = BallMaterial;
		meshRenderer.receiveShadows = true;
		meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
	}

	private void ResetTransform() {
		transform.position = new Vector3(0.0f, 5.0f, 0.0f);
		transform.rotation = Quaternion.identity;
	}
}
