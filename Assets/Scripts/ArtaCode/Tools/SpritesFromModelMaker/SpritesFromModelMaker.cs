using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SpritesFromModelMaker : MonoBehaviour {

	public int width = 32;
	public int height = 64;

	public string imgName = "bakedSprite";
	public bool forceSave = true;

	[Space(20)]
	public int framesCount = 4;
	public int sidesCount = 8;
	public string[] animations;

    [Space(20)]
    public float rotationOffset = 180.0f;

    [Space(20)]
	public Animator animator;
	public GameObject target;
	public Camera renderCamera;
	public RenderTexture renderTexture;

	[Space(20)]
	[ContextMenuItem("[Save]", "SaveImageToPNG")]
	public Texture2D outputTexture;

	Texture2D curTexture;


	void Start () {
		
	}
	
	int n = 0;
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			animator.speed = 0;
			animator.Play (animations[1], 0, n*1f/framesCount);
			n++;
			print (n*1f/framesCount + " ddd");
		}
	}



	[ContextMenu("--> Bake Sprite <--")]
	public void Create(){

		StartCoroutine (ICreate());
	}

	IEnumerator ICreate(){
		
		outputTexture = new Texture2D (width*(framesCount*sidesCount), height*(animations.Length) );
		animator.speed = 0;

		for (int a = 0; a < animations.Length; a++) {	//animations

			for (int f = 0; f < framesCount; f++) {	//frames
				



				for (int s = 0; s < sidesCount; s++) {	//sides
					
					animator.Play (animations[a], 0, f*1f/framesCount);
					
					target.transform.eulerAngles = new Vector3 (0, rotationOffset + (s) * 360f / sidesCount, 0); //(-s+1)
					yield return new WaitForSeconds(0.0f);


					BakeSprite ();


					//yield return StartCoroutine ();


					int x = width * ((f * sidesCount) + s);  //	//width*f
					int y = height*(a);		//height*(a*sidesCount+s);//

					print ("x: "+x/width + "\ty: "+y/height + "\ta: " + animations[a] + "\tyy: " + y);

					outputTexture.SetPixels (x, y, curTexture.width, curTexture.height, curTexture.GetPixels());

				}
			}
		}

		outputTexture.Apply ();

		if (forceSave)
			SaveImageToPNG ();
	}



	void BakeSprite(){
		//Debug.Log ("Bake sprite...");

		//renderTexture.width = width;
		//renderTexture.height = height;
		renderCamera.targetTexture = renderTexture;

		RenderTexture currentRT = RenderTexture.active;
		renderCamera.targetTexture.Release ();
		RenderTexture.active = renderCamera.targetTexture;
		renderCamera.Render ();

		curTexture = new Texture2D (renderCamera.targetTexture.width, renderCamera.targetTexture.height, TextureFormat.ARGB32, false);
		curTexture.ReadPixels (new Rect(0,0,renderCamera.targetTexture.width,renderCamera.targetTexture.height), 0,0);
		curTexture.Apply ();

		RenderTexture.active = currentRT;

		//yield return new WaitForSeconds(0.0f);
	}



	public void SaveImageToPNG(){
		if (outputTexture == null) {
			Debug.Log ("Output image is NULL");
			return;
		}

		if (string.IsNullOrEmpty (imgName)) {
			imgName = target.name + "Sprite";
			Debug.Log ("Image name is NULL! \n new name is: [" + imgName +"]");
			return;
		}

		//print ("Save Img...");

		IOManager.SaveTexture2DToPNG (outputTexture, "", imgName + ".png", true);
	}
}
