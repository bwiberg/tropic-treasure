using UnityEngine;
using UnityEngine.UI;
using EazyTools.SoundManager;

public class DifficultySoundPlayer : MonoBehaviour {
	public Toggle[] toggles;

	void OnEnable () {
		foreach (var toggle in toggles) {
			toggle.onValueChanged.AddListener(PlayToggleSound);
		}
	}

	void OnDisable () {
		foreach (var toggle in toggles) {
			toggle.onValueChanged.RemoveListener(PlayToggleSound);
		}
	}

	private void PlayToggleSound(bool play) {
		var clip = AudioClips.Instance.UI.MenuSelect.GetAny();
		SoundManager.PlayUISound(clip);
	}
}
