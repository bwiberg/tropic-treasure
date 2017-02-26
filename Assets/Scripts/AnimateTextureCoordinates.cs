using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateTextureCoordinates : MonoBehaviour {
	private Mesh mesh;
	private MeshRenderer renderer;

	public Vector2 AnimationSpeed = Vector2.zero;

	// Use this for initialization
	void Start () {
		mesh = GetComponent<MeshFilter>().mesh;
		renderer = GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		var uvs = mesh.uv;

		uvs[0] += AnimationSpeed * Time.deltaTime;

		mesh.uv = uvs;

		renderer.material.SetTextureOffset("_MainTex", AnimationSpeed * Time.time);
	}
}
