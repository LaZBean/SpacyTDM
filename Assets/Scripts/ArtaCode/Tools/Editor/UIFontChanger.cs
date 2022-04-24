using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

internal class UIFontChanger : EditorWindow
{

	private static GameObject _obj;

	private Font _font;



	[MenuItem("Pixpat/UI/UI Font Changer")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(UIFontChanger));
	}




	private void Update()
	{
		Repaint();
	}

	private void OnGUI()
	{
		if (!SelectGameObject()){
			GUILayout.Label("GameObject is missing!");
		}
		else{
			
			_font = EditorGUILayout.ObjectField(_font, typeof(Font)) as Font;

			if(GUILayout.Button("Change for all children")){

				Text[] txts = _obj.GetComponentsInChildren<Text>(true);
				for(int i=0; i<txts.Length; i++){
					txts[i].font = _font;
				}
				Debug.Log("Update " + txts.Length + " fonts");
			}

		}
	}

	private bool SelectGameObject()
	{
		if (Selection.activeObject != null && Selection.activeObject is UnityEngine.GameObject){
			if (_obj!= (GameObject)Selection.activeObject){
				_obj = (GameObject)Selection.activeObject;
			}

			return true;
		}
		return false;
	}




}

