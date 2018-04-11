using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim2DComponent : MonoBehaviour
{
	public string StartAnim;
	public StringAnim2DData[] AnimDatas;
	public SpriteRenderer Target;

	private Dictionary<string, Anim2DData> _dicoAnim;
	private IEnumerator _currentAnim;
	private string _currentKeyAnim;
	private System.Action<string> _onFinishAnim;

	private void Awake()
	{
		_dicoAnim = new Dictionary<string, Anim2DData>();
		for (int i = AnimDatas.Length - 1; i >= 0; --i)
		{
			if (string.IsNullOrEmpty(AnimDatas[i].Key))
				return;
			_dicoAnim[AnimDatas[i].Key] = AnimDatas[i].Value;
		}

		AnimDatas = null;

		if (Target == null)
			Target = GetComponent<SpriteRenderer>();
	}

	private void Start()
	{
		PlayAnim(StartAnim);
	}

	private void Update()
	{
		if (_currentAnim != null && !_currentAnim.MoveNext())
		{
			_currentAnim = null;
			if (_onFinishAnim != null)
				_onFinishAnim(_currentKeyAnim);

		}
	}

	public void PlayAnim(string strAnim, System.Action<string> callbackOnFinish = null)
	{
		if (Target == null || string.IsNullOrEmpty(strAnim) || strAnim == _currentKeyAnim || !_dicoAnim.ContainsKey(strAnim))
			return;
		_currentKeyAnim = strAnim;
		_onFinishAnim = callbackOnFinish;
		_currentAnim = _dicoAnim[strAnim].PlayEnum(Target);
	}
}

[System.Serializable]
public class StringAnim2DData
{
	public string Key;
	public Anim2DData Value;
	
	public StringAnim2DData(string key, Anim2DData value)
	{
		Key = key;
		Value = value;
	}
}

[System.Serializable]
public class Anim2DData
{
	public Sprite[] Frames;

	[Range(0.05f, 100f)]
	public float Duration;
	public bool Loop;

	public IEnumerator PlayEnum(SpriteRenderer target)
	{
		var length = Frames.Length;
		var durationByFrame = Duration / length;
		var t = 0f;
		var perc = 0f;
		do
		{
			var index = -1;
			while (++index < length)
			{
				target.sprite = Frames[index];

				t = 0f; perc = 0f;
				while (perc < 1f)
				{
					t += Time.deltaTime;
					perc = Mathf.Clamp01(t / durationByFrame);

					yield return null;
				}
			}
		} while (Loop);
	}
}
