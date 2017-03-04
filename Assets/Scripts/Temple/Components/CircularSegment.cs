using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularSegment : MonoBehaviour {
	private CircularLevelDescription LevelDescription {
		get {
			return transform.parent.GetComponent<CircularLevel>().Description;
		}
	}

	public float InnerRadius {
		get {
			return LevelDescription.InnerRadius;
		}
	}

	public float Thickness {
		get {
			return LevelDescription.Thickness;
		}
	}

	public float Height {
		get {
			return LevelDescription.Height;
		}
	}

	public Arc Arc;
}
