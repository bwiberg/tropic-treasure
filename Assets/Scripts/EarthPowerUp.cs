using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EarthPowerUp : MonoBehaviour {
	public float initialWaitDuration;

	public float chargingTime = 10.0f;
	public float activeTime = 5.0f;

	public Text InstructionText;
	public float InstructionTextDuration = 5.0f;
	public float InstructionTextFadeDuration = 2.0f;

	public bool IsFirstTime = true;

	private bool isCharged;
	private bool isActive;

	private float remainingActiveTime;
	private Image image;
	private Button button;

	public float shakeDetectionThreshold;
	private static float AccelUpdateInterval = 1.0f / 60.0f;
	private static float LowPassKernelWidthInSeconds = 1.0f;
	private float LowPassFilterFactor = AccelUpdateInterval / LowPassKernelWidthInSeconds;
	private Vector3 LowPassValue = Vector3.zero;
	private Vector3 Acceleration;
	private Vector3 FilteredAcceleration;
	private Vector3 ChestLocation;
	private int shakeCounter;

	private GameObject[] enemies;
	private GameObject[] enemiesKill;
	private float[] enemyDistanceFromChest;
	private int maxNumEnemies;




	// Use this for initialization
	void Start () {
		image = GetComponent<Image>();
		button = GetComponent<Button>();
		remainingActiveTime = 1.0f;
		image.fillAmount = 0;
		isCharged = false;
		isActive = false;
		button.interactable = false;

		LowPassValue = Input.acceleration;
		shakeCounter = 0;

		ChestLocation = GameObject.FindGameObjectWithTag ("Chest").transform.position;

		maxNumEnemies = 3;
		//enemiesKill = new GameObject[maxNumEnemies];

	}

	// Update is called once per frame
	void Update () {
		if (Time.time <= initialWaitDuration) {
			return;
		}

		if (isActive) {
			ShakeToKillEnemy ();
		}
		else if (!isCharged) {
			Charge();
		}
			
	}

	void Charge() {
		image.fillAmount += Time.deltaTime/chargingTime;

		if(image.fillAmount >= 1) {
			isCharged = true;
			button.interactable = true;
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

			
		button.interactable = false;
		isActive = true;

		enemiesKill = GameObject.FindGameObjectsWithTag ("Enemy");
	}
		

	void ShakeToKillEnemy() {
		remainingActiveTime -= Time.deltaTime/activeTime;
		if(remainingActiveTime <= 0) 
		{
			remainingActiveTime = 1.0f;
			isActive = false;
			isCharged = false;

			// based on shake counter value, destroy enemies
			Debug.Log("shakeCounter: " + shakeCounter);
			if(shakeCounter <= 5) {
				// kill one enemy
				Debug.Log("Kill 1 enemy");
				Destroy(enemiesKill[0]);
			}
			else if(shakeCounter > 5 && shakeCounter < 10) {
				//kill 2 enemies
				Debug.Log("Kill 2 enemies");
				Destroy(enemiesKill[0]);
				Destroy(enemiesKill[1]);
			}
			else if (shakeCounter >= 10) {
				// kill 3 enemies
				Debug.Log("Kill 3 enemies");
				Destroy(enemiesKill[0]);
				Destroy(enemiesKill[1]);
				Destroy(enemiesKill[2]);
			}

			// reset storage arrays for enemy game objects and distances

		}
		// low pass filter accelerometer values to decrease noise
		Acceleration = Input.acceleration;
		LowPassValue = Vector3.Lerp (LowPassValue, Acceleration, LowPassFilterFactor);
		FilteredAcceleration = Acceleration - LowPassValue;

		// shake input determined by the following condition
		if (FilteredAcceleration.sqrMagnitude >= shakeDetectionThreshold) {
			// increment shake counter 
			shakeCounter++;
		}
	}
}