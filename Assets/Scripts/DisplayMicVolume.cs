using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayMicVolume : MonoBehaviour {

	private Text volumeText;
	// Use this for initialization
	void Start () {
		volumeText = GetComponent<Text>();
		volumeText.text = "" + Mathf.FloorToInt(0);
	}
	
	// Update is called once per frame
	void Update () {
		volumeText.text = "" + Mathf.FloorToInt(MicInput.loudness * 1000);
	}
}
