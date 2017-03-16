using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayScore : MonoBehaviour {

	public float waitDuration;

	public GameObject clock;
	public float rotationSpeed = 3.0f;
	public float deltaAngle = 12.0f;  

	private Text timeText;
	private float continousTime;
	private float seconds;

	public int getScore() {
		return (int) seconds;
	}

	// Use this for initialization
	void Start () {
		continousTime = 0;
		seconds = continousTime;
		timeText = GetComponent<Text>();
		timeText.text = "" + Mathf.FloorToInt(seconds);
		UpdateTime(0);
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (Time.timeSinceLevelLoad <= waitDuration) {
			return;
		}

		//Remove once we have gamelogic
		float timeUpdate = Time.fixedDeltaTime;
		UpdateTime(timeUpdate);
	}

	public void UpdateTime(float newTime) {
		continousTime += newTime;
		if(continousTime-seconds > 1.0f)
		{
			seconds += 1;
			timeText.text = "" + Mathf.FloorToInt(seconds);
		}
		float angle = Mathf.Lerp(deltaAngle*(seconds), deltaAngle*(1+seconds), (continousTime-seconds)*rotationSpeed);
		clock.transform.rotation = Quaternion.Euler(0f, 0f, -angle);
		transform.localRotation = Quaternion.Euler(0f, 0f, angle);
	}
}
