using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public PawnComponent Pawn;

	private const string _keyMoveX = "Horizontal";
	private const string _keyJump = "Jump";
	private const string _keyLeftDash = "LeftDash";
	private const string _keyRightDash = "RightDash";

	private void Update()
	{
		var moveX = Input.GetAxisRaw(_keyMoveX);
		Pawn.InputMove(moveX);

		if (Input.GetButtonDown(_keyJump))
			Pawn.InputJump(true);
		else if (Input.GetButtonUp(_keyJump))
			Pawn.InputJump(false);

		if (Input.GetButton(_keyLeftDash))
			Pawn.Dash(false);

		if (Input.GetButton(_keyRightDash))
			Pawn.Dash(true);
	}
}
