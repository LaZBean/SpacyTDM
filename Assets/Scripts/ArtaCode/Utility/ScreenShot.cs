using UnityEngine;
using System.Collections;

public class ScreenShot : MonoBehaviour {

	public Camera camera;

	void Start () {
		if(camera == null)
			camera = GetComponent<Camera> ();
	}

	public int width = 640;
	public int height = 420;

	void Update () {
		if (Input.GetKeyDown(KeyCode.F)) {
			
			RenderTexture rt = new RenderTexture(width, height, 24);
			camera.targetTexture = rt;
			Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);
			camera.Render();
			RenderTexture.active = rt;
			screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
			camera.targetTexture = null;
			RenderTexture.active = null; // JC: added to avoid errors
			Destroy(rt);
			byte[] bytes = screenShot.EncodeToPNG();
			string filename = ScreenShotName(width, height);
			System.IO.File.WriteAllBytes(filename, bytes);
		
			Debug.Log(string.Format("Took screenshot to: {0}", filename));

		}

	}















	public static string ScreenShotName(int width, int height) {
		return string.Format("{0}/screen_{1}x{2}_{3}.png", 
			Application.dataPath, 
			width, height, 
			System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
	}
}
