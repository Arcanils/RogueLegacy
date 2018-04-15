using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour {

	public Vector3 BotLeftLimit;
	public Vector3 TopRightLimit;

	public int NCollumn;
	public int NLine;
	public float DurationMove;

	public PatternHandler[] Patterns;
	public float TimeBeetweenPatterns;

	public Action OnFinishPattern;

	private PawnComponent _pawnPlayer;
	private BossPawn _pawn;
	private Vector3[,] _gridPos;

	private int _xGridPosition;
	private int _yGridPosition;
	private Transform _trans;
	private int _indexPattern;

	private void Awake()
	{
		_trans = transform;
		_pawn = GetComponent<BossPawn>();
	}

	public int GetCurrentIndexPattern()
	{
		return _indexPattern + 1;
	}

	public void Init(PawnComponent pawnPlayer, PatternHandler.EDifficulty difficulty)
	{
		_pawnPlayer = pawnPlayer;
		_gridPos = new Vector3[NLine, NCollumn];
		for (int i = 0; i < NLine; ++i)
		{
			var percY = Mathf.Lerp(BotLeftLimit.y, TopRightLimit.y, (i / (float)(NLine - 1)));
			for (int j = 0; j < NCollumn; ++j)
			{
				_gridPos[i, j] = new Vector3(Mathf.Lerp(BotLeftLimit.x, TopRightLimit.x, (j / (float)(NCollumn - 1))) , percY, 0f);
			}
		}

		_xGridPosition = (NCollumn / 2);
		_yGridPosition = (NLine / 2);

		StartCoroutine(IAEnum(difficulty));
	}

	public IEnumerator IAEnum(PatternHandler.EDifficulty difficulty)
	{
		//Spawn
		_indexPattern = -1;
		while(_indexPattern < Patterns.Length - 1)
		{
			_indexPattern = (_indexPattern + 1) % Patterns.Length;
			yield return PlayPattern(Patterns[_indexPattern].GetPattern(difficulty));

			yield return PlayIdle();
		}

		if (OnFinishPattern != null)
			OnFinishPattern();
	}

	public IEnumerator PlayPattern(BossPattern pattern)
	{
		for (int n = 0, nLength = pattern.SubPattern.Length; n < nLength; n++)
		{
			var subPattern = pattern.SubPattern[n];
			var attacksArray = subPattern.Attacks;

			var routineMove = StartCoroutine(Move(subPattern.Move));

			for (int j = 0, jLength = attacksArray.Length; j < jLength; j++)
			{
				var attacksSameTime = attacksArray[j].Attaks;
				var _routinesToWait = new Coroutine[attacksSameTime.Length];
				for (int i = 0, iLength = _routinesToWait.Length; i < iLength; i++)
				{
					_routinesToWait[i] = StartCoroutine(_pawn.Attack(attacksSameTime[i], _pawnPlayer.transform));
				}
				
				for (int i = 0, iLength = _routinesToWait.Length; i < iLength; i++)
				{
					yield return _routinesToWait[i];
				}
			}
			yield return routineMove;
		}
	}

	private IEnumerator Move(BossMovePattern movePattern)
	{
		int xIndex;
		int yIndex;
		var moves = movePattern.Moves;
		for (int i = 0; i < moves.Length; i++)
		{
			moves[i].GetMove(out xIndex, out yIndex);
			xIndex = Mathf.Clamp(xIndex + _xGridPosition, 0, NCollumn - 1);
			yIndex = Mathf.Clamp(yIndex + _yGridPosition, 0, NLine - 1);

			var beg = _trans.position;
			var end = _gridPos[yIndex, xIndex];
			
			var duration = DurationMove * moves[i].Amount;

			if (xIndex == _xGridPosition && yIndex == _yGridPosition)
			{
				yield return new WaitForSeconds(duration);
				continue;
			}

			for (float t = 0f, perc = 0f; perc < 1f; t+= Time.deltaTime)
			{
				perc = Mathf.Clamp01(t / duration);
				_trans.position = Vector3.Lerp(beg, end, perc);

				yield return null;
			}

			_xGridPosition = xIndex;
			_yGridPosition = yIndex;
		}
		yield break;
	}

	private IEnumerator PlayIdle()
	{
		var beg = _trans.position;
		var end = Vector3.zero;

		var lengthToMove = Mathf.Max(Mathf.Abs(_xGridPosition - NCollumn / 2), Mathf.Abs(_yGridPosition - NLine / 2));

		if (lengthToMove != 0)
		{
			var duration = lengthToMove * DurationMove;

			for (float t = 0f, perc = 0f; perc < 1f; t += Time.deltaTime)
			{
				perc = Mathf.Clamp01(t / duration);
				_trans.position = Vector3.Lerp(beg, end, perc);

				yield return null;
			}
		}
		_xGridPosition = NCollumn / 2;
		_yGridPosition = NLine / 2;
		yield return new WaitForSeconds(TimeBeetweenPatterns);
	}

	private IEnumerator PlayDeath()
	{
		yield break;
	}
}
