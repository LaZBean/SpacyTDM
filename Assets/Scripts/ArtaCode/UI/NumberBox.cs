using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[RequireComponent(typeof(Text))]
public class NumberBox : MonoBehaviour {



	private int _value = 0;
	private int _curvalue = 0;

	public int value{
		get{return _value;}
		set{_value = value; OnValueChanged ();}
	}

	public string preffix = "";
	public string suffix = "$";
	public float time = 1f;
	public Text uiText;
	public Text uiAddText;

	void Awake () {
		if(uiText==null) uiText = GetComponent<Text> ();
	}
	

	void Update () {
		uiText.text = preffix + _curvalue + suffix;
	}


	void OnValueChanged(){
		int d = _value - _curvalue;

		StopCoroutine (ShowDifference (d));
		StartCoroutine (ShowDifference(d));

		StopCoroutine (ChangeValue ());
		StartCoroutine (ChangeValue());
	}









	IEnumerator ChangeValue(){
		float delay = 1f - time / Mathf.Abs(_value - _curvalue);
		float t = 1;

		while(_curvalue != _value){

			if (t >= 1) {
				int d = ( _value - _curvalue);
				_curvalue += (int)Mathf.Sign(d);
				t = delay;
			}

			t += Time.deltaTime;

			yield return null;
		}
	}

	IEnumerator ShowDifference(int d){
		float lt = 2.0f;
		float t = 0;

		Vector2 bsize = Vector2.one;

		uiAddText.text = ((d<0)? "-" : "+") + Mathf.Abs(d);
		uiAddText.color = ((d<0)? Color.red : Color.green);

		while(t<1){
			uiAddText.rectTransform.localScale = t * bsize;
			t += Time.deltaTime * lt;

			yield return null;
		}

		while(t>0){
			uiAddText.rectTransform.localScale = t * bsize;
			t -= Time.deltaTime * lt;

			yield return null;
		}
	}
}
