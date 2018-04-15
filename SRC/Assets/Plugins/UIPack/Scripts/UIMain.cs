using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMain : MonoBehaviour
{
	public RectTransform StartScreen;
	public RectTransform FirstButtonList;
	public RectTransform SecondButtonList;

	public static int GlobalParamSet;

	public void Awake()
	{
		SecondButtonList.gameObject.SetActive(false);
	}

	public void GoSettingPlay()
	{
		FirstButtonList.GetComponent<CanvasGroup>().interactable = false;

		SecondButtonList.GetComponent<CanvasGroup>().interactable = false;
		SecondButtonList.anchoredPosition += Vector2.right * Screen.width;
		SecondButtonList.gameObject.SetActive(true);

		var routineButtonListFollow = HelperTween.MoveRectTransformEnum(
			SecondButtonList,
			SecondButtonList.anchoredPosition,
			Vector2.zero,
			1f,
			AnimationCurve.EaseInOut(0f, 0f, 1f, 1f),
			true,
			() => SecondButtonList.GetComponent<CanvasGroup>().interactable = true);

		var routine = HelperTween.MoveRectTransformEnum(
			FirstButtonList,
			FirstButtonList.anchoredPosition,
			FirstButtonList.anchoredPosition +
			Vector2.left * Screen.width,
			1f,
			AnimationCurve.EaseInOut(0f, 0f, 1f, 1f),
			true,
			() => StartCoroutine(routineButtonListFollow));

		StartCoroutine(routine);
	}

	public void Play(int index)
	{
		StartScreen.GetComponent<CanvasGroup>().interactable = false;

		var routine = HelperTween.MoveRectTransformEnum(
			StartScreen,
			StartScreen.anchoredPosition,
			StartScreen.anchoredPosition +
			Vector2.up * StartScreen.rect.height,
			1f,
			AnimationCurve.EaseInOut(0f, 0f, 1f, 1f),
			true,
			() => SceneManager.LoadScene(1));

		StartCoroutine(routine);
		GlobalParamSet = index;
	}

	public void Quit()
	{
		StartScreen.GetComponent<CanvasGroup>().interactable = false;
		Debug.Log(Vector2.down * StartScreen.rect.height);
		var routine = HelperTween.MoveRectTransformEnum(
			StartScreen,
			StartScreen.anchoredPosition,
			StartScreen.anchoredPosition +
			Vector2.down * StartScreen.rect.height,
			1f,
			AnimationCurve.EaseInOut(0f, 0f, 1f, 1f),
			true,
			() => Application.Quit());
		StartCoroutine(routine);
	}
}
