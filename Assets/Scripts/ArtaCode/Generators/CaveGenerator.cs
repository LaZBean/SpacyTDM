using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CaveGenerator : MonoBehaviour {

	public int width = 128;
	public int height = 128;

	public string seed;

	[Range(0,100)]public int randomFillPercent = 30;

	int [,] map;

	private Texture2D tex;

	void Start () {
		Generate ();

	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.Space)) {
			Generate();
		}
	}
	

	void Generate() {
		map = new int[width, height];
		RandomFill (map, seed, randomFillPercent);

		for (int i = 0; i < 5; i++) {
			Smooth (map);
		}

		GenerateTexture ();
	}




	public static void RandomFill(int[,] map, string seed="", int fillPercent = 50){
		if (seed=="") {
			seed = Time.time.ToString ();
		}

		int w = map.GetLength (0);
		int h = map.GetLength (1);

		System.Random r = new System.Random (seed.GetHashCode());

		for (int x = 0; x < w; x++) {
			for (int y = 0; y < h; y++) {

				if (x == 0 || x == w - 1 || y == 0 || y == h - 1) {
					map [x, y] = 1;
				} else {
					map [x, y] = (r.Next (0, 100) < fillPercent) ? 1 : 0;
				}
			}
		}


	}

	public static void Smooth(int[,] map){

		int w = map.GetLength (0);
		int h = map.GetLength (1);

		for (int x = 0; x < w; x++) {
			for (int y = 0; y < h; y++) {
				int neighbours = GetSurroundingWallCount (x,y, map);

				if (neighbours > 4)
					map [x, y] = 1;
				else if(neighbours<4)
					map [x, y] = 0;
					
			}
		}
	}

	public static int GetSurroundingWallCount(int gridX, int gridY, int[,] map){

		int w = map.GetLength (0);
		int h = map.GetLength (1);

		int wCount = 0;
		for (int neighbourX = gridX-1; neighbourX <= gridX+1; neighbourX++) {
			for (int neighbourY = gridY-1; neighbourY <= gridY+1; neighbourY++) {
				if (neighbourX >= 0 && neighbourX < w && neighbourY >= 0 && neighbourY < h) { 
					if (neighbourX != gridX || neighbourY != gridY) {
						wCount += map [neighbourX, neighbourY];
					}
				} else {
					wCount++;
				}
			}
		}
		return wCount;
	}








	public static int[,] GenerateCave(int w, int h, int fill = 50, string seed = "", bool inverce=false){
		int[,] map = new int[w, h];


		RandomFill (map, seed, fill);

		for (int i = 0; i < 5; i++) {
			Smooth (map);
		}

		if(inverce)
			Invert (map);

		return map;
	}



	public static int[,] Invert(int[,] map){
		int w = map.GetLength (0);
		int h = map.GetLength (1);

		for (int x = 0; x < w; x++) {
			for (int y = 0; y < h; y++) {

				map [x, y] = (map[x, y] == 1) ? 0 : 1;
			}
		}
		return map;
	}









	//=====================================================================================
	void GenerateTexture(){

		tex = new Texture2D (width, height);

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {

				float v = (float)map [x, y];
				Color c = new Color (v,v,v,1f);

				tex.SetPixel(x,y, c);
			}
		}

		tex.filterMode = FilterMode.Point;
		tex.Apply ();
	}


	void OnGUI(){
		if(tex != null)
		GUI.DrawTexture (new Rect (10, 10, width * 2, height * 2), tex);
	}
	//=====================================================================================
}
