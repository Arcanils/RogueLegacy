using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physics2DComponent : MonoBehaviour
{

	[Flags]
	public enum ECollision
	{
		BOTTOM = 0x01,
		TOP = 0x02,
		RIGHT = 0x04,
		LEFT = 0x08,
	}

	public float Mass;
	public float GravityScale;
	public LayerMask ColisionMask;

	public Vector2 ConstantForce { get; private set; }
	public ECollision CurrentCollision { get; private set; }


	private Vector2 _velocity;
	private BoxCollider2D _col;
	private Transform _trans;
	private Vector2 _manualMove;

	private void Reset()
	{
		Mass = 1f;
		GravityScale = 1f;
	}

	private void Awake()
	{
		_col = GetComponent<BoxCollider2D>();
		_trans = transform;
	}

	public void Update()
	{
		Tick(Time.deltaTime);
	}

	public void Tick(float deltaTime)
	{
		UpdateCollision();
		var firstparam = (Physics2D.gravity * GravityScale) * deltaTime * deltaTime;
		var secondparam = _velocity * deltaTime;
		_velocity += new Vector2(firstparam.x + secondparam.x, firstparam.y + firstparam.y);

		var deltaMove = new Vector3(_velocity.x + _manualMove.x * deltaTime, _velocity.y + _manualMove.y * deltaTime, 0f);

		if (((CurrentCollision & ECollision.BOTTOM) == ECollision.BOTTOM) && _velocity.y < 0f)
		{
			_velocity.y = 0f;
			deltaMove.y = 0f;
		}
		_trans.position += deltaMove;
		_manualMove = Vector2.zero;
	}

	public void Move(Vector2 offMove)
	{
		_manualMove = offMove;
	}


	private void UpdateCollision()
	{
		ECollision newCollisionState = 0;
		const float distance = 0.00f;
		var positionP = new Vector2(_trans.position.x, _trans.position.y);
		var colOffset = _col.offset;
		var colCenter = positionP + _col.offset;
		var colSize = _col.size / 2f;

		var pointBot = new Vector2(colCenter.x, colCenter.y - colSize.y);
		var pointTop = new Vector2(colCenter.x, colCenter.y + colSize.y);
		var pointLeft = new Vector2(colCenter.x - colSize.x, colCenter.y);
		var pointRight = new Vector2(colCenter.x + colSize.x, colCenter.y);

		TestCollisionDir(ref newCollisionState, ECollision.BOTTOM, Vector2.down, distance + colSize.y, pointLeft, pointRight);
		TestCollisionDir(ref newCollisionState, ECollision.TOP, Vector2.up, distance + colSize.y, pointLeft, pointRight);
		TestCollisionDir(ref newCollisionState, ECollision.LEFT, Vector2.left, distance + colSize.x, pointBot, pointTop);
		TestCollisionDir(ref newCollisionState, ECollision.RIGHT, Vector2.right, distance + colSize.x, pointBot, pointTop);

		CurrentCollision = newCollisionState;
	}

	private void TestCollisionDir(ref ECollision maskCol, ECollision valueToAddOnSucces, Vector2 dir, float distance, params Vector2[] points)
	{
		for (var i = points.Length - 1; i >= 0; --i)
		{
			var hit = Physics2D.Raycast(points[i], dir, distance, ColisionMask);

			if (hit.transform != null)
			{
				maskCol |= valueToAddOnSucces;
				return;
			}
		}
	}
}
