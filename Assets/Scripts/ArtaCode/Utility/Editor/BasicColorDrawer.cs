using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(BasicColor))]
public class BasicColorDrawer : PropertyDrawer {

	public override void OnGUI(Rect pos, SerializedProperty property, GUIContent label){

		GUI.color = new Color (250 / 255f, 200 / 255f, 100 / 255f);
		GUI.Box (pos, "");

		GUI.color = Color.blue;
		GUI.Box (new Rect(pos.x, pos.y, 8,8), "");
		GUI.color = Color.grey;
		GUI.Box (new Rect(pos.x+8, pos.y+8, 8,8), "");
		GUI.color = Color.white;



		label = EditorGUI.BeginProperty (pos, label, property);
		Rect contPos = EditorGUI.PrefixLabel (pos, label);

		int r = property.FindPropertyRelative ("_r").intValue;
		int g = property.FindPropertyRelative ("_g").intValue;
		int b = property.FindPropertyRelative ("_b").intValue;
		int a = property.FindPropertyRelative ("_a").intValue;

		Color c = EditorGUI.ColorField (contPos, GameUtility.ColorFromRGB(r,g,b,a));

		property.FindPropertyRelative ("_r").intValue = BasicColor.FromColor (c).r;
		property.FindPropertyRelative ("_g").intValue = BasicColor.FromColor (c).g;
		property.FindPropertyRelative ("_b").intValue = BasicColor.FromColor (c).b;
		property.FindPropertyRelative ("_a").intValue = BasicColor.FromColor (c).a;

		EditorGUI.EndProperty ();
	}
}
