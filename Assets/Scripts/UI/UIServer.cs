using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIServer : MonoBehaviour, IPointerClickHandler {

    ServerInfo _serverInfo;

    public Text nameText;

	
	void Start () {
		
	}
	
	
	void Refresh () {
        nameText.text = _serverInfo.name + ": " + "[" + _serverInfo.ipAddress + ":" + _serverInfo.port + "]";

    }



    public void OnPointerClick(PointerEventData eventData)
    {
        UIServerList.i.ConnectToServer(_serverInfo);
    }



    public ServerInfo serverInfo
    {
        get { return _serverInfo; }
        set {
            _serverInfo = value;
            Refresh();
        }
    }
}
