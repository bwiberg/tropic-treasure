using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreManager : MonoBehaviour {

	public string place = "1.";

	private string nameKey;
	private string scoreKey;

	private int score;
	private string name;

	private const string mainNameKey = "HighscoreName";
	private const string mainScoreKey = "HighscoreScore";

	private Text guiText;

	// Use this for initialization
	void Start () {
		guiText = GetComponent<Text>();

		nameKey = mainNameKey + place;
		scoreKey = mainScoreKey + place;

		name = PlayerPrefs.GetString(nameKey, "Nobody");
		score = PlayerPrefs.GetInt(scoreKey,0);

		if(score > 0)
			guiText.text = place + " " + name + "    " + score.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
