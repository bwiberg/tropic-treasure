using UnityEngine;
using System.Collections;

public class CubeAnimationController : MonoBehaviour {
	public float inputSpeed = 4.0f;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		float hSpeed = Input.GetAxis("Horizontal");
		float vSpeed = Input.GetAxis("Vertical");
		transform.Translate(new Vector3(hSpeed, 0.0f, vSpeed) * Time.deltaTime * inputSpeed);
	}
}
