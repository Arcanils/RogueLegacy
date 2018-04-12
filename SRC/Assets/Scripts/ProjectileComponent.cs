using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileComponent : MonoBehaviour {

	public float Speed;

	private Transform _trans;
	private Vector2 _dir;

	private void Reset()
	{
		Speed = 10f;
	}
	private void Awake()
	{
		_trans = transform;
		_dir = Vector2.right;
	}

	private void Update()
	{
		_trans.Translate(_dir * Speed * Time.deltaTime);
	}


	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.transform.tag == "Player")
		{
			Debug.Log("HitPlayer !");
			//Inflict Dmg ou autre;
		}
		DestroyBullet();
	}

	public void InitParam(Vector2 dir)
	{
		_dir = dir;
	}

	public void DestroyBullet()
	{
		Destroy(gameObject);
	}

}
