using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SegmentObstacle : MonoBehaviour {
	[SerializeField] private ParticleSystem particles;
	[SerializeField] private ParticleSystemRenderer particleRenderer;

	public NavMeshObstacle Obstacle;

	private float emitTime;
	private bool hasEmitted = false;

	private void Awake() {
		Obstacle = GetComponent<NavMeshObstacle>();
	}

	public void EmitParticles(int count) {
		emitTime = Time.time;
		hasEmitted = true;
		particles.Emit(count);
	}

	private void Update() {
		if (hasEmitted) {
			float t = (Time.time - emitTime) / particles.main.startLifetime.constant;
			var alpha = particles.colorOverLifetime.color.Evaluate(t).a;

			var color = particleRenderer.material.color;
			color.a = alpha;
			particleRenderer.material.color = color;
		}
	}
}
