using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BossMovePattern", menuName = "Boss/MovePattern", order = 3)]
public class BossMovePattern : ScriptableObject
{
	public enum EPatternMove
	{

	}

	public EPatternMove[] MoveList;
}
