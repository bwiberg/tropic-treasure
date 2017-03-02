using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayScore : MonoBehaviour {

	public GameObject clock;
	public float rotationSpeed = 3.0f;
	public float deltaAngle = 12.0f;  

	private Text timeText;
	private float continousTime;
	private float seconds;


	// Use this for initialization
	void Start () {
		continousTime = 120;
		seconds = continousTime;
		timeText = GetComponent<Text>();
		timeText.text = "" + Mathf.FloorToInt(seconds);
		UpdateTime(0);
	}

	// Update is called once per frame
	void FixedUpdate () {
		//Remove once we have gamelogic
		float timeUpdate = Time.deltaTime;
		UpdateTime(timeUpdate);
	}

	public void UpdateTime(float newTime) {
		continousTime -= newTime;
		if(seconds-continousTime > 1.0f)
		{
			seconds -= 1;
			timeText.text = "" + Mathf.FloorToInt(seconds);
		}
		float angle = Mathf.Lerp(deltaAngle*(seconds+1), deltaAngle*(seconds), (seconds-continousTime)*rotationSpeed);
		clock.transform.rotation = Quaternion.Euler(0f, 0f, angle);
		transform.localRotation = Quaternion.Euler(0f, 0f, -angle);
	}
}
