using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(SpriteBillboardRenderer))]
public class SpriteBillboardRendererEditor : Editor {


	public override void OnInspectorGUI()
	{
		//DrawDefaultInspector ();

		SpriteBillboardRenderer myTarget = (SpriteBillboardRenderer)target;

		myTarget.sprite = EditorGUILayout.ObjectField("Sprite", myTarget.sprite, typeof(Sprite), GUILayout.Height(16)) as Sprite;
		myTarget.color = EditorGUILayout.ColorField ("Color", myTarget.color);

		EditorGUILayout.BeginHorizontal ();
		myTarget.lookAtCamera = EditorGUILayout.Toggle ("Look At Camera", myTarget.lookAtCamera);
		myTarget.doNotRotate = EditorGUILayout.Toggle ("Do Not Rotate", myTarget.doNotRotate);

		EditorGUILayout.EndHorizontal ();

		myTarget.castShadow = EditorGUILayout.Toggle ("Cast Shadow", myTarget.castShadow);

		myTarget.material = EditorGUILayout.ObjectField("Material", myTarget.material, typeof(Material), GUILayout.Height(16)) as Material;
		myTarget.pixelPerUnit = EditorGUILayout.FloatField("Pixel Per Unit", myTarget.pixelPerUnit);

		EditorGUILayout.LabelField ("Y Angle To Camera", myTarget.angle.ToString());

		myTarget.zOffset = EditorGUILayout.FloatField("Z Offset", myTarget.zOffset);
		myTarget.xyOffset = EditorGUILayout.Vector2Field("XY Offset", myTarget.xyOffset);

		if(GUI.changed)
			EditorUtility.SetDirty (myTarget);
	}
}
