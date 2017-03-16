using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EazyTools.SoundManager;

public class VolumeControl : MonoBehaviour {

	public Slider slider;

	public void setVolume()
	{
		SoundManager.globalVolume = slider.value;
		SoundManager.globalMusicVolume = 0.25f * slider.value;
	}
}
