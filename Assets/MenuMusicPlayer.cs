using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EazyTools.SoundManager;

public class MenuMusicPlayer : MonoBehaviour {

	private Audio audioMusic;
	private Audio audioOceanWaves;

	private void Start () {
		var clip = AudioClips.Instance.Ambience.OceanWaves.GetAny();
		audioOceanWaves = SoundManager.GetAudio(SoundManager.PlaySound(clip));
		audioOceanWaves.fadeInSeconds = 2.0f;
		audioOceanWaves.loop = true;
	}
	
	public void FadeOutMusic() {
		audioOceanWaves.fadeOutSeconds = 2.0f;
		audioOceanWaves.Stop();
	}
}
