using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossSubPattern", menuName = "Boss/SubPattern", order = 1)]
public class BossSubPattern : ScriptableObject
{
	public BossMovePattern Move;
	public BossAttackPattern[] Attacks;
}
