using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EazyTools.SoundManager;

using DG.Tweening;

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

		DOTween.Sequence()
			.AppendCallback(PlayIntroMusic)
			.AppendInterval(timeUntilStart)
			.AppendCallback(PlayGameLoopMusic)
			.Play();
	}

	private void Update () {
		if (!hasStartedPlaying && Time.timeSinceLevelLoad >= timeUntilStart) {
			hasStartedPlaying = true;

			var clip = AudioClips.Instance.Music.GameMusic.GetAny();
			music = SoundManager.GetMusicAudio(SoundManager.PlayMusic(clip, 1.0f, true, false));
			music.fadeInSeconds = 0.05f;
		}
	}

	private void PlayIntroMusic() {
		var clip = AudioClips.Instance.Music.GameIntro.GetAny();
		music = SoundManager.GetMusicAudio(SoundManager.PlayMusic(clip, 1.0f, false, false));
	}

	private void PlayGameLoopMusic() {
		var clip = AudioClips.Instance.Music.GameMusic.GetAny();
		music = SoundManager.GetMusicAudio(SoundManager.PlayMusic(clip, 1.0f, true, false));
	}

	public void PauseGameMusic() {
		music.Pause();
	}

	public void ResumeGameMusic() {
		music.Resume();
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
