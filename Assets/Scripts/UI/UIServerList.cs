using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIServerList : MonoBehaviour {

    public static UIServerList i;

    public GameObject serverPanelPrefab;
    public RectTransform rectHolder;

	void Awake () {
        i = this;
	}

    void Start()
    {
        Refresh();
    }
	
	
	public void Refresh() {
        Clear();

        List<ServerInfo> servers = ANetworkDiscovery.i.lanAdresses.Keys.ToList();
        
        int i = 0;
        foreach (ServerInfo server in servers)
        {
            GameObject obj = Instantiate(serverPanelPrefab, rectHolder);
            obj.GetComponent<UIServer>().serverInfo = server;
            i++;
        }
    }


    public void ConnectToServer(ServerInfo info)
    {
        print("connect to " + info.ipAddress);
        GameManager.i.MPConnect(info.ipAddress, info.port);
    }


    void Clear()
    {
        for(int i=0; i<rectHolder.childCount; i++){
            Destroy(rectHolder.GetChild(i).gameObject);
        }
    }
}
