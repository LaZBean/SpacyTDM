using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameUtility {


	//2-ple array a[x+y*w]
	//3-ple array a[x+y*w+z*w*h]
	
	public static Vector3 DirFromAngle(float angleInDeg){
		return new Vector3(Mathf.Sin(angleInDeg * Mathf.Deg2Rad),0,Mathf.Cos(angleInDeg * Mathf.Deg2Rad));
	}

	public static Vector3 MouseWorldPosOnPlane(Camera cam, Vector3 normal, Vector3 point) {
		Vector3 pos = Vector3.zero;

		if (cam == null) {
			Debug.LogError ("Camera = null");
			return pos;
		}

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Plane hPlane = new Plane(normal, point);
		float distance = 0; 

		if (hPlane.Raycast(ray, out distance)){
			pos = ray.GetPoint(distance);
		}

		return pos;
	}

	public static Vector2 ScreenPosToRectPosNormalize(Vector2 pos ,RectTransform rect){
		Vector2 localpoint;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, pos, rect.GetComponentInParent<Canvas>().worldCamera, out localpoint);

		Vector2 normalizedPoint = Rect.PointToNormalized(rect.rect, localpoint);

		return normalizedPoint;
	}

	public static Vector3 DirNormalize(Vector3 dir){
		if (dir.sqrMagnitude > 1) {
			dir.Normalize ();
		}
		return dir;
	}

	public static Rect RectTransformToScreenSpace(RectTransform transform)
	{
		Vector2 size= Vector2.Scale(transform.rect.size, transform.lossyScale);
		float x= transform.position.x + transform.anchoredPosition.x;
		float y= Screen.height - transform.position.y - transform.anchoredPosition.y;

		return new Rect(x, y, size.x, size.y);
	}
	

	public Vector3 WorldToGUIPoint(Vector3 pos){
		Vector3 guiPos = Camera.main.WorldToScreenPoint (pos);
		guiPos.y = Screen.height - guiPos.y;
		return guiPos;
	}

	public static Rect GetScreenRect( Vector3 screenPosition1, Vector3 screenPosition2 )
	{
		screenPosition1.y = Screen.height - screenPosition1.y;
		screenPosition2.y = Screen.height - screenPosition2.y;

		var topLeft = Vector3.Min( screenPosition1, screenPosition2 );
		var bottomRight = Vector3.Max( screenPosition1, screenPosition2 );

		return Rect.MinMaxRect( topLeft.x, topLeft.y, bottomRight.x, bottomRight.y );
	}
	

	public static float RandomValue() {
		return Random.Range (0, 1f);
	}

	public static float RoundToXD(float v, int x) {
		float mult = Mathf.Pow(10.0f, (float)x);
		return Mathf.Round(v * mult) / mult;
	}

	public static Vector3 RoundToXDVector3(Vector3 v, int x){
		return new Vector3 (RoundToXD(v.x, x), RoundToXD(v.y, x), RoundToXD(v.z, x));
	}

	public static Vector3 Vector2ToVector3(Vector2 v){
		return new Vector3 (v.x, v.y, 0);
	}

	public static Vector2 Vector3ToVector2(Vector3 v){
		return new Vector2 (v.x, v.y);
	}

    public static Vector2 VectorRotate(Vector2 v, float angle)
    {
        float x = Mathf.Cos(Mathf.PI/180f * angle);
        float y = Mathf.Sin(Mathf.PI/180f * angle);
        return new Vector2(x*v.x - y*v.y, y*v.x + x*v.y);
    }

    public static Color RandomColor() {
		return new Color(Random.Range (0, 1f), Random.Range (0, 1f), Random.Range (0, 1f));
	}

	public static Color ColorFromRGB(float r, float g, float b, float a=1f) {
		return new Color (r, g, b, a);
	}

	public static Color ColorFromRGB(int r, int g, int b, int a=255) {
		return new Color (r/255f, g/255f, b/255f, a/255f);
	}

	public static Color[] ClearColorArray(int n){
		if (n == 0)	return null;
		Color[] cs = new Color[n];
		Color color = new Color (0, 0, 0, 0);
		for (int i = 0; i < n; i++) {
			cs[i] = color;
		}
		return cs;
	}

	public static Color[] SolidColorArray(Color c, int n) {
		if (n == 0) return null;

		Color[] cs = new Color[n];
		for (int i = 0; i < n; i++) {
			cs[i] = c;
		}
		return cs;
	}

	public static Texture2D GetSpriteTexture(Sprite s) {
		int x = (int)s.rect.x;
		int y = (int)s.rect.y;
		int w = (int)s.rect.width;
		int h = (int)s.rect.height;

		Texture2D tex = new Texture2D(w, h);

		Color[] cs = s.texture.GetPixels (x, y, w, h);
		tex.SetPixels (cs);

		tex.filterMode = FilterMode.Point;
		return tex;
	}



	public static int GetDistance(int x1, int y1, int x2, int y2){

		int dx = Mathf.Abs (x1 - x2);
		int dy = Mathf.Abs (y1 - y2);

		if (dx > dy) {
			return (14 * dy + 10 * (dx - dy)) /10;
		}
		return (14 * dx + 10 * (dy - dx)) /10;
	}

	public static int GetDistance(Vector2 p1, Vector2 p2){
		return GetDistance ((int)p1.x, (int)p1.y, (int)p2.x, (int)p2.y);
	}



	public static int[] Dem2ToDem1Array(int[,] map){
		int w = map.GetLength(0);
		int h = map.GetLength(1);
		int[] newMap = new int[w*h];

		for (int x = 0; x < w; x++) {
			for (int y = 0; y < h; y++) {
				newMap [x + w * y] = map [x, y];	//((y * 16 + z) * 16 + x) Dem3
			}
		}
		return newMap;
	}

	public static Vector2 GridPosToIsometric(int x, int y, int tileSizeX = 32, int tileSizeY = 16, float pixPerUnit = 100f){
		float nx = (x-y) * (tileSizeX / 2f);
		float ny = (x+y) * (tileSizeY / 2f);
		return new Vector2 (nx, ny) / pixPerUnit;
	}

	public static Vector2Int WorldToGridPos(Vector2 pos, int tileWidth = 16, int tileHeight = 16, Transform parent = null , float pixPerUnit = 100f){

		Vector3 offset = (parent == null) ? Vector3.zero : parent.transform.position;

		int nx = (int)((pos.x - offset.x) / (tileWidth / pixPerUnit) );
		int ny = (int)((pos.y - offset.y) / (tileHeight / pixPerUnit) );
		return new Vector2Int(nx, ny) ;
	}


	public static Vector3Int WorldToGridPos(Vector3 pos, int tileWidth = 1, int tileHeight = 1, int tileDepth = 1, Transform parent = null){

		Vector3 offset = (parent == null) ? Vector3.zero : parent.transform.position;

		int nx = (int)((pos.x - offset.x) / (tileWidth) );
		int ny = (int)((pos.y - offset.y) / (tileHeight) );
		int nz = (int)((pos.z - offset.z) / (tileDepth) );
		return new Vector3Int(nx, ny, nz) ;
	}



	public static float DirAngle(Vector2 toVector2, Vector2 fromVector2) {
		float angle = Vector2.Angle(fromVector2, toVector2);
		Vector3 cross = Vector3.Cross(fromVector2, toVector2);
		if (cross.z > 0)	angle = 360 - angle;
		return angle;
	}

	public static int SideOfCircle(float angle, int sides=8){
		float a = 360f / sides;
		for (int i = 0; i<sides; i++) {
			if (i == 0 && (angle >= 360f-(a / 2f) && angle <= 360f) || (angle < (a / 2) && angle >= 0)) {
				return 0;
			} 
			else {
				if(angle>=((a*i)-(a/2f)) && angle<((a*i)+(a/2))){
					return i;
				}
			}
		}
		return 0;
	}



	public static string ColorToHex(Color32 c){
		return c.r.ToString ("X2") + c.g.ToString ("X2") + c.b.ToString ("X2") + c.a.ToString ("X2");
	}

	public static Color32 HexToColor(string hex){
		hex = hex.Replace ("0x", "");// 0xFFFFFF
		hex = hex.Replace ("#", "");// #FFFFFF
		byte a = 255;
		byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);

		if(hex.Length == 8){
			a = byte.Parse(hex.Substring(6,2), System.Globalization.NumberStyles.HexNumber);
		}
		return (new Color32(r,g,b,a));
	}



	public static void DestroyAllChildren(Transform parent){
		for (int i = 0; i < parent.childCount; i++) {
			Object.Destroy (parent.GetChild (i).gameObject);
		}
	}
}




[System.Serializable]
public struct BasicColor{

	public BasicColor(int r, int g, int b, int a=255){
		_r = (byte)r;
		_g = (byte)g;
		_b = (byte)b;
		_a = (byte)a;
	}

	public BasicColor(float r, float g, float b, float a=1f){
		_r = (byte)((int)(r*255));
		_g = (byte)((int)(g*255));
		_b = (byte)((int)(b*255));
		_a = (byte)((int)(a*255));
	}

	[SerializeField]byte _r;
	[SerializeField]byte _g;
	[SerializeField]byte _b;
	[SerializeField]byte _a;

	public int r{ get{return _r;} set{_r = (byte)value;} }
	public int g{ get{return _g;} set{_g = (byte)value;} }
	public int b{ get{return _b;} set{_b = (byte)value;} }
	public int a{ get{return _a;} set{_a = (byte)value;} }

	public Color ToColor(){
		return GameUtility.ColorFromRGB (_r, _g, _b, _a);
	}

	public static BasicColor FromColor(Color color){
		return new BasicColor (color.r, color.g, color.b, color.a);
	}

	public static BasicColor Random(){
		return FromColor(GameUtility.RandomColor ());
	}
}







[System.Serializable]
public struct RectInt{
	[SerializeField]int _x;
	[SerializeField]int _y;
	[SerializeField]int _w;
	[SerializeField]int _h;

	public RectInt(int x, int y, int w, int h){ 
		_x = x; 
		_y = y; 
		_w = w; 
		_h = h;
	}

	public int x{ get{return _x;} set{_x = value;} }
	public int y{ get{return _y;} set{_y = value;} }
	public int w{ get{return _w;} set{_w = value;} }
	public int h{ get{return _h;} set{_h = value;} }

	public Rect ToRect(){
		return new Rect(_x, _y, _w, _h);
	}

	public static RectInt FromRect(Rect r){
		return new RectInt ((int)r.x, (int)r.y, (int)r.width, (int)r.height);
	}
}
	