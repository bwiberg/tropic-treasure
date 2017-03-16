using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeFromWhite : MonoBehaviour {
	public float waitDuration;
	public float fadeDuration;

	public Canvas canvas;

	private Image image;

	private void Start () {
		image = GetComponent<Image>();
		image.enabled = true;

		DOTween.Sequence()
			.Append(image.DOFade(0.0f, fadeDuration).SetEase(Ease.InCubic))
			.AppendCallback(() => {
				canvas.gameObject.SetActive(false);
			})
			.Play();
	}
}
