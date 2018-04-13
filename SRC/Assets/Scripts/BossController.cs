using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour {

	public BossPattern[] Patterns;
	public float TimeBeetweenPatterns;

	private PawnComponent _pawnPlayer;

	public void Init(PawnComponent pawnPlayer)
	{
		_pawnPlayer = pawnPlayer;
		StartCoroutine(IAEnum());
	}

	public IEnumerator IAEnum()
	{
		//Spawn
		var indexPattern = -1;
		while(true)
		{
			indexPattern = (indexPattern + 1) % Patterns.Length;
			//PlayPattern;

			//PlayIdle;
			yield break;
		}
	}

	public IEnumerator PlayPattern(BossPattern pattern)
	{
		yield break;
	}

	private IEnumerator PlayIdle()
	{
		//Reposition
		//WatchPlayer
		yield break;
	}

	private IEnumerator PlayDeath()
	{
		yield break;
	}
}
