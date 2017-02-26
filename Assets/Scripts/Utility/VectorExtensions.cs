using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExtensions {
	public static Vector2 xy(this Vector3 vec3) {
		return new Vector2(vec3.x, vec3.y);
	}

	public static Vector2 yx(this Vector3 vec3) {
		return new Vector2(vec3.y, vec3.x);
	}

	public static Vector2 xz(this Vector3 vec3) {
		return new Vector2(vec3.x, vec3.z);
	}

	public static Vector2 zx(this Vector3 vec3) {
		return new Vector2(vec3.z, vec3.x);
	}

	public static Vector2 yz(this Vector3 vec3) {
		return new Vector2(vec3.y, vec3.z);
	}

	public static Vector2 zy(this Vector3 vec3) {
		return new Vector2(vec3.z, vec3.y);
	}
}
