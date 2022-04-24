using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorText : MonoBehaviour {


	Text uiText;
	string text;
	string curText;

	public float timeToNext = 0.2f;
	float timer;

	public int curCharSize = 50;
	int curChar = 0;
	int lastChar;

	void Start () {
		uiText = GetComponent<Text> ();
		text = uiText.text;

		colors = new Color[text.Length];
	}

	int lastSize;
	public int curSize;

	void Update () {

		timer -= Time.deltaTime;
		if (timer <= 0) {
			//RandomColors ();

			colors [curChar] = GameUtility.RandomColor ();

			lastChar = curChar;
			if (curChar + 1 >= text.Length) {
				curChar = 0;
			} else {
				curChar += 1;
			}


			timer = timeToNext;

			curSize = uiText.fontSize;
			lastSize = curCharSize;
		}

		curSize = (int)(uiText.fontSize + (1-(timer / timeToNext)) * curCharSize);
		lastSize = (int)(uiText.fontSize + (timer / timeToNext) * curCharSize);


		NextColors();



	}


	Color[] colors;

	void NextColors(){
		curText = "";
		for (int i = 0; i < text.Length; i++) {
			if (i == curChar) {
				curText += ("<size=" + curSize + ">" + "<color=#" + GameUtility.ColorToHex ((Color32)colors [i]) + ">" + text [i] + "</color>" + "</size>");
			}else if(i == lastChar){
				curText += ("<size=" + lastSize + ">" + "<color=#" + GameUtility.ColorToHex ((Color32)colors [i]) + ">" + text [i] + "</color>" + "</size>");
			}
			else {
				//colors [i] = Color.yellow;
				curText += ("<color=#" + GameUtility.ColorToHex ((Color32)colors [i]) + ">" + text [i] + "</color>");
			}
		}
		uiText.text = curText;
	}




	void RandomColors(){
		curText = "";
		for (int i = 0; i < text.Length; i++) {
			Color c = GameUtility.RandomColor ();
			curText += ("<color=#" + GameUtility.ColorToHex((Color32)c) + ">" + text [i] + "</color>");
		}
		uiText.text = curText;
	}
}
