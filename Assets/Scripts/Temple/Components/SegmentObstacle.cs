using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentObstacle : MonoBehaviour {
	[SerializeField] private ParticleSystem particles;

	public void EmitParticles(int count) {
		particles.Emit(count);
	}
}
