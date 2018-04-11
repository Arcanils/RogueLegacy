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

	private bool _enableVerticalVelocity;

	private BoxCollider2D _col;
	private Transform _trans;
	private void Awake()
	{
		_trans = transform;
		_col = GetComponent<BoxCollider2D>();
		_enableVerticalVelocity = true;
	}

	private void FixedUpdate()
	{
		_box = new Rect(_col.bounds.min.x, _col.bounds.min.y, _col.bounds.size.x, _col.bounds.size.y);

		if (_enableVerticalVelocity && !_grounded)
		{
			_velocity.y = Mathf.Max(_velocity.y - Gravity, -Maxfall);
			_frameOnGround = 0;
		}

		if (_enableVerticalVelocity)
			GravityMove();

		LateralMove();

		if (_enableVerticalVelocity)
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

	public void SetActiveVelocityVertical(bool enable)
	{
		if (!enable)
		{
			_enableVerticalVelocity = false;
			_velocity.y = 0f;
			_grounded = false;
			_falling = false;
			_frameOnGround = 0;
		}
		else
			_enableVerticalVelocity = true;
	}

	private void GravityMove()
	{
		_falling |= _velocity.y < 0f;

		if (_grounded || _falling)
		{
			var downRayLength = _box.height / 2f + (_grounded ? Margin : Mathf.Abs(_velocity.y * Time.deltaTime));
			var connection = false;
			var lastConnection = 0;
			var min = new Vector2(_box.xMin + Margin, _box.center.y);
			var max = new Vector2(_box.xMax - Margin, _box.center.y);
			var downRays = new RaycastHit2D[VerticalRays];

			for (int i = 0; i < VerticalRays; i++)
			{
				var lerpAmount = (float)i / ((float)VerticalRays - 1);
				var start = Vector2.Lerp(min, max, lerpAmount);
				var end = start + Vector2.down * downRayLength;
				downRays[i] = Physics2D.Linecast(start, end, PlateformLayer.DownColision);

				if (downRays[i].fraction > 0f)
				{
					connection = true;
					lastConnection = i;
				}
			}
			if (connection)
			{
				_grounded = true;
				++_frameOnGround;
				_falling = false;
				_velocity.y = 0f;
				_trans.position += Vector3.down * (-downRays[lastConnection].point.y + _box.yMin);
			}
			else
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
			var upRayLength = _grounded ? Margin : _velocity.y * Time.deltaTime;
			var connection = false;
			var lastConnection = 0;
			var min = new Vector2(_box.xMin + Margin, _box.center.y);
			var max = new Vector2(_box.xMax - Margin, _box.center.y);
			var upRays = new RaycastHit2D[VerticalRays];

			for (int i = 0; i < VerticalRays; i++)
			{
				var lerpAmount = (float)i / ((float)VerticalRays - 1);
				var start = Vector2.Lerp(min, max, lerpAmount);
				var end = start + Vector2.up * (upRayLength + _box.height / 2f);

				upRays[i] = Physics2D.Linecast(start, end, PlateformLayer.UpCollision);

				if (upRays[i].fraction > 0f)
				{
					connection = true;
					lastConnection = i;
				}
			}

			if (connection)
			{
				_velocity.y = 0f;
				_trans.position += Vector3.up * (upRays[lastConnection].point.y - _box.yMax);
			}
		}
	}
}
