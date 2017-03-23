using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.AI;

public class EarthPowerUp : MonoBehaviour {
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
	private Rigidbody enemyRigidBody;
	private float[] enemyDistanceFromChest;
	private int maxNumEnemies;
	private GameObject Cam;
	private Quaternion StaticCameraRotation;

	private Animator enemyAnim;
	private SimpleAgent enemyAgent;





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
		Cam = GameObject.FindGameObjectWithTag ("MainCamera");
		StaticCameraRotation = Cam.transform.rotation;


		maxNumEnemies = 3;
		//enemiesKill = new GameObject[maxNumEnemies];

	}

	// Update is called once per frame
	void Update () {
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
		Cam.transform.DOPunchRotation (new Vector3 (0, 1, 1), activeTime*2, 15, 1);
			
		button.interactable = false;
		isActive = true;

		enemiesKill = GameObject.FindGameObjectsWithTag ("Enemy");
		for (int i = 0; i < enemiesKill.Length; i++) {
			// stop enemies from moving
			enemiesKill[i].GetComponent<NavMeshAgent> ().Stop();

			//switch to quake animation
			enemyAnim = enemiesKill[i].GetComponentInChildren<Animator>();
			enemyAnim.SetBool ("powerActivated", true);
		}

	}
		

	void ShakeToKillEnemy() {
		remainingActiveTime -= Time.deltaTime/activeTime;
		if(remainingActiveTime <= 0) 
		{
			remainingActiveTime = 1.0f;
			isActive = false;
			isCharged = false;

			// switch to walking animation and turn on nav mesh
			for (int i = 0; i < enemiesKill.Length; i++) {

				//switch to walk animation
				enemyAnim = enemiesKill[i].GetComponentInChildren<Animator>();
				enemyAnim.SetBool ("powerActivated", false);

				// turn on nav mesh
				enemiesKill[i].GetComponent<NavMeshAgent> ().Resume();
			}

			// kill 2 enemies regardless
			Destroy(enemiesKill[0]);
			Destroy(enemiesKill[1]);

 


			shakeCounter = 0;

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