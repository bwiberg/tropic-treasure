using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeCanvasOnStart : MonoBehaviour {
	public float waitDuration;
	public float fadeDuration;

	private CanvasRenderer[] renderers;
	private float[] initialAlphas;
	private int n;

	private bool hasCompletedFade;

	private void Start () {
		renderers = GetComponentsInChildren<CanvasRenderer>();
		initialAlphas = new float[renderers.Length];

		for (int i = 0; i < renderers.Length; ++i) {
			initialAlphas[i] = renderers[i].GetAlpha();
			renderers[i].SetAlpha(0.0f);
		}
	}

	private void Update() {
		if (hasCompletedFade) {
			this.enabled = false;
			return;
		}

		if (Time.timeSinceLevelLoad <= waitDuration) {
			return;
		}

		if (Time.timeSinceLevelLoad >= waitDuration + fadeDuration) {
			hasCompletedFade = true;
			setAlphas(1.0f);
			return;
		}

		float fraction = (Time.timeSinceLevelLoad - waitDuration) / fadeDuration;
		setAlphas(fraction);
	}

	private void setAlphas(float fraction) {
		for (int i = 0; i < renderers.Length; ++i) {
			renderers[i].SetAlpha(fraction * initialAlphas[i]);
		}
	}
}
