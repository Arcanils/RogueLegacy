using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPawn : MonoBehaviour
{
	public GameObject PrefabSimpleBullet;
	public GameObject PrefabBounceBullet;
	public GameObject PrefabFragBullet;

	public Transform SpawnAttack;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.T))
			BarrageAttack();

		if (Input.GetKeyDown(KeyCode.Y))
			ShotgunAttack();
	}

	public void BarrageAttack()
	{
		StartCoroutine(BarrageAttackEnum(PrefabSimpleBullet, Vector2.down, 10f, true, 0.1f, 100));
	}

	public void ShotgunAttack()
	{
		StartCoroutine(ShotgunAttackEnum(PrefabSimpleBullet, Vector2.down, 30f, 0.5f, 6, 4));
	}


	
	private IEnumerator BarrageAttackEnum(GameObject bullet, Vector2 baseDirection, float angleOffset, bool sensHoraire, float fireRate, int nBullet)
	{
		var dirEuler = new Vector3(0f, 0f, Mathf.Atan2(baseDirection.y, baseDirection.x) -90f);
		if (sensHoraire)
			angleOffset *= -1;
		var wait = new WaitForSeconds(fireRate);

		for (int i = nBullet - 1; i >= 0; --i)
		{
			dirEuler.z += angleOffset;
			var orientation = Quaternion.Euler(dirEuler);
			   SpawnBullet(bullet, orientation);

			yield return wait;
		}
	}
	private IEnumerator ShotgunAttackEnum(GameObject bullet, Vector2 baseDirection, float range, float fireRate, int nBullet, int nShotgunFire)
	{
		var begAngle = Mathf.Atan2(baseDirection.y, baseDirection.x) - range / 2f - 90f;
		var wait = new WaitForSeconds(fireRate);

		for (int i = nShotgunFire - 1; i >= 0; --i)
		{
			for (int j = nBullet - 1; j >= 0; --j)
			{
				var perc = Mathf.Clamp01(j / (float)(nBullet - 1));
				var orientation = Quaternion.Euler(0f, 0f, begAngle + range * perc);
				SpawnBullet(bullet, orientation);
			}

			yield return wait;
		}
	}
	private void SpawnBullet(GameObject bullet, Quaternion orientation)
	{
		var instance = GameObject.Instantiate(bullet, SpawnAttack.position, orientation);
	}
}
