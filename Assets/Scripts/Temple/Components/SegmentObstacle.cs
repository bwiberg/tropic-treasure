using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SegmentObstacle : MonoBehaviour {
	public ParticleSystem dustParticles;
	public ParticleSystemRenderer dustParticlesRenderer;

	public ParticleSystem destroyedParticles;
	public ParticleSystemRenderer destroyedParticlesRenderer;

	public NavMeshObstacle Obstacle;

	public int DustParticlesPerDistance = 1;

	private float emitTime;
	private bool hasEmitted = false;

	private void Awake() {
		Obstacle = GetComponent<NavMeshObstacle>();
	}

	public void EmitDestroyedParticles(int count) {
		emitTime = Time.time;
		hasEmitted = true;
		destroyedParticles.Emit(count);
		dustParticles.Emit(count); 
	}

	public void ShouldEmitDustParticles(bool shouldEmit) {
		if (shouldEmit) {
			var emission = dustParticles.emission;
			emission.rateOverDistance = DustParticlesPerDistance;
		}
		else {
			var emission = dustParticles.emission;
			emission.rateOverDistance = 0;
		}
	}

	private void Update() {
		if (hasEmitted) {
			float t = (Time.time - emitTime) / destroyedParticles.main.startLifetime.constant;
			var alpha = destroyedParticles.colorOverLifetime.color.Evaluate(t).a;

			var color = destroyedParticlesRenderer.material.color;
			color.a = alpha;
			destroyedParticlesRenderer.material.color = color;
		}
	}
}
