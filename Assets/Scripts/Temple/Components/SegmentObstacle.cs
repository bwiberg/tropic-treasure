using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentObstacle : MonoBehaviour {
	[SerializeField] private ParticleSystem particles;
	[SerializeField] private ParticleSystemRenderer particleRenderer;

	private float emitTime;
	private bool hasEmitted = false;

	public void EmitParticles(int count) {
		emitTime = Time.time;
		hasEmitted = true;
		particles.Emit(count);
	}

	private void Update() {
		if (hasEmitted) {
			float t = (Time.time - emitTime) / particles.main.startLifetime.constant;
			Debug.Log(t);
			var alpha = particles.colorOverLifetime.color.Evaluate(t).a;

			var color = particleRenderer.material.color;
			color.a = alpha;
			particleRenderer.material.color = color;
		}
	}
}
