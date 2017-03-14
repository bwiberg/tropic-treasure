using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EazyTools.SoundManager;

public class Cannonball : MonoBehaviour {
	public PirateShip firingShip;

	private Audio whistlingAudio;

	public void OnFired() {
		var clip = AudioClips.Instance.Cannonball.Whistling.GetAny();
		whistlingAudio = SoundManager.GetAudio(SoundManager.PlaySound(clip));
	}

	private void OnCollisionEnter(Collision col) {
		if (whistlingAudio != null) {
			whistlingAudio.fadeOutSeconds = 0.1f;
			whistlingAudio.Stop();
		}

		if (col.gameObject.CompareTag(Tags.CircularSegment)) {
			col.gameObject.GetComponent<CircularSegment>().handleCannonballHit();
			GameObject.Destroy(gameObject);

			firingShip.FireState = PirateShip.CannonFireState.TargetDestroyed;
		}

		else if (col.gameObject.CompareTag(Tags.Enemy)) {
			col.gameObject.GetComponent<SimpleAgent>().handleHitByCannonball(this);
		}
	}
}
