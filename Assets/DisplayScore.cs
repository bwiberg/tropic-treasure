using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayScore : MonoBehaviour {

	private Text scoreText;
	private float score;

	// Use this for initialization
	void Start () {
		score = 0;
		scoreText = GetComponent<Text>();
		UpdateScore(0);
	}

	// Update is called once per frame
	void Update () {
		//Remove once we have gamelogic
		float scoreUpdate = Time.deltaTime;
		UpdateScore(scoreUpdate);
	}

	public void UpdateScore(float newScore) {
		score += newScore;
		scoreText.text = "Time: " + Mathf.FloorToInt(score);
	}
}
