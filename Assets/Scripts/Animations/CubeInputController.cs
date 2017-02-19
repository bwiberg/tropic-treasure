using UnityEngine;
using System.Collections;

public class CubeInputController : MonoBehaviour {
	public float animationSpeed = 5.0f;
	private Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		animator.speed = animationSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		float hSpeed = Input.GetAxis("Horizontal");
		float vSpeed = Input.GetAxis("Vertical");
		float totalSpeed = Mathf.Sqrt(hSpeed * hSpeed + vSpeed * vSpeed);

		if (totalSpeed > 0.1) {
    		animator.SetBool("isRun", true);
		}
		else {
			animator.SetBool("isRun", false);
		}
	}
}
