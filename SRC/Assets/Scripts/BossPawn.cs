using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossPawn : MonoBehaviour
{
	public GameObject[] PrefabBullet;



	public Transform SpawnAttack;


	public IEnumerator Attack(BossDataAttack data, Transform target = null)
	{
		var bullet = PrefabBullet[(int)data.TypeBullet];

		float begAngle = 0f;
		Transform targetObj = null;

		var isBaseDirMode = (data.TypeTargeting & BossDataAttack.ETarget.BASE_DIRECTION) == BossDataAttack.ETarget.BASE_DIRECTION;
		var isPlayerMode = (data.TypeTargeting & BossDataAttack.ETarget.PLAYER) == BossDataAttack.ETarget.PLAYER;
		
		if (isBaseDirMode)
		{
			begAngle = GetAngleFromDirection(data.BaseDirection);
		}
		if (isPlayerMode)
		{
			targetObj = target;
		}

		var spawnPos = SpawnAttack;
		var fireRate = data.FireRate;
		var range = data.RangeShot;


		float duration = 0f;
		int nTick = 0;

		var isDurationMode = (data.TypeDuration & BossDataAttack.EDuration.DURATION) == BossDataAttack.EDuration.DURATION;
		var isOutOfAmmoMode = (data.TypeDuration & BossDataAttack.EDuration.OUT_OF_AMMO) == BossDataAttack.EDuration.OUT_OF_AMMO;

		if (isDurationMode && isOutOfAmmoMode)
		{
			duration = data.Duration;
			nTick = data.NRound;
		}
		else if (isDurationMode)
		{
			duration = data.Duration;
			nTick = -1;
		}
		else if (isOutOfAmmoMode)
		{
			duration = -1f;
			nTick = data.NRound;
		}

		var nBullet = data.NBulletEachRound;

		IEnumerator valueToReturn = null;
		
		switch (data.TypeAttack)
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
				valueToReturn = SpawnBulletEachRoundAndTurn(
					bullet,
					targetObj,
					spawnPos,
					begAngle,
					data.RangeGlobalAttack,
					range,
					fireRate,
					duration,
					nTick,
					nBullet);
				break;
			case BossDataAttack.EAttack.SPAWN_MINION:
				break;
		}

		return valueToReturn;
	}

	private static IEnumerator SpawnBulletEachRoundAndTurn(
		GameObject bullet,
		Transform target,
		Transform spawnPos,
		float baseAngle, float rangeGlobalAttack, float rangeShot, float fireRate, float duration,
		int nRound, int nInstance = 1)
	{
		var wait = new WaitForSeconds(fireRate);

		var angleToTarget = (target != null ? GetAngleFromDirection(target.position - spawnPos.position) : 0f);
		baseAngle += angleToTarget -rangeShot / 2f;
		var endBaseAngle = baseAngle + rangeGlobalAttack;

		int nTurnForDuration = duration <= 0f ? int.MaxValue : ((int)(duration * 1000f) % (int)(fireRate * 1000f)) / 1000;

		for (int i = Mathf.Min(nRound, nTurnForDuration) - 1; i >= 0; --i)
		{
			var begAngle = Mathf.Lerp(baseAngle, endBaseAngle, i / (float)(nRound - 1));
			var endAngle = begAngle + rangeShot;
			SpawnBullet(bullet, spawnPos.position, begAngle, endAngle, nInstance);

			yield return wait;
		}

		var valueToWait = duration - nTurnForDuration * fireRate;

		if (valueToWait > 0f)
			yield return new WaitForSeconds(valueToWait);
	}

	private static IEnumerator SpawnBulletEachRound(
		GameObject bullet,
		Transform target,
		Transform spawnPos,
		float baseAngle, float range, float fireRate, float duration,
		int nRound, int nInstance = 1)
	{
		var wait = new WaitForSeconds(fireRate);
		
		int nTurnForDuration = duration <= 0f ? int.MaxValue : ((int)(duration * 1000f) % (int)(fireRate * 1000f)) / 1000;
		
		for (int i = Mathf.Min(nRound, nTurnForDuration) - 1; i >= 0; --i)
		{
			var angleToTarget = (target != null ? GetAngleFromDirection(target.position - spawnPos.position) : 0f);
			var begAngle = baseAngle + angleToTarget - range / 2f;
			var endAngle = begAngle + range;
			SpawnBullet(bullet, spawnPos.position, begAngle, endAngle, nInstance);
			
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
			var perc = nInstance <= 1 ? 0.5f : Mathf.Clamp01(j / (float)(nInstance - 1));
			var orientation = Quaternion.Euler(0f, 0f, Mathf.Lerp(begAngle, endAngle, perc));
			var instance = Object.Instantiate(bullet, position, orientation);
		}
	}

	private static float GetAngleFromDirection(Vector2 dir)
	{
		dir.Normalize();

		return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
	}
}
