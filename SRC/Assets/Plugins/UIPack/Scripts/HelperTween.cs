using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class HelperTween
{
	public static IEnumerator ScaleEnum(Transform target, Vector3 beg, Vector3 end, float duration, AnimationCurve curve, bool useUnscaledDeltaTime = true, Action callback = null)
	{
		return IterateEnum((value) => target.localScale = value, Vector3.Lerp, beg, end, duration, curve, useUnscaledDeltaTime, callback);
	}

	public static IEnumerator MoveRigid2DEnum(Rigidbody2D target, Vector3 beg, Vector3 end, float duration, AnimationCurve curve, bool useUnscaledDeltaTime = true, Action callback = null)
	{
		return IterateEnum((value) => target.MovePosition(value), Vector3.Lerp, beg, end, duration, curve, useUnscaledDeltaTime, callback);
	}
	public static IEnumerator MoveTransformEnum(Transform target, Vector3 beg, Vector3 end, float duration, AnimationCurve curve, bool useUnscaledDeltaTime = true, Action callback = null)
	{
		return IterateEnum((value) => target.position = value, Vector3.Lerp, beg, end, duration, curve, useUnscaledDeltaTime, callback);
	}

	public static IEnumerator MoveRectTransformEnum(RectTransform target, Vector2 beg, Vector2 end, float duration, AnimationCurve curve, bool useUnscaledDeltaTime = true, Action callback = null)
	{
		return IterateEnum((value) => target.anchoredPosition = value, Vector2.Lerp, beg, end, duration, curve, useUnscaledDeltaTime, callback);
	}

	public static IEnumerator TimeScaleEnum(float beg, float end, float duration, AnimationCurve curve, bool useUnscaledDeltaTime = true, Action callback = null)
	{
		return IterateEnum((value) => Time.timeScale = value, Mathf.Lerp, beg, end, duration, curve, useUnscaledDeltaTime, callback);
	}

	private static IEnumerator IterateEnum<T>(Action<T> setValue, Func<T, T, float, T> lerpFunc, T beg, T end, float duration, AnimationCurve curve, bool useUnscaledDeltaTime, Action callback)
	{
		for (float t = 0f, perc = 0f; perc < 1f; t += useUnscaledDeltaTime ? Time.unscaledDeltaTime : Time.deltaTime)
		{
			perc = Mathf.Clamp01(t / duration);
			setValue(lerpFunc(beg, end, curve.Evaluate(perc)));
			yield return null;
		}

		if (callback != null)
			callback();
	}
}