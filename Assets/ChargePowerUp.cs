using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargePowerUp : MonoBehaviour {

	public float chargingTime = 10.0f;
	public float activeTime = 5.0f;

	public bool isActive;

	private bool isCharged;

	private float remainingActiveTime;
	private Image image;
	private Button button;

	// Use this for initialization
	void Start () {
		image = GetComponent<Image>();
		button = GetComponent<Button>();
		isCharged = true;
		isActive = false;
		remainingActiveTime = 1.0f;
		image.fillAmount = 1;
		button.interactable = true;
	}

	// Update is called once per frame
	void Update () {
		if(isActive) {
			Deplete();
		}
		else if(!isCharged) {
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

	void Deplete() {
		remainingActiveTime -= Time.deltaTime/activeTime;
		if(remainingActiveTime <= 0) 
		{
			remainingActiveTime = 1.0f;
			isActive = false;
		}
	}

	public void Activate() {
		Reset();
		isActive = true;
	}

	void Reset() {
		image.fillAmount = 0;
		isCharged = false;
		button.interactable = false;
		button.gameObject.SetActive(false);
		button.gameObject.SetActive(true);
	}
}
