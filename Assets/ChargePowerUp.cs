using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargePowerUp : MonoBehaviour {

	public float chargingTime = 10.0f;

	private float angle;
	private bool isCharged;
	private Image image;
	private Button button;

	// Use this for initialization
	void Start () {
		angle = 0;
		isCharged = false;

		image = GetComponent<Image>();
		image.fillAmount = 0;

		button = GetComponent<Button>();
		button.interactable = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(!isCharged) {
			Charge();
		}
		else {
			Reset();
		}
	}

	void Charge() {
		image.fillAmount += Time.deltaTime/chargingTime;

		if(image.fillAmount >= 1) {
			isCharged = true;
			button.interactable = true;
		}
	}

	void Reset() {
		image.fillAmount = 0;
		isCharged = false;
		button.interactable = false;
	}
}
