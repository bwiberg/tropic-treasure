using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EazyTools.SoundManager;

public class TempleTarget : MonoBehaviour {
	void OnTriggerEnter(Collider other) {
		if(other.gameObject.GetComponent<SimpleAgent>()) {
			GameObject.Destroy(other.gameObject);
			var clip = AudioClips.Instance.Pirates.GrabCoins.GetAny();
			SoundManager.PlaySound(clip);
		}
	}
}
