using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;

public class HSVColorPicker : MonoBehaviour{



	void Start () {
		GenerateHue();
		UpdateImage();
		UpdateUI();
	}

	void Update () {
		textRGB.text = (int)(color.r*255) + ", " + (int)(color.g*255) + ", " + (int)(color.b*255);
	}

	public Text textRGB;

	private Color _color;

	[Range(0,1)]public float Hue;
	[Range(0,1)]public float Saturation;
	[Range(0,1)]public float Value;

	//UpdateImage
	public RawImage Image;
	private Texture2D texImg;

	//HUE
	public RawImage HueImage;
	private Texture2D texHue;

	//Update UI
	public RectTransform SVHandle;
	public RectTransform HueHandle;

	/*[System.Serializable]
	public class ColorEvent : UnityEvent<Color>{}
	public ColorEvent onChangedColor;*/

	public UnityEvent onChangedColor;

	public FilterMode texturesFilter = FilterMode.Point;

	public Color color{
		get{return _color;}
		set{_color = value;

			Color.RGBToHSV (color, out Hue, out Saturation, out Value);

			UpdateUI ();
			UpdateImage();
		}
	}



	void UpdateImage(){

		if (texImg == null) {
			RectTransform rt = Image.transform as RectTransform;
			texImg = new Texture2D ((int)rt.sizeDelta.x, (int)rt.sizeDelta.y);
		}

		float s = 0;
		float v = 0;

		for(int x=0; x<texImg.width; x++){
			
			s = x * 1.0f/texImg.width;

			for(int y=0; y<texImg.height; y++){

				v = y * 1.0f/texImg.height;

				Color c = Color.HSVToRGB(Hue, s, v);

				texImg.SetPixel(x, y, c);
			}
		}


		texImg.filterMode = texturesFilter;
		texImg.Apply();

		Image.texture = texImg;
	}






	void GenerateHue(){
		RectTransform rt = HueImage.transform as RectTransform;
		texHue = new Texture2D((int)rt.sizeDelta.x, (int)rt.sizeDelta.y);
		float h = 0;

		for(int x=0; x<texHue.height; x++){
			for(int y=0; y<texHue.width; y++){
				
				Color c = Color.HSVToRGB(h, 1f, 1f);

				texHue.SetPixel(y, x, c);

			}
			h += 1.0f/texHue.height;
		}

		texHue.filterMode = texturesFilter;
		texHue.Apply();

		HueImage.texture = texHue;
	}
		
	//SET Saturation & Value
	public void SetSaturationAndValue(){
		RectTransform rt = Image.transform.parent as RectTransform;

		Canvas canvas = GetComponentInParent<Canvas> ();
		Vector2 mPos =  RectTransformUtility.PixelAdjustPoint(Input.mousePosition, Image.transform, canvas);

		Camera cam = canvas.worldCamera;
		Vector3[] corners = new Vector3[4];
		Image.rectTransform.GetWorldCorners(corners);
		Rect newRect = new Rect(corners[0], corners[2]-corners[0]);
		Vector3 rectPos = cam.WorldToScreenPoint(corners[0]);

		float x = (int)((mPos.x - rectPos.x));
		float y = (int)(mPos.y - rectPos.y);
		//print (x + " " + y);
		//int x = (int)(Input.mousePosition.x + (rt.rect.x + rt.position.x));
		//int y = (int)(Input.mousePosition.y + (rt.rect.y + rt.position.y));

		if(0 <= x && x <= rt.sizeDelta.x){
			Saturation = x * (1/rt.sizeDelta.x);
		}
		if(0 <= y && y <= rt.sizeDelta.y){
			Value = y * (1/rt.sizeDelta.y);
		}

		UpdateUI();
	}



	//SET HUE
	public void SetHue(){
		RectTransform rt = HueImage.transform.parent as RectTransform;

		Canvas canvas = GetComponentInParent<Canvas> ();
		Vector2 mPos =  RectTransformUtility.PixelAdjustPoint(Input.mousePosition, Image.transform, canvas);

		Camera cam = canvas.worldCamera;
		Vector3[] corners = new Vector3[4];
		Image.rectTransform.GetWorldCorners(corners);
		Rect newRect = new Rect(corners[0], corners[2]-corners[0]);
		Vector3 rectPos = cam.WorldToScreenPoint(corners[0]);

		float x = (int)(mPos.x - rectPos.x);
		float y = (int)(mPos.y - rectPos.y);

		//int x = (int)(Input.mousePosition.x - (rt.rect.x + rt.position.x));
		//int y = (int)(Input.mousePosition.y - (rt.rect.y + rt.position.y));

		if(0 <= y && y <= rt.sizeDelta.y){
			Hue = y * (1/rt.sizeDelta.y);
		}

		UpdateUI();
		UpdateImage();
	}




	void UpdateUI(){

		_color = Color.HSVToRGB(Hue, Saturation, Value);

		RectTransform rh = HueImage.transform as RectTransform;
		HueHandle.anchoredPosition = new Vector2(HueHandle.anchoredPosition.x, (rh.rect.y + Hue*rh.sizeDelta.y));

		RectTransform rsv = Image.transform as RectTransform;
		SVHandle.anchoredPosition = new Vector2((rsv.rect.x + Saturation * rsv.sizeDelta.x), (rsv.rect.y + Value * rsv.sizeDelta.y));
	
		onChangedColor.Invoke ();
	}
}




	
