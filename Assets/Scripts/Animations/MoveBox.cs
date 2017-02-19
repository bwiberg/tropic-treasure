using UnityEngine;
using System.Collections;

public class MoveBox : MonoBehaviour {
	public float RotateSpeed = 2.0f;
    public float RadiusX = 1.0f;
    public float RadiusY = 1.0f;
    private Vector3 _centre;
    private float _angle;

	// Use this for initialization
	void Start () {
		_centre = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		_angle += RotateSpeed * Time.deltaTime;

		var offset = new Vector3(RadiusX*Mathf.Sin(_angle), RadiusY*Mathf.Cos(_angle), 0.0f);
		transform.position = _centre + offset;
	}
}
