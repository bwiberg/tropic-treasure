using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EazyTools.SoundManager;

public class SettingsManager : MonoBehaviour {

	public GameObject volumeControl;
	public List<GameObject> difficultyControls;

	private Slider volumeSlider;

	private List<Toggle> toggles;
	private string volumeKey = "volume";
	private string difficultyKey = "difficultyBox";
	private int current_difficulty = 0;

	// Use this for initialization
	void Start () {
		volumeSlider = volumeControl.GetComponent<Slider>();

		toggles = new List<Toggle>();
		for(int i=0; i < difficultyControls.Count; i++)
		{
			toggles.Add(difficultyControls[i].GetComponent<Toggle>());
		}

		if(PlayerPrefs.HasKey(volumeKey))
		{
			volumeSlider.value = PlayerPrefs.GetFloat(volumeKey);
			SetVolume();
		}

		if(PlayerPrefs.HasKey(difficultyKey))
		{
			UpdateToggles(PlayerPrefs.GetInt(difficultyKey));
		}
		else
		{
			UpdateToggles(current_difficulty);
		}
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void SaveVolume () {
		SetVolume();
		PlayerPrefs.SetFloat(volumeKey, volumeSlider.value);
	}

	public void SaveDifficulty () {
		PlayerPrefs.SetInt(difficultyKey, current_difficulty);
	}

	public void UpdateToggles(int difficulty)
	{
		current_difficulty = difficulty;
		toggles[current_difficulty].isOn = true;
		SaveDifficulty();
	}

	public void SetVolume() {
		SoundManager.globalVolume = volumeSlider.value;
		SoundManager.globalMusicVolume = 0.25f * volumeSlider.value;
		SoundManager.globalUISoundsVolume = volumeSlider.value;
		SoundManager.globalSoundsVolume = volumeSlider.value;
		AudioListener.volume = volumeSlider.value;
	}
}
