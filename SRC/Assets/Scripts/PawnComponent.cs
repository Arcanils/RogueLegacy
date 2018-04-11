using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnComponent : MonoBehaviour {

	private Anim2DComponent _anim;

	private void Awake()
	{
		_sr = GetComponentInChildren<SpriteRenderer>();
		_anim = _sr.GetComponent<Anim2DComponent>();
		_physic = GetComponent<Physics2DComponentV2>();
		_nJumpLeft = NJump;
	}
	private void Reset()
	{
		SpeedMove = 10f;
		NJump = 3;
		ForceJump = 40f;
		DurationDash = 0.5f;
		NDash = 1;
	}

	private void FixedUpdate()
	{
		MoveLogic();
		JumpLogic();
		DashLogic();
	}

	#region Move

	public float SpeedMove;
	private float _deltaMove;
	private SpriteRenderer _sr;

	public void InputMove(float inputX)
	{
		if (_dash != null)
			return;
		_deltaMove = inputX * SpeedMove;
		_physic.Move(_deltaMove);
	}

	private void MoveLogic()
	{
		//_trans.position += new Vector3(_deltaMove, 0f, 0f);

		if (Math.Abs(_deltaMove) > Mathf.Epsilon)
			_anim.PlayAnim("Run");
		else
			_anim.PlayAnim("Idle");

		if (_deltaMove < 0f)
		{
			_sr.flipX = true;
		}
		else if (_deltaMove > 0f)
		{
			_sr.flipX = false;
		}
		_deltaMove = 0f;
	}


	#endregion

	#region Jump
	public enum EJumpState
	{
		GROUNDED,
		JUMPING,
		FALLING,
		DASH,
	}

	public float ForceJump;
	public int NJump;
	public LayerMask LayerWakeable;

	public EJumpState StateJump { get; private set; }
	private Physics2DComponentV2 _physic;

	private bool _inputJumpDown;
	private int _nJumpLeft;

	public void InputJump(bool isDown)
	{
		_inputJumpDown = isDown;
	}

	private void JumpLogic()
	{
		if (_dash != null)
		{
			StateJump = EJumpState.DASH;
			return;
		}
		if (_physic.Grounded)
		{
			StateJump = EJumpState.GROUNDED;
			_nDashLeft = NDash;
		}
		else if (_physic.Velocity.y < 0f)
			StateJump = EJumpState.FALLING;
		else
			StateJump = EJumpState.JUMPING;

		if (StateJump == EJumpState.GROUNDED && _inputJumpDown)
		{
			_nJumpLeft = NJump - 1;
			_physic.ImpulseForce(new Vector2(0f, ForceJump));
		}
		else if (StateJump == EJumpState.FALLING && _inputJumpDown && _nJumpLeft > 0)
		{
			--_nJumpLeft;
			_physic.ImpulseForce(new Vector2(0f, ForceJump));
		}
	}
	#endregion

	#region Dash

	public float DurationDash;
	public int NDash;
	private int _nDashLeft;
	private IEnumerator _dash;

	public void Dash(bool dashRight)
	{
		if (_dash != null || _nDashLeft <= 0)
			return;

		_dash = DashEnum(dashRight);
		--_nDashLeft;
	}

	private IEnumerator DashEnum(bool dashRight)
	{
		_physic.SetActiveVelocityVertical(false);
		_physic.Move(dashRight ? SpeedMove * 2f : -SpeedMove * 2f);
		for (float t = 0f, perc = 0f; perc < 1f; t += Time.fixedDeltaTime)
		{
			perc = t / DurationDash;
			yield return null;
		}

		_physic.SetActiveVelocityVertical(true);
		_physic.Move(0f);
	}

	private void DashLogic()
	{
		if (_dash != null)
		{
			if (!_dash.MoveNext())
			{
				_dash = null;
			}
		}
	}
	#endregion
}
