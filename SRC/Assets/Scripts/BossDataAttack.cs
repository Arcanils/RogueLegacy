using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BossDataAttack", menuName = "Boss/DataAttack", order = 5)]
public class BossDataAttack : ScriptableObject
{
	public enum EAttack
	{
		SHOTGUN,
		BARRAGE,
		SPAWN_MINION,
	}

	public enum ETypeBullet
	{
		SIMPLE,
		BOUNCE,
		FRAG,
	}

	[System.Flags]
	public enum ETarget
	{
		BASE_DIRECTION = 0x01,
		PLAYER = 0x02,
	}

	[System.Flags]
	public enum EDuration
	{
		OUT_OF_AMMO = 0x01,
		DURATION = 0x02,
	}
	
	public EAttack TypeAttack;
	public ETypeBullet TypeBullet;
	[HideInInspector]
	public ETarget TypeTargeting;
	[HideInInspector]
	public EDuration TypeDuration;

	public float Duration;
	public float FireRate;
	public float RangeGlobalAttack;
	public float RangeShot;
	public int NBulletEachRound;
	public int NRound;

	public Vector2 BaseDirection;
}