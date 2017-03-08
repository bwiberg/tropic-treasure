using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using cakeslice;

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

	public Arc Arc;

	public Material outlineOnlyMaterial;
	public Material originalMaterial;

	private MeshRenderer meshRenderer;
	private Outline outline;

	private ShadowCastingMode initialShadowCastingMode;

	private void Start() {
		meshRenderer = GetComponent<MeshRenderer>();
		outline = GetComponent<Outline>();

		initialShadowCastingMode = meshRenderer.shadowCastingMode;
	}

	public void renderOutlineOnly(bool renderOutlineOnly) {
		outline.enabled = renderOutlineOnly;

		if (renderOutlineOnly) {
			meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			meshRenderer.material = outlineOnlyMaterial;
		} else {
			meshRenderer.shadowCastingMode = initialShadowCastingMode;
			meshRenderer.material = originalMaterial;
		}
	}

	public void setEnableObstacle(bool isActive) {
		foreach (var obstacle in GetComponentsInChildren<NavMeshObstacle>()) {
			obstacle.enabled = isActive;
		}
	}

	public void handleCannonballHit(Cannonball cannonball) {
		GameObject.Destroy(cannonball);

	}
}
