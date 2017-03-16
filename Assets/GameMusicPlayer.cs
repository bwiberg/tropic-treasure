using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EazyTools.SoundManager;

public class GameMusicPlayer : MonoBehaviour {
	public bool hasStartedPlaying = false;
	public float timeUntilStart = 5.0f;

	private Audio music;

	private void Start() {
		SoundManager.globalMusicVolume = 0.25f;
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
}
