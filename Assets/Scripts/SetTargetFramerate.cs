using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTargetFramerate : MonoBehaviour {
	public int TargetFramerate = 60;

	void Awake() {
		Application.targetFrameRate = TargetFramerate;
	}
}