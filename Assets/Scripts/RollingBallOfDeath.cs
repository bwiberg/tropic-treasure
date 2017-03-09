using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RollingBallOfDeath : MonoBehaviour {
	public enum State {
		Placing_ValidPosition,
		Placing_InvalidPosition,
		Rolling_HasTarget,
		Rolling_NoTarget
	}

	public State state;

	private Rigidbody rb;
	new private SphereCollider collider;

	private float rollStartTime;

	public float MaxRollDuration = 5.0f;

	[SerializeField] private Transform target;
	[SerializeField] private float initialRollSpeed;

	public void StartRolling(Vector3 toward) {
		var direction = (toward - transform.position).normalized;
		var velocity = direction * initialRollSpeed;
		rb.velocity = velocity;

		rollStartTime = Time.time;
	}

	private void Start() {
		rb = GetComponent<Rigidbody>();
		collider = GetComponent<SphereCollider>();

		StartRolling(target.position);
	}

	private void Update() {
		if (isPlacing()) {
			PlacingUpdate();
		}
		else {
			RollingUpdate();
		}
	}

	private void PlacingUpdate() {
		
	}

	private void RollingUpdate() {
		checkLifetime();
	}

	private void checkLifetime() {
		if (Time.time - rollStartTime >= MaxRollDuration) {
			Debug.Log("Time's up for Rolling Ball of Death");
			GameObject.Destroy(gameObject);
		}
	}

	private void OnCollisionEnter(Collision col) {
		if (col.gameObject.CompareTag(Tags.Enemy)) {
			var enemy = col.gameObject;
			enemy.GetComponent<SimpleAgent>().handleHitByRollingBall(this);

			Debug.Log("Rolling Ball of Death killed an enemy pirate");
		}
	}

	private bool isPlacing() {
		return state == State.Placing_InvalidPosition || state == State.Placing_ValidPosition;
	}

	private bool isRolling() {
		return !isPlacing();
	}
}
