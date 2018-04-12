using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMoveOffsetTexture : MonoBehaviour {


	public Vector2 Speed;

	private Material _mat;
	private Vector2 _offset;
	private void Awake()
	{
		_mat = GetComponent<Renderer>().material;
	}

	private void Update()
	{
		_offset += Speed * Time.deltaTime;
		_mat.SetTextureOffset("_MainTex", _offset);
	}
}
