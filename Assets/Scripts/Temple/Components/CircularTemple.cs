using UnityEngine;
using System.Collections.Generic;

public class CircularTemple : MonoBehaviour {
	public float WallRotationDurationFactor = 1.0f;

	public CircularLevel[] Levels;

	private void Start() {
		Levels = GetComponentsInChildren<CircularLevel>();
	}
}
