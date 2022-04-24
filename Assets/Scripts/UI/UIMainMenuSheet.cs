using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenuSheet : MonoBehaviour {

    public RectTransform rectTransform;
    public float hideSpeed = 2f;

    bool _isHide = true;
	
	void Awake () {
        rectTransform.gameObject.SetActive(false);
    }
	
	
	void Update () {
		
	}



    public bool isHide
    {
        get { return _isHide; }
        set {
            if (_isHide == value) return;
            _isHide = value;

            if (!_isHide)    StartCoroutine(IEShow());
            else            StartCoroutine(IEHide());
        }
    }

    


    IEnumerator IEShow()
    {
        float t = 0;
        Vector2 bSize = rectTransform.sizeDelta;
        Vector2 bPos = new Vector2(bSize.x * 2, 0);

        rectTransform.anchoredPosition = bPos;
        rectTransform.gameObject.SetActive(true);

        while (t <= 1f)
        {
            t += Time.deltaTime * hideSpeed;
            rectTransform.anchoredPosition = Vector2.Lerp(bPos, new Vector2(0,0), t);
            yield return null;
        }

    }

    IEnumerator IEHide()
    {
        float t = 0;
        Vector2 bSize = rectTransform.sizeDelta;
        Vector2 bPos = new Vector2(bSize.x * 2, 0);

        rectTransform.anchoredPosition = new Vector2(0, 0);

        while (t <= 1f)
        {
            t += Time.deltaTime * hideSpeed;
            rectTransform.anchoredPosition = Vector2.Lerp(new Vector2(0, 0), bPos, t);
            yield return null;
        }

        rectTransform.gameObject.SetActive(false);
    }
}
