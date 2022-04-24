using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CursorManager : MonoBehaviour {

   
    public static CursorManager i;

	public RectTransform cursorTransform;
    public Image cursorImg;

    public Gradient cursorGradient;
    public float gradientScale = 0.2f;
    public float cursorAngle = 360f;

    public CursorPreset cursorPreset;

    float animTimer;


	void Awake() {
		i = this;


        Cursor.visible = false;
    }
	
	void Start(){
		
	}
	
	
	void Update () {
		if (cursorTransform == null || cursorImg == null)
			return;

        Vector2 canvasSize = cursorTransform.GetComponentInParent<CanvasScaler>().referenceResolution;


        Vector2 cpos = (Vector2)Input.mousePosition;
        cpos.x = (cpos.x / Screen.width) * cursorTransform.sizeDelta.x;
        cpos.y = (cpos.y / Screen.height) * cursorTransform.sizeDelta.y;


        

        cursorImg.rectTransform.anchoredPosition = cpos;

        if ((EventSystem.current.IsPointerOverGameObject()))
        {
            cursorImg.sprite = cursorPreset.hover;

            //cursorImg.color = Color.white;
            //cursorImg.rectTransform.eulerAngles = Vector3.zero;
        }
        else
        {
            cursorImg.sprite = cursorPreset.normal;

            //cursorImg.color = cursorGradient.Evaluate(animTimer % 1f);
            //animTimer += Time.deltaTime * gradientScale;

            //cursorImg.rectTransform.eulerAngles = new Vector3(0, 0, animTimer * cursorAngle);
        }
        cursorImg.SetNativeSize();

    }
}
