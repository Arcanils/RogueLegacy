using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(DataMovePattern))]
public class DataMovePatternEditor : PropertyDrawer
{

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		//base.OnGUI(position, property, label);
		// Using BeginProperty / EndProperty on the parent property means that
		// prefab override logic works on the entire property.
		EditorGUI.BeginProperty(position, label, property);

		// Draw label
		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

		// Don't make child fields be indented
		var indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		// Calculate rects
		var amountRect = new Rect(position.x, position.y, 100, position.height / 2f);
		var unitRect = new Rect(position.x, position.y + position.height / 2f, 50, position.height / 2f);

		// Draw fields - passs GUIContent.none to each so they are drawn without labels
		var prop = property.FindPropertyRelative("Move");
		prop.intValue = EditorGUI.MaskField(amountRect, GUIContent.none, prop.intValue, System.Enum.GetNames(typeof(DataMovePattern.EPatternMove)));
		EditorGUI.PropertyField(unitRect, property.FindPropertyRelative("Amount"), GUIContent.none);


		// Set indent back to what it was
		EditorGUI.indentLevel = indent;

		EditorGUI.EndProperty();
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return base.GetPropertyHeight(property, label)  * 2f;
	}
}
