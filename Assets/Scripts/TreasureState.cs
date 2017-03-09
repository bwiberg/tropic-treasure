using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureState : MonoBehaviour {

	public GameObject coinsObject;
	public int initialCoins = 50;

	private int currentCoins;

	private float maxHeight = -0.009f;
	private float minHeight = -0.27f;
	private float currentHeight;

	public bool isEmpty = false;

	// Use this for initialization
	void Start () {
		currentHeight = maxHeight;
		currentCoins = initialCoins;
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.GetComponent<SimpleAgent>())
		{
			GameObject.Destroy(other.gameObject);
			UpdateCoins(1);
		}
	}

	void UpdateCoins(int coinUpdate) {
		if(!isEmpty) {
			currentCoins -= coinUpdate;
			float newHeight = maxHeight - (maxHeight-minHeight) * ((float) (initialCoins - currentCoins) / (float) initialCoins);
			UpdateHeight(newHeight);

			if(currentCoins == 0)
				isEmpty = true;
		}
	}

	void UpdateHeight(float newHeight) {
		coinsObject.transform.localPosition = new Vector3(0, 0, newHeight);
	}
}
