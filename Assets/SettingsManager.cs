using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour {

	public GameObject volumeControl;
	public GameObject difficultyControl;

	private Slider volumeSlider;
	private Slider difficultySlider;

	private string volumeKey = "volume";
	private string difficultyKey = "difficulty";

	// Use this for initialization
	void Start () {
		volumeSlider = volumeControl.GetComponent<Slider>();
		difficultySlider = difficultyControl.GetComponent<Slider>();

		if(PlayerPrefs.HasKey(volumeKey))
			volumeSlider.value = PlayerPrefs.GetFloat(volumeKey);

		if(PlayerPrefs.HasKey(difficultyKey))
			difficultySlider.value = PlayerPrefs.GetFloat(difficultyKey);
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void SaveVolume () {
		PlayerPrefs.SetFloat(volumeKey, volumeSlider.value);
	}

	public void SaveDifficulty () {
		PlayerPrefs.SetInt(difficultyKey, (int)difficultySlider.value);
	}
}
