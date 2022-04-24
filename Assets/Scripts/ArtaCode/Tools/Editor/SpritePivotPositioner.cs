#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

internal class SpritePivotPositioner : EditorWindow
{
	#region Private Fields

	private static Texture2D _rectTexture;
	private static bool _newPivotSelected;
	private static int _newPivotXPixel;
	private static int _newPivotYPixel;
	private static Sprite _sprite;

	private static float _frameMargin = 2;
	private static float _scale = 6;
	private static Color _borderColor = Color.gray;
	private static float _shade = 0.6f;
	private static Color _frameColor = new Color(_shade, _shade, _shade);

	private static Color _currentPivotColor = Color.green;
	private static Color _mousePosColor = Color.gray;
	private static Color _newPivotColor = Color.blue;
	private static Color _misplacedPivotColor = Color.red;

	private float _outerFrameWidth;
	private float _outerFrameHeight;
	private float _innerFrameWidth;
	private float _innerFrameHeight;
	private float _x;
	private float _y;

	int _spritesCount;
	int _anotherCount;

	[MenuItem("Pixpat/Sprites/Sprite Pivot Positioner")]
	public static void ShowWindow(){
		EditorWindow window = EditorWindow.GetWindow(typeof(SpritePivotPositioner));
	}



	private void Update(){
		
		Repaint();
	}

	private void OnGUI(){
		
		DrawMainPanel ();

		if (!SelectSprite()) return;
		DrawSprite();
		DrawMousePos();
		DrawNewPivot();
		DrawCurrentPivot();
		ProcessMouseClick();
		//ProcessApplyButton();

		DrawGrid ();
		DrawInfo ();

		//_scale = Mathf.Min(( (Screen.width-10)/(_sprite.rect.width)) , ( (Screen.height-50)/(_sprite.rect.height+5)));
		_scale = Mathf.Min(( (Screen.width-10)/(_sprite.rect.width)) , ( (Screen.height-48)/(_sprite.rect.height)));
	}

	private bool SelectSprite(){
		
		/*if (Selection.activeObject != null &&
			Selection.activeObject is UnityEngine.Sprite){ 

			if (_sprite != (Sprite)Selection.activeObject)
			{
				_sprite = (Sprite)Selection.activeObject;
				_newPivotSelected = false;
			}

			Debug.Log (Selection.objects.Length);
			return true;
		}
		return false;*/
		_spritesCount = 0;
		_anotherCount = 0;

		for (int i = 0; i < Selection.objects.Length; i++) {
			UnityEngine.Object obj = Selection.objects [i];

			if (obj != null && obj is Sprite) {
				_spritesCount++;
			} else {
				_anotherCount++;
			}
		}

		if (Selection.activeObject != null && _sprite != Selection.activeObject && Selection.activeObject is Sprite)
		{
			_sprite = (Sprite)Selection.activeObject;
			_newPivotSelected = false;
		}
			

		if (_spritesCount == 0) {
			return false;
		}
		return true;
	}

	private void DrawSprite(){
		
		_innerFrameWidth = _sprite.rect.width * _scale;
		_innerFrameHeight = _sprite.rect.height * _scale;

		_outerFrameWidth = _innerFrameWidth + 2 * _frameMargin;
		_outerFrameHeight = _innerFrameHeight + 2 * _frameMargin;

		var rect = EditorGUILayout.GetControlRect(true, _outerFrameHeight);
		_x = rect.min.x;
		_y = rect.min.y;

		//draw the rect that fills the scroll:
		GUIExtensions.DrawRect(new Rect(_x, _y, _outerFrameWidth, _outerFrameHeight), _borderColor, ref _rectTexture);

		//draw the background colour of each frame:
		_x += _frameMargin;
		_y += _frameMargin;
		GUIExtensions.DrawRect(new Rect(_x, _y, _innerFrameWidth, _innerFrameHeight), _frameColor, ref _rectTexture);

		//draw the sprite
		Texture texture = _sprite.texture;
		Rect textureRect = _sprite.rect;
		var textureCoords = new Rect(textureRect.x / texture.width, textureRect.y / texture.height,
			textureRect.width / texture.width, textureRect.height / texture.height);
		var positionRect = new Rect(_x, _y, _innerFrameWidth, _innerFrameHeight);
		GUI.DrawTextureWithTexCoords(positionRect, texture, textureCoords);
		//
	}


	//DRAW INTERFACE
	bool _grid = true;
	string _newName = "";

	private void DrawMainPanel(){
		GUILayout.BeginHorizontal(EditorStyles.toolbar);

		_grid = GUILayout.Toggle(_grid, "Grid", EditorStyles.toolbarButton);

		GUILayout.Label ("Background");
		_shade = GUILayout.HorizontalSlider (_shade, 0f, 1f, GUILayout.Width(100));
		_frameColor = new Color(_shade, _shade, _shade);
		GUILayout.Label (_shade + "");

		GUILayout.Space (20);

		GUILayout.Label ("Rename:");
		_newName = GUILayout.TextField (_newName, GUILayout.Width(100));



		GUILayout.FlexibleSpace ();

		GUI.enabled = _sprite && _newPivotSelected;
		if (GUILayout.Button ("Apply", EditorStyles.toolbarButton))
			ProcessApplyButton ();
		GUI.enabled = true;

		GUILayout.EndHorizontal();
	}

	//DRAW GRID
	private void DrawGrid(){
		if (!_grid)
			return;

		Rect positionRect = new Rect(_x, _y, _innerFrameWidth, _innerFrameHeight);

		float shade = 1-_shade;
		Color x1 = new Color(shade,shade,shade, 0.2f);
		Color x4 = new Color(shade,shade,shade, 0.4f);
		Color x8 = new Color(shade,shade,shade, 0.6f);

		GUI.BeginGroup (positionRect);
		for (int x = 0; x < _sprite.rect.height; x++) {
			Color c = (x%8 == 0)? x8 : (x%4 == 0)? x4 : x1;
			Rect rect = new Rect (0, x * _scale-1, Screen.width, 1);
			GUIExtensions.DrawRect (rect, c, ref _rectTexture);
		}

		for (int y = 0; y < _sprite.rect.width; y++) {
			Color c = (y%8 == 0)? x8 : (y%4 == 0)? x4 : x1;
			Rect rect = new Rect (y * _scale-1, 0, 1, Screen.height);
			GUIExtensions.DrawRect (rect, c, ref _rectTexture);
		}
		GUI.EndGroup ();
	}

	//DRAW INFO
	private void DrawInfo(){
		var positionRect = new Rect(_x, _y, _innerFrameWidth, _innerFrameHeight);

		Rect infoRect1 = positionRect;
		Rect infoRect2 = new Rect(positionRect.x, positionRect.y + 15, positionRect.width, positionRect.height);

		Rect infoRect3 = new Rect(positionRect.x, positionRect.y + 30, positionRect.width, positionRect.height);
		Rect infoRect4 = new Rect(positionRect.x, positionRect.y + 45, positionRect.width, positionRect.height);

		GUI.Label (infoRect1, "Current pivot: (" + _sprite.pivot.x + ", " + _sprite.pivot.y + ")");
		GUI.Label (infoRect2, "New pivot: (" + _newPivotXPixel + ", " + _newPivotYPixel + ")");

		GUI.Label (infoRect3, "Selected objects: "+Selection.objects.Length+" (sprites: " + _spritesCount + ", other: " + _anotherCount + ")");

		if (Selection.activeObject != null && Selection.activeObject is Sprite) {
			GUI.Label (infoRect4, "Current sprite: [" + Selection.activeObject.name + "]");
		}

		GUI.color = Color.red;
		int x, y;
		if (GetMousePixel (out x, out y)) {
			Rect infoMouseRect = new Rect (Event.current.mousePosition.x-24, Event.current.mousePosition.y-16, 100, 20);
			GUI.Label (infoMouseRect, "(" + (x) + ", " + (int)(_sprite.rect.height - 1 - y) + ")");
		}
		GUI.color = Color.white;
	}

	//CURVE TEST
	public static void DrawCurves(Rect wr, Rect wr2,Color color)
	{
		Vector3 startPos = new Vector3(wr.x + wr.width, wr.y + 3 + wr.height / 3, 0);
		Vector3 endPos = new Vector3(wr2.x, wr2.y + wr2.height / 2, 0);
		float mnog = Vector3.Distance(startPos,endPos);
		Vector3 startTangent = startPos + Vector3.right * (mnog / 3f) ;
		Vector3 endTangent = endPos + Vector3.left * (mnog / 3f);
		Handles.BeginGUI();
		Handles.DrawBezier(startPos, endPos, startTangent, endTangent,color, null, 3f);
		Handles.EndGUI();
	}






	private void DrawPixel(float x, float y, Color color, bool invertY = true){
		
		bool intergerPosition = (x == Math.Floor(x) && y == Math.Floor(y));
		x = _x + x * _scale;
		if (invertY) y = _sprite.rect.height - 1 - y;
		y = _y + y * _scale;
		GUIExtensions.DrawRect(new Rect(x, y, _scale, _scale), intergerPosition ? color : _misplacedPivotColor, ref _rectTexture);
	}

	private void DrawCurrentPivot()
	{
		float x = _sprite.pivot.x;
		float y = _sprite.pivot.y;
		DrawPixel(x, y, _currentPivotColor);
	}

	private void DrawNewPivot()
	{
		if (!_newPivotSelected) return;
		DrawPixel(_newPivotXPixel, _newPivotYPixel, _newPivotColor);
	}

	private void DrawMousePos()
	{
		int x, y;
		if (GetMousePixel(out x, out y))
		{
			DrawPixel(x, y, _mousePosColor, false);
		}
	}

	private bool GetMousePixel(out int x, out int y)
	{
		x = (int)((Event.current.mousePosition.x - _x) / _scale);
		y = (int)((Event.current.mousePosition.y - _y) / _scale);
		return x >= 0 && x < _sprite.rect.width &&
			y >= 0 && y < _sprite.rect.height;
	}

	private void ProcessMouseClick()
	{
		
		int x, y;
		if (GetMousePixel(out x, out y) &&
			Event.current.isMouse)
		{
			//-==================
			//Debug.Log ("click");
			_newPivotSelected = true;
			_newPivotXPixel = x;
			_newPivotYPixel = (int)_sprite.rect.height - 1 - y;
		}
	}
		

	private void ProcessApplyButton()
	{
		
		/*GUI.enabled = _newPivotSelected;
		if (!GUILayout.Button("Apply Changes")) return;
		GUI.enabled = true;*/
		List<string> names = new List<string> ();
		for (int i = 0; i < Selection.objects.Length; i++) {
			if (Selection.objects [i] is Sprite)
				names.Add (Selection.objects [i].name);
		}

		int curN = 0;

		string path = AssetDatabase.GetAssetPath (_sprite.texture);
		var textureImporter = AssetImporter.GetAtPath (path) as TextureImporter;
		var spritesheet = textureImporter.spritesheet;
		for (int i = 0; i < spritesheet.Length; i++) {
			/*if (spritesheet [i].name != _sprite.name)
				continue;*/

			bool b = false;
			for (int n = 0; n < names.Count; n++) {
				if(spritesheet [i].name == names[n]) b = true;
			}

			if (!b)	continue;

			curN++;

			textureImporter.isReadable = true;
			var spriteMetaData = spritesheet [i];



			spriteMetaData.alignment = (int)SpriteAlignment.Custom;
			float xFraction = _newPivotXPixel / (float)_sprite.rect.width;
			float yFraction = _newPivotYPixel / (float)_sprite.rect.height;
			spriteMetaData.pivot = new Vector2 (xFraction, yFraction);

			spritesheet [i] = spriteMetaData;

			textureImporter.spritesheet = spritesheet;
			textureImporter.isReadable = false; //apparently this must be before the AssetDatabase.ImportAsset(...) call
			AssetDatabase.ImportAsset (path, ImportAssetOptions.ForceUpdate);

		}
		_newPivotSelected = false;

	}

	private void OnDestroy()
	{
		_rectTexture = null;
	}
	#endregion Private Methods
}

public class GUIExtensions
{
	static private GUIStyle _rectStyle;
	//I am passing the rectTexture rather than using a local 
	//static variable because it will leak memory otherwise
	public static void DrawRect(Rect position, Color color, ref Texture2D _rectTexture)
	{
		if (_rectTexture == null)
		{
			_rectTexture = new Texture2D(1, 1);
			_rectTexture.hideFlags = HideFlags.DontSaveInEditor;
		}
		if (_rectStyle == null)
		{
			_rectStyle = new GUIStyle();
		}
		_rectTexture.SetPixel(0, 0, color);
		_rectTexture.Apply();
		_rectStyle.normal.background = _rectTexture;
		GUI.Box(position, GUIContent.none, _rectStyle);
	}
}

#endif 



