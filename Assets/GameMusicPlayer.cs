using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EazyTools.SoundManager;

public class GameMusicPlayer : MonoBehaviour {
	public bool hasStartedPlaying = false;
	public float timeUntilStart = 5.0f;

	private Audio music;
	private string volumeKey = "volume";

	private void Start() {
		if(PlayerPrefs.HasKey(volumeKey))
		{
			float volume = PlayerPrefs.GetFloat(volumeKey);
			SetVolume(volume);
		}
		else
		{
			SetVolume(1.0f);
		}
	}

	private void Update () {
		if (!hasStartedPlaying && Time.time >= timeUntilStart) {
			hasStartedPlaying = true;

			var clip = AudioClips.Instance.Music.GameMusic.GetAny();
			music = SoundManager.GetMusicAudio(SoundManager.PlayMusic(clip));
			music.fadeInSeconds = 0.05f;
			music.loop = true;
		}
	}

	public void SetVolume(float volume)
	{
		SoundManager.globalVolume = volume;
		SoundManager.globalMusicVolume = 0.25f * volume;
		SoundManager.globalUISoundsVolume = volume;
		SoundManager.globalSoundsVolume = volume;
		AudioListener.volume = volume;
	}
}
