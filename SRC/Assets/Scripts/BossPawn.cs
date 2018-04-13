using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossPawn : MonoBehaviour
{
	public GameObject[] PrefabBullet;



	public Transform SpawnAttack;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.T))
			BarrageAttack();

		if (Input.GetKeyDown(KeyCode.Y))
			ShotgunAttack();
	}

	public IEnumerator Attack(BossDataAttack data, Transform target = null)
	{
		var bullet = PrefabBullet[(int)data.TypeBullet];
		var begAngle = GetAngleFromDirection(data.BaseDirection);
		var targetObj = (data.Target & BossDataAttack.ETarget.PLAYER) == BossDataAttack.ETarget.PLAYER ? target : null;
		var spawnPos = SpawnAttack.position;
		var fireRate = data.FireRate;
		var range = data.OffsetAngle;
		var duration = data.Duration;
		var nTick = data.NRound;
		var nBullet = data.NBullet;

		IEnumerator valueToReturn = null;
		switch (data.Attack)
		{
			case BossDataAttack.EAttack.SHOTGUN:
				valueToReturn = SpawnBulletEachRound(
					bullet,
					targetObj,
					spawnPos,
					begAngle,
					range,
					fireRate,
					duration,
					nTick,
					nBullet);
				break;
			case BossDataAttack.EAttack.BARRAGE:
				break;
			case BossDataAttack.EAttack.SPAWN_MINION:
				break;
		}

		return valueToReturn;
	}

	public void BarrageAttack()
	{
		StartCoroutine(BarrageAttackEnum(PrefabBullet[0], Vector2.down, 10f, true, 0.1f, 100));
	}

	public void ShotgunAttack()
	{
		StartCoroutine(SpawnBulletEachRound(
			PrefabBullet[0], null, SpawnAttack.position,
			GetAngleFromDirection(Vector2.down),
			30f, 0.5f,
			0f, 4, 6));
	}

	public void SpawnMinion()
	{

	}

	public void Spawn()
	{

	}

	
	private IEnumerator BarrageAttackEnum(GameObject bullet, Vector2 baseDirection, 
		float angleOffset, bool sensHoraire, float fireRate, int nBullet)
	{
		/*
		var dirEuler = 
		if (sensHoraire)
			angleOffset *= -1;
		var wait = new WaitForSeconds(fireRate);

		for (int i = nBullet - 1; i >= 0; --i)
		{
			var orientation = Quaternion.Euler(dirEuler);
			   SpawnBullet(bullet, orientation);
			
			dirEuler.z += angleOffset;
			yield return wait;
		}
		*/
		yield break;
	}

	private static IEnumerator SpawnBulletEachRound(
		GameObject bullet,
		Transform target,
		Vector3 spawnPos,
		float baseAngle, float range, float fireRate, float duration,
		int nRound, int nInstance = 1)
	{
		var wait = new WaitForSeconds(fireRate);

		for (int i = nRound - 1; i >= 0; --i)
		{
			var angleToTarget = (target != null ? GetAngleFromDirection(target.position - spawnPos) : 0f);
			var begAngle = baseAngle + angleToTarget - range / 2f;
			var endAngle = begAngle + range;
			SpawnBullet(bullet, spawnPos, begAngle, endAngle, nInstance);

			yield return wait;
		}

		var valueToWait = duration - nRound * fireRate;

		if (valueToWait > 0f)
			yield return new WaitForSeconds(valueToWait);
	}

	private static void SpawnBullet(GameObject bullet, Vector3 position, float begAngle, float endAngle, int nInstance = 1)
	{
		for (int j = nInstance - 1; j >= 0; --j)
		{
			var perc = Mathf.Clamp01(j / (float)(nInstance - 1));
			var orientation = Quaternion.Euler(0f, 0f, Mathf.Lerp(begAngle, endAngle, perc));
			var instance = Object.Instantiate(bullet, position, orientation);
		}
	}

	private static float GetAngleFromDirection(Vector2 dir)
	{
		dir.Normalize();

		return Mathf.Atan2(dir.y, dir.x) - 90f;
	}
}
