using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameTitleFadeIn : MonoBehaviour {

	public float waitDuration;
	public float fadeDuration;

	private Image title;

	void Start () {
		title = GetComponent<Image>();

		var color = title.color;
		color.a = 0.0f;
		title.color = color;

		title.DOFade(1.0f, fadeDuration).SetEase(Ease.InOutQuad);
	}
}
