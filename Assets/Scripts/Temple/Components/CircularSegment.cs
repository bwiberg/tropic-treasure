using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
