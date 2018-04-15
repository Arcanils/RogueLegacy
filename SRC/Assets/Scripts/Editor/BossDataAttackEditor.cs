using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BossDataAttack))]
public class BossDataAttackEditor : Editor {

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		var script = target as BossDataAttack;

		script.TypeTargeting = (BossDataAttack.ETarget)EditorGUILayout.MaskField(
			(int)script.TypeTargeting, System.Enum.GetNames(typeof(BossDataAttack.ETarget)));
		script.TypeDuration = (BossDataAttack.EDuration)EditorGUILayout.MaskField(
			(int)script.TypeDuration, System.Enum.GetNames(typeof(BossDataAttack.EDuration)));

	}
}
