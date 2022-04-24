using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

[ExecuteInEditMode]
public class FrameCounter : MonoBehaviour{

	public bool Show = true;
	public Gradient color = new Gradient(){ colorKeys = new GradientColorKey[3] {new GradientColorKey(Color.red, 0f), new GradientColorKey(Color.yellow, 0.5f), new GradientColorKey(Color.green, 1f)} };

	//CHECK FPS
	private float accum   = 0;
	private int   frames  = 0;
	private float timeleft = 0.5F;
	private float _fps;

	private float latency;


	[Header("[GUI]")]
	public GUIPos GUIPosition;
	public enum GUIPos{
		UpLeft,
		UpMiddle,
		UpRight,

		Left,
		Middle,
		Right,

		DownLeft,
		DownMiddle,
		DownRight
	}


	[Header("[UI]")]
	public Text text;

	Rect rect = new Rect(5,5,100,20);


	//================================================//
	public float fps{
		get{return _fps;}
	}

	//================================================//
	void Update(){
		TakeFps();
	}


	void OnGUI(){
		if(text != null)
			DrawUI ();
		else
			DrawGUI ();

		if(NetworkManager.singleton!=null && NetworkManager.singleton.isNetworkActive){
			ShowLatency ();
		}
	}

	//================================================//
	void DrawGUI(){
		if(!Show) return;

		float downY = Screen.height-25;
		float middY = Screen.height/2-10;

		switch(GUIPosition){
		case GUIPos.UpLeft : rect = new Rect(5, 5, 100, 20); break;
		case GUIPos.UpMiddle : rect = new Rect(Screen.width/2-50, 5, 100, 20); break;
		case GUIPos.UpRight : rect = new Rect(Screen.width-105, 5, 100, 20); break;

		case GUIPos.Left : rect = new Rect(5, middY, 100, 20); break;
		case GUIPos.Middle : rect = new Rect(Screen.width/2-50, middY, 100, 20); break;
		case GUIPos.Right : rect = new Rect(Screen.width-105, middY, 100, 20); break;

		case GUIPos.DownLeft : rect = new Rect(5, downY, 100, 20); break;
		case GUIPos.DownMiddle : rect = new Rect(Screen.width/2-50, downY, 100, 20); break;
		case GUIPos.DownRight : rect = new Rect(Screen.width-105, downY, 100, 20); break;
		}

		//SHADOW
		Rect shadowRect = rect;
		shadowRect.x += -1;
		shadowRect.y += -1;
		GUI.color = Color.black;
		GUI.Label(shadowRect, " FPS: " + _fps);

		//===
		GUI.color = GetColorFromGradient();
		GUI.Label(rect, " FPS: " + _fps);
		GUI.color = Color.white;
	}

	void DrawUI(){
		if(Show){
			text.enabled = true;
			_fps = Mathf.Round(_fps*100)/100;
			text.text = " FPS: " + _fps;
			text.color = GetColorFromGradient();
		}
		else{
			text.enabled = false;
		}
	}

	Color GetColorFromGradient(){
		return color.Evaluate(_fps * (1f/60f));
	}


	//================================================//
	public void TakeFps(){
		timeleft -= Time.deltaTime;
		accum += Time.timeScale/Time.deltaTime;
		++frames;
		
		if( timeleft <= 0.0 ){
			_fps = accum/frames;

			timeleft = 0.5f;
			accum = 0.0F;
			frames = 0;
		}
	}
		
	public void ShowLatency(){

		if (NetworkManager.singleton == null && NetworkManager.singleton.client == null)
			return;

        latency = NetworkManager.singleton.client.GetRTT();

		GUI.Label(new Rect(105,5,100,20), "Ping: " + latency + "ms");
	}


		
}
