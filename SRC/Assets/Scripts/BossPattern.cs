using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossPattern", menuName = "Boss/Pattern", order = 0)]
public class BossPattern : ScriptableObject
{

#if UNITY_EDITOR
	public string Description;
#endif

	public BossSubPattern[] SubPattern;
}

