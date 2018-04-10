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
		_physic = GetComponent<Physics2DComponentV2>();
		//_rigid = GetComponent<Rigidbody2D>();
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
		_deltaMove = inputX * SpeedMove;
		_physic.Move(_deltaMove);
	}

	private void MoveLogic()
	{
		//_trans.position += new Vector3(_deltaMove, 0f, 0f);

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
	//private Rigidbody2D _rigid;
	private Physics2DComponentV2 _physic;

	private bool _inputJumpDown;
	private int _nJumpLeft;

	public void InputJump(bool isDown)
	{
		_inputJumpDown = isDown;
	}

	private void JumpLogic()
	{
		if (_physic.Grounded)
			StateJump = EJumpState.GROUNDED;
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
}
