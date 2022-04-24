using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICustomText : MonoBehaviour {


	[SerializeField]private string _text;

	[Header("<Chars>")]
	public string chars = "";
	public Sprite[] charsSprites = new Sprite[0];

	[Header("<UI>")]
	public float offset = 0;
	public RectTransform panel;

	private List<Image> charsImages = new List<Image>();




	public string text{
		get{return _text;}
		set{
			if (_text != value) {
				_text = value;
				Refresh ();
			}
		}
		
	}



	void Start () {
		Refresh ();
	}
	

	void Update () {
		
	}




	public void Refresh(){
		
		Clear ();

		float nextPosX = 0;

		for (int i = 0; i < _text.Length; i++) {
			
			for (int c = 0; c < chars.Length; c++) {
				if (_text [i] == chars[c]) {

					GameObject charObj = new GameObject(""+_text[i]);
					Image charImg = charObj.AddComponent<Image> ();

					Sprite charSprite = charsSprites [c];
					charImg.sprite = charSprite;

					charImg.rectTransform.SetParent(panel);
					charImg.rectTransform.pivot = new Vector2 (0f, 1f);
					charImg.rectTransform.anchorMin = new Vector2 (0, 1);
					charImg.rectTransform.anchorMax = new Vector2 (1, 0);
					charImg.rectTransform.anchoredPosition = new Vector2 (nextPosX, 0);
					charImg.rectTransform.localScale = new Vector3 (1, 1, 1);
					charImg.SetNativeSize ();

					nextPosX += charSprite.rect.width + offset;

					charsImages.Add (charImg);

					break;
				}
			}
		}
	}


	public void Clear(){
		for (int i = 0; i < charsImages.Count; i++) {
			Destroy (charsImages [i]);
		}
		charsImages.Clear();
	}
}
