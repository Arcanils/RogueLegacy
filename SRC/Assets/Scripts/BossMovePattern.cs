using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BossMovePattern", menuName = "Boss/MovePattern", order = 3)]
public class BossMovePattern : ScriptableObject
{
	public DataMovePattern[] Moves;
}

[System.Serializable]
public struct DataMovePattern
{
	[System.Flags]
	public enum EPatternMove
	{
		LEFT = 0x01,
		RIGHT = 0x02,
		TOP = 0x04,
		BOTTOM = 0x08,
	}
	public EPatternMove Move;

	public int Amount;

	public void GetMove(out int xIndex, out int yIndex)
	{
		xIndex = 0;
		yIndex = 0;
		if ((Move & EPatternMove.LEFT) != 0)
			--xIndex;
		else if ((Move & EPatternMove.RIGHT) != 0)
			++xIndex;

		if ((Move & EPatternMove.BOTTOM) != 0)
			--yIndex;
		else if ((Move & EPatternMove.TOP) != 0)
			++yIndex;

		xIndex *= Amount;
		yIndex *= Amount;
	}
}
