using UnityEngine;
using System.Collections;

public class MoveCube : MonoBehaviour {
	public float animationSpeed = 5.0f;
	// Use this for initialization
	void Start () {
		Animator animator = GetComponent<Animator>();
		animator.speed = animationSpeed;
	}
	
	// Update is called once per frame
	void Update () {

	}
}
