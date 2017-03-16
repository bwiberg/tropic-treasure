using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using cakeslice;
using EazyTools.SoundManager;

public class CircularSegment : MonoBehaviour {
	private CircularLevel ParentLevel {
		get {
			return transform.parent.GetComponent<CircularLevel>();
		}
	}

	public float InnerRadius {
		get {
			return ParentLevel.InnerRadius;
		}
	}

	public float OuterRadius {
		get {
			return InnerRadius + Thickness;
		}
	}

	public float Thickness {
		get {
			return ParentLevel.Thickness;
		}
	}

	public float Height {
		get {
			return ParentLevel.Height;
		}
	}

	public int SegmentIndex;

	public float AngleStart;
	public float AngleEnd;

	public float Angle {
		get {
			return AngleEnd - AngleStart;
		}
	}

	public SegmentObstacle[] Obstacles;

	[SerializeField] private Material outlineOnlyMaterial;
	[SerializeField] private Material originalMaterial;

	[SerializeField] private float EmittedParticlesConstant;

	private MeshRenderer meshRenderer;
	private MeshCollider meshCollider;
	private Outline outline;

	private ShadowCastingMode initialShadowCastingMode;

	private void Awake() {
		meshRenderer = GetComponent<MeshRenderer>();
		meshCollider = GetComponent<MeshCollider>();
		outline = GetComponent<Outline>();
		initialShadowCastingMode = meshRenderer.shadowCastingMode;
	}

	private void Start() {
		Obstacles = GetComponentsInChildren<SegmentObstacle>();
	}

	public void renderOutlineOnly(bool renderOutlineOnly) {
		outline.enabled = renderOutlineOnly;
		foreach (var obstacle in Obstacles) {
			obstacle.ShouldEmitDustParticles(!renderOutlineOnly);
		}

		if (renderOutlineOnly) {
			meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			meshRenderer.material = outlineOnlyMaterial;
		} else {
			meshRenderer.shadowCastingMode = initialShadowCastingMode;
			meshRenderer.material = originalMaterial;
		}
	}

	public void setEnableObstacle(bool isActive) {
		foreach (var obstacle in Obstacles) {
			obstacle.Obstacle.enabled = isActive;
		}
	}
		
	public void handleCannonballHit() {
		// Disable this segment in the clone object
		if (ParentLevel.Clone != null) {
			ParentLevel.Clone.GetSegmentByIndex(SegmentIndex).gameObject.SetActive(false);
		}
			
		meshRenderer.enabled = false;
		meshCollider.enabled = false;
		setEnableObstacle(false);

		int particleCount = Mathf.RoundToInt(Angle * Height * (Mathf.Pow(OuterRadius, 2) - Mathf.Pow(InnerRadius, 2)) * EmittedParticlesConstant);

		for (int i = 0; i < Obstacles.Length; ++i) {
			Obstacles[i].EmitDestroyedParticles(Mathf.CeilToInt(particleCount / Obstacles.Length));
		}

		var clip = AudioClips.Instance.Walls.Destroyed.GetAny();
		SoundManager.PlaySound(clip);
	}
}
