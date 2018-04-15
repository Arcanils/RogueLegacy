using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PatternHandler", menuName = "Boss/FinalPattern")]
public class PatternHandler : ScriptableObject
{
	public enum EDifficulty
	{
		EASY,
		MEDIUM,
		HARD,
	}
	public BossPattern[] Easy;
	public BossPattern[] Medium;
	public BossPattern[] Hard;


	public BossPattern GetPattern(EDifficulty difficulty = EDifficulty.EASY)
	{
		switch (difficulty)
		{
			case EDifficulty.EASY:
				return RandomItem(ref Easy);
			case EDifficulty.MEDIUM:
				return RandomItem(ref Medium);
			case EDifficulty.HARD:
				return RandomItem(ref Hard);
		}

		return null;
	}


	private static T RandomItem<T>(ref T[] array) where T : class
	{
		if (array == null)
			return null;

		return array[UnityEngine.Random.Range(0, array.Length)];
	}
}
