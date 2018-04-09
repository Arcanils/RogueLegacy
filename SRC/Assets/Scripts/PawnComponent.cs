using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnComponent : MonoBehaviour {


	private Transform _trans;


	private void Awake()
	{
		_trans = transform;
		_sr = GetComponentInChildren<SpriteRenderer>();
		_col = GetComponent<BoxCollider2D>();
		_rigid = GetComponent<Rigidbody2D>();
		_nJumpLeft = NJump;
	}
	private void Reset()
	{
		SpeedMove = 10f;
		NJump = 3;
		ForceJump = 40f;
	}

	private void FixedUpdate()
	{
		MoveLogic();
		JumpLogic();
		//Jump()
	}

	#region Move

	public float SpeedMove;
	private float _deltaMove;
	private SpriteRenderer _sr;

	public void InputMove(float inputX)
	{
		_deltaMove += inputX * Time.fixedDeltaTime * SpeedMove;
	}

	private void MoveLogic()
	{
		_trans.position += new Vector3(_deltaMove, 0f, 0f);

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

	public enum EJumpState
	{
		GROUNDED,
		JUMPING,
		FALLING,
	}

	public float ForceJump;
	public int NJump;
	public LayerMask LayerWakeable;

	public EJumpState StateJump { get; private set; }

	private BoxCollider2D _col;
	private Rigidbody2D _rigid;

	private bool _inputJumpDown;
	private bool _isGrounded;
	private int _nJumpLeft;

	public void InputJump(bool isDown)
	{
		_inputJumpDown = isDown;
	}


	private void UpdateJumpStateFromFalling()
	{

		StateJump = EJumpState.GROUNDED;
		_isGrounded = true;
	}

	private bool IsGrounded()
	{
		var origin = new Vector2(_trans.position.x + _col.offset.x, _trans.position.y + _col.offset.y - (_col.size.y / 2f));
		var dir = Vector2.down;
		var hit = Physics2D.Raycast(origin, dir, 0.05f, LayerWakeable);

		return hit.transform != null;
	}

	private void UpdateJumpStateFromGround()
	{
		_isGrounded = IsGrounded();

		if (StateJump == EJumpState.GROUNDED)
		{
			if (!_isGrounded)
				StateJump = EJumpState.FALLING;

			return;
		}
		else if (StateJump == EJumpState.FALLING)
		{
			if (_isGrounded)
			{
				StateJump = EJumpState.GROUNDED;
				_nJumpLeft = NJump;
			}

			return;
		}
	}

	private void JumpLogic()
	{
		if (StateJump == EJumpState.GROUNDED || StateJump == EJumpState.FALLING)
		{
			if (_inputJumpDown && _nJumpLeft > 0)
			{
				--_nJumpLeft;
				StateJump = EJumpState.JUMPING;
				_rigid.velocity = Vector2.zero;
				_rigid.AddForce(Vector2.up * ForceJump, ForceMode2D.Impulse);
			}
			else
				UpdateJumpStateFromGround();
		}
		else
		{
			if (_rigid.velocity.y < 0f)
				StateJump = EJumpState.FALLING;
			else if (!_inputJumpDown)
			{
				StateJump = EJumpState.FALLING;
				_rigid.velocity = new Vector2(0f, -5f);
			}
		}
	}
}
