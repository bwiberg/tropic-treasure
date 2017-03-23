using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EazyTools.SoundManager;

public class MenuMusicPlayer : MonoBehaviour {

	private Audio audioMusic;
	private Audio audioOceanWaves;

	private string volumeKey = "volume";

	private void Start () {
		if(PlayerPrefs.HasKey(volumeKey))
		{
			float volume = PlayerPrefs.GetFloat(volumeKey);
			SetVolume(volume);
		}
		else
		{
			SetVolume(1.0f);
		}

		var clip = AudioClips.Instance.Ambience.OceanWaves.GetAny();
		audioOceanWaves = SoundManager.GetAudio(SoundManager.PlaySound(clip, 1.0f, true, null));

		clip = AudioClips.Instance.Music.MainMenu.GetAny();
		audioMusic = SoundManager.GetAudio(SoundManager.PlayMusic(clip, 1.0f, true, false));
		audioMusic.fadeInSeconds = 0.1f;
		audioMusic.fadeOutSeconds = 0.25f;
	}
	
	public void FadeOutMusic() {
		audioOceanWaves.fadeOutSeconds = 2.0f;
		audioOceanWaves.Stop();
	}

	public void SetVolume(float volume)	{
		SoundManager.globalVolume = volume;
		SoundManager.globalMusicVolume = 0.25f * volume;
		SoundManager.globalUISoundsVolume = volume;
		SoundManager.globalSoundsVolume = volume;
		AudioListener.volume = volume;
	}
}
