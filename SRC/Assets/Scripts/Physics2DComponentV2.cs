using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physics2DComponentV2 : MonoBehaviour {

	public float Acceleration = 4f;
	public float MaxSpeed = 150f;
	public float Gravity = 6f;
	public float Maxfall = 200f;
	public float Jump = 200f;

	public LayerMask LayerWall;

	public int HorizontalRays = 6;
	public int VerticalRays = 4;
	public float Margin = 0.25f;

	private Rect _box;
	private Vector2 _velocity;
	private bool _grounded = false;
	private bool _falling = false;

	private BoxCollider2D _col;
	private Transform _trans;

	private void Awake()
	{
		_trans = transform;
		_col = GetComponent<BoxCollider2D>();
	}

	private void FixedUpdate()
	{
		_box = new Rect(_col.bounds.min.x, _col.bounds.min.y, _col.bounds.size.x, _col.bounds.size.y);

		if (!_grounded)
			_velocity.y = Mathf.Max(_velocity.y - Gravity, -Maxfall);

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
					_falling = false;
					_trans.Translate(Vector3.down * (hit.distance - _box.height / 2f));
					_velocity.y = 0f;
					break;
				}
			}

			_grounded &= connected;
		}

	}

	private void LateUpdate()
	{
		_trans.Translate(_velocity * Time.deltaTime);
	}

}
