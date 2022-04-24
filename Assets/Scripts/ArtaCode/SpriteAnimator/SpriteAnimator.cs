using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteAnimator : MonoBehaviour {

	[SerializeField]private Sprite[] _frames;

	public float lifetime = 1.0f;

	private SpriteRenderer _sr;
	private Image _img;

	private int curIndx = 0;
	private float timeToNextFrame;
	private float timer;

	public bool loop = true;
	private bool isPlaying = false;


	public Sprite[] frames{
		get{return _frames;}
		set{ 
			if (frames != value) {
				timer = 0;
			}
			_frames = value;
		}
	}


	void Awake() {
		_sr = GetComponent<SpriteRenderer>();
		_img = GetComponent<Image>();

		isPlaying = true;
	}
	

	void Update () {
		if (!isPlaying || frames.Length==0)
			return;

		timeToNextFrame = lifetime / (1.0f * frames.Length);

		timer -= Time.deltaTime;
		if(timer<=0){

			if ( (curIndx >= frames.Length-1) && !loop)
				isPlaying = false; 

			curIndx = (curIndx >= frames.Length-1)? 0 : curIndx+1;
			timer = timeToNextFrame;
		}

		//-|-
		if (curIndx >= frames.Length)
			curIndx = 0;


		if (_sr != null)
			_sr.sprite = frames [curIndx];
		else if (_img != null)
			_img.sprite = frames [curIndx];

	}


	public void Emit(){
		loop = false;
		isPlaying = true;
	}

	public void Stop(){
		isPlaying = false;
	}
}
