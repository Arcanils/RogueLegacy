using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BossAttackPattern", menuName = "Boss/AttackPattern", order = 4)]
public class BossAttackPattern : ScriptableObject
{
	public BossDataAttack[] Attaks;
}