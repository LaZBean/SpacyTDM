using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextPrint : MonoBehaviour {

	public bool loop = true;
	public float timeBetweenChars = 0.1f;

    [Header("[INFO]")]
    public bool isPrinted = false;

	private Text uiText;
	private string text;
	private int dti = 0;
	private float timer;

	void Awake(){
		uiText = GetComponent<Text> ();
		text = uiText.text;
	}

	void Start () {
		
	}
	

	void Update () {

        isPrinted = (dti == -1) ? true : false;

        if (dti == -1 && !loop)
        {
            return;
        }


        uiText.text = text.Remove (dti + 1, text.Length - (dti+1));

		if (timer <= 0) {
			dti++;
            if (dti >= text.Length)
            {
                dti = -1;
            }

			timer = timeBetweenChars;
		}
		timer -= Time.deltaTime;

		//if (dti == -1 && !loop) return;

	}
}
