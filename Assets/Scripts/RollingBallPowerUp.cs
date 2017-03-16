using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using EazyTools.SoundManager;

public class RollingBallPowerUp : MonoBehaviour {
	public float initialWaitDuration;

	public float chargingTime = 10.0f;

	public RollingBallOfDeath ball;

	public Text InstructionText;
	public float InstructionTextDuration = 5.0f;
	public float InstructionTextFadeDuration = 2.0f;

	public bool IsFirstTime = true;

	private bool isCharging = true;

	private float remainingActiveTime;
	private Image image;
	private Button button;

	// Use this for initialization
	void Start () {
		image = GetComponent<Image>();
		button = GetComponent<Button>();
		remainingActiveTime = 1.0f;
		image.fillAmount = 0.0f;
		button.interactable = false;
	}

	// Update is called once per frame
	void Update () {
		if (Time.time <= initialWaitDuration) {
			return;
		}

		if (isCharging) {
			Charge();
		}
	}

	void Charge() {
		image.fillAmount += Time.deltaTime/chargingTime;

		if(image.fillAmount >= 1) {
			Reset();

			var clip = AudioClips.Instance.Abilities.Recharged.GetAny();
			var audio = SoundManager.GetUISoundAudio(clip);
			if (audio == null || !audio.playing) {
				SoundManager.PlayUISound(clip);
			};
		}
	}

	public void Activate() {
		if (IsFirstTime) {
			IsFirstTime = false;
			InstructionText.enabled = true;
			DOTween.Sequence()
				.AppendInterval(InstructionTextDuration - InstructionTextFadeDuration)
				.Append(InstructionText.DOFade(0.0f, InstructionTextFadeDuration).SetEase(Ease.InOutQuad))
				.AppendCallback(() => {
					InstructionText.enabled = false;
				})
				.Play();
		}

		ball.enabled = true;
		button.interactable = false;
	}

	public void Reset() {
		ball.enabled = false;
		isCharging = false;
		image.fillAmount = 1;
		button.interactable = true;
		button.gameObject.SetActive(false);
		button.gameObject.SetActive(true);
	}

	public void HandleBallPlaced() {
		InstructionText.enabled = false;
		image.fillAmount = 0.0f;
	}

	public void HandleAbilityFinished() {
		isCharging = true;
	}
}
