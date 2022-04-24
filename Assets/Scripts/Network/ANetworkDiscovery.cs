using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ANetworkDiscovery : NetworkDiscovery {

    public static ANetworkDiscovery i;

    public float timeout = 5f;

    public Dictionary<ServerInfo, float> lanAdresses = new Dictionary<ServerInfo, float>();

	
	void Awake () {
        i = this;

        StartClientBroadcast();
        StartCoroutine(CleanupExpiredEntries());
	}
	
	
	void Update () {
		
	}




    void UpdateMatchInfos()
    {
        UIServerList.i.Refresh();
    }





    public void StartClientBroadcast()
    {
        if(isClient || isServer)
            StopBroadcast();
        base.Initialize();
        base.StartAsClient();
    }

    


    public void StartServerBroadcast()
    {
        if (isClient || isServer)
            StopBroadcast();
        base.Initialize();
        base.StartAsServer();
    }


    IEnumerator CleanupExpiredEntries()
    {
        while (true)
        {
            bool changed = false;
            var keys = lanAdresses.Keys.ToList();
            foreach (var key in keys)
            {
                if(lanAdresses[key] <= Time.time)
                {
                    lanAdresses.Remove(key);
                    changed = true;
                }
            }
            if (changed)
                UpdateMatchInfos();

            yield return new WaitForSeconds(timeout);
        }
    }


    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        base.OnReceivedBroadcast(fromAddress, data);

        ServerInfo info = new ServerInfo(fromAddress, data);

        if(lanAdresses.ContainsKey(info) == false)
        {
            lanAdresses.Add(info, Time.time + timeout);
            UpdateMatchInfos();
        }
        else
        {
            lanAdresses[info] = Time.time + timeout;
        }

        print("received broadcast");
    }


    
}

[System.Serializable]
public struct ServerInfo
{
    public string ipAddress;
    public int port;
    public string name;

    public ServerInfo(string address, string data)
    {
        this.ipAddress = address.Substring(address.LastIndexOf(":")+1, address.Length - (address.LastIndexOf(":") + 1));
        string portText = data.Substring(data.LastIndexOf(":") + 1, data.Length - (data.LastIndexOf(":") + 1));
        this.port = 7777;   int.TryParse(portText, out port);
        this.name = "local: " + this.ipAddress;
    }
}
