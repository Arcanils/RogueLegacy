using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEndScreen : MonoBehaviour {


	public RectTransform UIContainerGameplay;
	public RectTransform UIContainerEndPanel;

	public float DurationLetters = 2f;

	public Text ScoreTxt;

	public RectTransform ButtonsEndPanel;

	public void Awake()
	{
		UIContainerEndPanel.gameObject.SetActive(false);
		UIContainerEndPanel.anchoredPosition = new Vector2(-UIContainerEndPanel.rect.width, 0f);
		ScoreTxt.text = "";
		ButtonsEndPanel.anchoredPosition = ButtonsEndPanel.anchoredPosition - new Vector2(0f, UIContainerEndPanel.rect.height);
		ButtonsEndPanel.GetComponent<CanvasGroup>().interactable = false;
	}

	public void PlayEnd(string currentTime, string bestTime)
	{
		StartCoroutine(AnimEndPanel(currentTime, bestTime));
	}

	public IEnumerator AnimEndPanel(string newScoreTxt, string bestScoreTxt)
	{
		if (UIContainerGameplay != null)
		{
			UIContainerGameplay.GetComponent<CanvasGroup>().interactable = false;

			var routineMoveGameplayUI = HelperTween.MoveRectTransformEnum(
				UIContainerGameplay,
				UIContainerGameplay.anchoredPosition,
				UIContainerGameplay.anchoredPosition +
				Vector2.up * UIContainerGameplay.rect.height,
				1f,
				AnimationCurve.EaseInOut(0f, 0f, 1f, 1f),
				true);

			yield return StartCoroutine(routineMoveGameplayUI);
		}
		UIContainerEndPanel.gameObject.SetActive(true);

		var routine = HelperTween.MoveRectTransformEnum(
			UIContainerEndPanel,
			UIContainerEndPanel.anchoredPosition,
			Vector2.zero,
			1f,
			AnimationCurve.EaseInOut(0f, 0f, 1f, 1f),
			true);

		yield return StartCoroutine(routine);
		

		var str = newScoreTxt + "\n" + bestScoreTxt;
		var length = str.Length;
		for (float t = 0f, perc = 0f; perc < 1f; t += Time.unscaledDeltaTime)
		{
			perc = Mathf.Clamp01(t / DurationLetters);
			var nLetterToShow = (int)(length * perc);
			ScoreTxt.text = str.Substring(0, nLetterToShow);

			yield return null;
		}

		routine = HelperTween.MoveRectTransformEnum(
			ButtonsEndPanel,
			ButtonsEndPanel.anchoredPosition,
			new Vector2(0f, 20f),
			1f,
			AnimationCurve.EaseInOut(0f, 0f, 1f, 1f),
			true);

		yield return StartCoroutine(routine);

		ButtonsEndPanel.GetComponent<CanvasGroup>().interactable = true;
	}

	public void Retry()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(1);
	}

	public void Quit()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
	}
}
