using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessagePopup : MonoBehaviour {

    public static MessagePopup i;

    public List<Message> messages;

    public float showTime = 3f;
    public float popupTime = 0.2f;

    public RectTransform rectTransform;

    public MaskableGraphic typeImage;
    public Text msgText;
    public Color[] typeColors = new Color[] { Color.blue, Color.green, Color.yellow, Color.red };


    Vector2 size;
    Vector2 bPos;
    Vector2 tPos;


    void Awake(){
        i = this;

        size = rectTransform.sizeDelta;
        bPos = rectTransform.anchoredPosition;
        tPos = bPos + new Vector2(0, size.y);

        rectTransform.anchoredPosition = tPos;

        rectTransform.gameObject.SetActive(false);
    }


	void Start () {
		
	}
	
	
	void Update () {

	}




    IEnumerator IEPopup(){
        float t = 0;
        
        rectTransform.gameObject.SetActive(true);

        while(t <= 1f) {
            t += Time.deltaTime / popupTime;
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, bPos, t);
            yield return null;
        }

        yield return new WaitForSeconds(showTime);

        t = 0;
        while (t <= 1f)
        {
            t += Time.deltaTime / popupTime;
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, tPos, t);
            yield return null;
        }

        rectTransform.gameObject.SetActive(false);
    }





    public void SetCurrentMessage(int id){

        Message msg = GetMessage(id);
        if (msg == null) return;

        msgText.text = msg.text;
        typeImage.color = typeColors[msg.code];

        StopAllCoroutines();
        StartCoroutine(IEPopup());
    }


    public Message GetMessage(int id){
        if (id < 0 || id >= messages.Count) return null;
        return messages[id];
    }

    public void AddMessage(string msg, int code){

        messages.Add(new Message(msg, code));
        SetCurrentMessage(messages.Count-1);
    }



    

    public enum MessageType{
        Native = 0,
        Success = 1,
        Warning = 2,
        Error = 3,
    }

    [System.Serializable]
    public class Message{

        public string text;
        public int code;

        public Message(string text, int code){
            this.text = text;
            this.code = code;
        }
    }
}
