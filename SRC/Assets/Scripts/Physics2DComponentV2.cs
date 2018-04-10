using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physics2DComponentV2 : MonoBehaviour {

	public float Acceleration = 4f;
	public float MaxSpeed = 150f;
	public float Gravity = 6f;
	public float Mass;
	public float Maxfall = 200f;
	public float Jump = 200f;

	public LayerMask LayerWall;

	public int HorizontalRays = 6;
	public int VerticalRays = 4;
	public float Margin = 0.25f;

	public Vector2 Velocity { get { return _velocity; } }
	public bool Grounded { get { return _grounded && _frameOnGround > 1; } }
	private bool _grounded;
	private int _frameOnGround;
	private Vector2 _velocity;
	private Rect _box;
	private bool _falling;
	private float _moveX;

	private BoxCollider2D _col;
	private Transform _trans;
	private void Awake()
	{
		_trans = transform;
		_col = GetComponent<BoxCollider2D>();
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawCube(_col.bounds.min, Vector3.one * 0.05f);
		Gizmos.DrawCube(_col.bounds.max, Vector3.one);
	}


	private void FixedUpdate()
	{
		_box = new Rect(_col.bounds.min.x, _col.bounds.min.y, _col.bounds.size.x, _col.bounds.size.y);
		GravityMove();
		LateralMove();
		TopCollision();
	}

	private void LateUpdate()
	{
		_trans.Translate(_velocity * Time.deltaTime);
	}

	public void Move(float moveXPower)
	{
		_moveX = moveXPower;
	}

	public void ImpulseForce(Vector2 force)
	{
		_velocity = force;
	}
	private void GravityMove()
	{

		if (!_grounded)
		{
			_velocity.y = Mathf.Max(_velocity.y - Gravity, -Maxfall);
			_frameOnGround = 0;
		}

		_falling |= _velocity.y < 0f;

		if (_grounded || _falling)
		{
			var startPoint = new Vector3(_box.xMin + Margin, _box.center.y, _trans.position.z);
			var endPoint = new Vector3(_box.xMax - Margin, _box.center.y, _trans.position.z);

			var distance = _box.height / 2f + (_grounded ? Margin : Mathf.Abs(_velocity.y * Time.deltaTime));

			var connected = false;

			for (int i = 0; i < VerticalRays; i++)
			{
				var lerpAmount = (float)i / ((float)VerticalRays - 1);
				var origin = Vector3.Lerp(startPoint, endPoint, lerpAmount);

				var hit = Physics2D.Raycast(origin, Vector2.down, distance, LayerWall);

				if (hit.transform != null)
				{
					connected = true;
					_grounded = true;
					++_frameOnGround;
					_falling = false;
					_trans.Translate(Vector3.down * (hit.distance - _box.height / 2f));
					_velocity.y = 0f;
					break;
				}
			}
			
			if (!connected)
			{
				_grounded = false;
				_frameOnGround = 0;
			}
		}
	}

	private void LateralMove()
	{
		_velocity.x = _moveX;

		if (_velocity.x != 0f)
		{
			var startPoint = new Vector3(_box.center.x, _box.yMin + Margin, _trans.position.z);
			var endPoint = new Vector3(_box.center.x, _box.yMax - Margin, _trans.position.z);

			var distance = _box.width / 2f + Mathf.Abs(_velocity.x * Time.deltaTime);
			var dir = _velocity.x > 0f ? Vector2.right : Vector2.left;
			

			for (int i = 0; i < HorizontalRays; i++)
			{
				var lerpAmount = (float)i / ((float)HorizontalRays - 1);
				var origin = Vector3.Lerp(startPoint, endPoint, lerpAmount);

				var hit = Physics2D.Raycast(origin, dir, distance, LayerWall);

				if (hit.transform != null)
				{
					_trans.Translate(dir * (hit.distance - _box.width / 2f));
					_velocity.x = 0f;
					break;
				}
			}
		}
	}


	private void TopCollision()
	{
		if (_grounded || _velocity.y > 0f)
		{
			var startPoint = new Vector3(_box.xMin + Margin, _box.center.y, _trans.position.z);
			var endPoint = new Vector3(_box.xMax - Margin, _box.center.y, _trans.position.z);

			var distance = _box.height / 2f + (_grounded ? Margin : Mathf.Abs(_velocity.y * Time.deltaTime));
			Debug.Log("Test top");

			for (int i = 0; i < VerticalRays; i++)
			{
				var lerpAmount = (float)i / ((float)VerticalRays - 1);
				var origin = Vector3.Lerp(startPoint, endPoint, lerpAmount);

				var hit = Physics2D.Raycast(origin, Vector2.up, distance, LayerWall);

				if (hit.transform != null)
				{
					Debug.LogWarning("HIT TOP !");
					_falling = true;
					_trans.Translate(Vector2.up * (hit.distance - _box.height / 2f));
					_velocity.y = 0f;
					break;
				}
			}
		}
	}

}
