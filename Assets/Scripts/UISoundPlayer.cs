using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EazyTools.SoundManager;

public class UISoundPlayer : MonoBehaviour {
	public void PlaySelectSound() {
		var clip = AudioClips.Instance.UI.MenuSelect.GetAny();
		SoundManager.PlayUISound(clip);
	}

	public void PlayBackSound() {
		var clip = AudioClips.Instance.UI.MenuSelect.GetAny();
		SoundManager.PlayUISound(clip);
	}

	public void PlayConfirmSound() {
		var clip = AudioClips.Instance.UI.MenuConfirm.GetAny();
		SoundManager.PlayUISound(clip);
	}

	public void PlayUseAbilitySound() {
		var clip = AudioClips.Instance.Abilities.Used.GetAny();
		SoundManager.PlayUISound(clip);
	}
}
