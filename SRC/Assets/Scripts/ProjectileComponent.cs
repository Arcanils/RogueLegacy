using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileComponent : MonoBehaviour {

	public float Speed;
	public bool Bounce;
	public int NBounce = 2;
	private Transform _trans;
	private Vector2 _dir;
	private int _bounceLeft;

	private void Reset()
	{
		Speed = 10f;
	}
	private void Awake()
	{
		_trans = transform;
		_trans.localScale = Vector3.one * 0.05f;
		_dir = Vector2.right;
		_bounceLeft = Bounce ? NBounce : 0;
	}

	public void Start()
	{
		StartCoroutine(ScaleEnum());
	}

	public IEnumerator ScaleEnum()
	{
		var beg = _trans.localScale;
		var end = Vector3.one;
		for (float t = 0f, perc = 0f; perc < 1f; t += Time.deltaTime)
		{
			perc = Mathf.Clamp01(t / 0.3f);
			_trans.localScale = Vector3.Lerp(beg, end, perc);
			yield return null;
		}
	}

	private void Update()
	{
		_trans.Translate(_dir * Speed * Time.deltaTime);
	}


	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.transform.tag == "Player")
		{
			var script = collision.transform.GetComponent<PawnComponent>();

			if (script != null)
				script.HitMe();
			DestroyBullet();
			return;
		}

		if (_bounceLeft <= 0)
		{
			DestroyBullet();
			return;
		}
		--_bounceLeft;
		var normal = collision.contacts[0].normal;

		var dir = _trans.right;
		

		if (Mathf.Abs(normal.x) > 0.1f)
			dir.x *= -1;

		if (Mathf.Abs(normal.y) > 0.1f)
			dir.y *= -1;
		
		var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

		_trans.eulerAngles = new Vector3(0f, 0f, angle);
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
