using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PirateShip : MonoBehaviour {
	public GameObject cannon;
	public GameObject cannonballSpawnPoint;

	public GameObject cannonballPrefab;

	private Vector3 calcVelocityToHitTarget(Vector3 origin, Vector3 target, float initialSpeed) {
		Vector3 diff = target - origin;
		Vector2 dxz = diff.xz();

		float x = dxz.magnitude;
		float y = diff.y;

		float v2 = Mathf.Pow(initialSpeed, 2);
		float g = Physics.gravity.magnitude;

		// https://en.wikipedia.org/wiki/Trajectory_of_a_projectile#Angle_.7F.27.22.60UNIQ--postMath-00000010-QINU.60.22.27.7F_required_to_hit_coordinate_.28x.2Cy.29
		float angle = Mathf.Atan((v2 - Mathf.Sqrt(v2 * v2 - g * (g * x * x + 2 * y * v2 ))) / (g * x));

		float vy = initialSpeed * Mathf.Sin(angle);

		float vxz = initialSpeed * Mathf.Cos(angle);
		float vx = dxz.normalized.x * vxz;
		float vz = dxz.normalized.y * vxz;

		return new Vector3(vx, vy, vz);
	}

	public float CannonballSpeed = 15.0f;
	public float CannonRotationDuration = 1.0f;
	public float CannonFireWaitDuration = 0.4f;
	public float CannonReshootWaitDuration = 2.0f;

	private Vector3 cannonballTarget;

	public void FireCannonballAtSegment(GameObject segment, Vector3 target) {
		cannonballTarget = target;

		GameObject cannonball = GameObject.Instantiate(cannonballPrefab);
		cannonball.GetComponent<MeshRenderer>().enabled = false;

		var animation = DOTween.Sequence();

		//var lookAt = target + Vector3.up * 10.0f;
		var lookAt = calcVelocityToHitTarget(cannon.transform.position, cannonballTarget, CannonballSpeed).normalized;

		animation
			.Append(cannon.transform.DOLookAt(cannon.transform.position + lookAt, CannonRotationDuration))
			.AppendInterval(CannonFireWaitDuration)
			.AppendCallback(() => {
				cannonball.transform.position = cannonballSpawnPoint.transform.position;
				cannonball.GetComponent<MeshRenderer>().enabled = true;

				var rb = cannonball.GetComponent<Rigidbody>();
				rb.velocity = calcVelocityToHitTarget(cannonball.transform.position, cannonballTarget, CannonballSpeed);
			})
			.AppendInterval(CannonReshootWaitDuration)
			.AppendCallback(() => {
				SimpleAgent.shipIsCurrentlyFiring = false;
			})
			.Play();
	}
}
