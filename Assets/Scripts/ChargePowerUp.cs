using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EazyTools.SoundManager;
using UnityEngine.AI;

public class ChargePowerUp : MonoBehaviour {

	public float chargingTime = 10.0f;
	public float activeTime = 5.0f;

	public bool isActive;

	public float initialWaitDuration = 5.0f;

	private bool isCharged = false;
	private bool enemiesFrozen = false;

	private float remainingActiveTime;
	private Image image;
	private Button button;
	private GameObject[] enemies;
	private Animator enemyAnim;

	// Use this for initialization
	void Start () {
		image = GetComponent<Image>();
		button = GetComponent<Button>();
		isActive = false;
		remainingActiveTime = 1.0f;
		image.fillAmount = 0.0f;
		button.interactable = false;
	}

	// Update is called once per frame
	void Update () {
		if (Time.timeSinceLevelLoad <= initialWaitDuration) {
			return;
		}

		if(isActive) {
			Deplete();
		}
		else if(!isCharged) {
			Charge();
		}
	}

	void Charge() {
		if (enemiesFrozen) {
			EnableEnemies ();
			enemiesFrozen = false;
		}
		image.fillAmount += Time.deltaTime/chargingTime;

		if(image.fillAmount >= 1) {
			isCharged = true;
			button.interactable = true;

			var clip = AudioClips.Instance.Abilities.Recharged.GetAny();
			var audio = SoundManager.GetUISoundAudio(clip);
			if (audio == null || !audio.playing) {
				SoundManager.PlayUISound(clip);
			};
		}
	}

	void Deplete() {
		remainingActiveTime -= Time.deltaTime/activeTime;
//		if (enemiesFrozen) {
//			EnableEnemies ();
//			enemiesFrozen = false;
//		}
		if(remainingActiveTime <= 0) 
		{
			remainingActiveTime = 1.0f;
			isActive = false;
		}
	}

	public void Activate() {
		

		Reset();
		isActive = true;
		enemiesFrozen = true;
	}

	void Reset() {
		image.fillAmount = 0;
		isCharged = false;
		button.interactable = false;
		button.gameObject.SetActive(false);
		button.gameObject.SetActive(true);
	}

	void DisableEnemies() {
		enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		for (int i = 0; i < enemies.Length; i++) {
			// stop enemies from moving
			enemies[i].GetComponent<NavMeshAgent> ().Stop();

			//switch to quake animation
			enemyAnim = enemies[i].GetComponentInChildren<Animator>();
			enemyAnim.SetBool ("powerActivated", true);
		}
		enemiesFrozen = true;
	}

	void EnableEnemies() {
		enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		for (int i = 0; i < enemies.Length; i++) {
			//switch to walking animation
			enemyAnim = enemies[i].GetComponentInChildren<Animator>();
			enemyAnim.SetBool ("powerActivated", false);

			// let enemies move
			enemies[i].GetComponent<NavMeshAgent> ().Resume();
			enemiesFrozen = false;
		}
	}
}