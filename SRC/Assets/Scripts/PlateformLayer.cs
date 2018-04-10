using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlateformLayer
{
	public static readonly int OnlyCollision;
	public static readonly int UpCollision;
	public static readonly int DownColision;

	private const string LayerFullCollision = "FullCollision";
	private const string LayerSoftTop = "SoftTop";
	private const string LayerSoftBottom = "SoftBottom";

	static PlateformLayer()
	{
		OnlyCollision = 1 << LayerMask.NameToLayer(LayerFullCollision) |
			1 << LayerMask.NameToLayer(LayerSoftTop) |
			1 << LayerMask.NameToLayer(LayerSoftBottom);

		UpCollision = 1 << LayerMask.NameToLayer(LayerFullCollision) |
			1 << LayerMask.NameToLayer(LayerSoftTop);

		DownColision = 1 << LayerMask.NameToLayer(LayerFullCollision) |
			1 << LayerMask.NameToLayer(LayerSoftBottom);
	}
}
