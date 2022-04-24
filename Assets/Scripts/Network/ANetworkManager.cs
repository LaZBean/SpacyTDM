using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ANetworkManager : NetworkManager {

    public static ANetworkManager i;

    public PlayerList playerList;
    //
	
	void Awake() {
        singleton = this;
        i = this;
	}

    void Start()
    {
        
    }


    void Update () {
		
	}



    public void RegisterHandlers()
    {
        //REG HANDLERS
        //NetworkServer.RegisterHandler((short)UnetMessageType.PlayerInfo, OnServerReceivedPlayerInfo);

        NetworkServer.RegisterHandler((short)UnetMessageType.Connected, OnServerClientConnected);
        NetworkServer.RegisterHandler((short)UnetMessageType.Disconnected, OnServerClientDisconnected);
    }






    public bool hostIsActive
    {
        get { return (NetworkServer.active || this.IsClientConnected()); }
    }









    //HANDLERS
    /*void OnServerReceivedPlayerInfo(NetworkMessage netMsg)
    {
        //print("[NET][SERVER] Receiving Player Info...");
        var packet = netMsg.ReadMessage<PlayerInfo>();

        //ADD PLAYER
        UnetPlayerList.i.Add(UnetPlayer.FromPlayerInfo(netMsg.conn.connectionId, packet));
    }*/

    void OnServerClientConnected(NetworkMessage netMsg)
    {
        print("[NET][CONNECT] Client Connected");
    }

    void OnServerClientDisconnected(NetworkMessage netMsg)
    {
        NetworkServer.Destroy(netMsg.conn.playerControllers[0].gameObject);
        NetworkServer.Destroy(netMsg.conn.playerControllers[1].gameObject);
        print("[NET][SERVER] Client Disconnected");
    }














    //================================================================================================
    public override void OnServerReady(NetworkConnection conn){

        RegisterHandlers();

        base.OnServerReady(conn);
    }


    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)            //SERVER ADD PLAYER
    {
        GameObject player = Instantiate(Resources.Load("core/player"), playerList.transform) as GameObject;
        NetworkServer.AddPlayerForConnection(conn, player, 1);



        /*NetworkStartPosition[] spawners = FindObjectsOfType<NetworkStartPosition>();
        int r = Random.Range(0, spawners.Length);
        GameObject player = Instantiate(ANetworkManager.i.playerPrefab, spawners[r].transform.position, spawners[r].transform.rotation) as GameObject;
        NetworkServer.AddPlayerForConnection(NetworkServer.connections[connId], player, 0);*/

        base.OnServerAddPlayer(conn, playerControllerId);
    }


    public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
    {
        print("remove player " + conn.connectionId);
        base.OnServerRemovePlayer(conn, player);
    }



    public override void OnStartServer()            //START SERVER
    {
        base.OnStartServer();

        
    }

    public override void OnStartHost()
    {
        base.OnStartHost();
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        //SEND PLAYER INFO TO SERVER
        //print("[NET][CLIENT] Sending Player Info To A Server...");
        /*PlayerInfo pi = new PlayerInfo(conn.connectionId, GameManager.i.playerData);
        client.Send((short)UnetMessageType.PlayerInfo, pi);*/

        UIMainMenu.i.Close();
    }

    
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);

        MessagePopup.i.AddMessage("[NETWORK] Disconnect: " + conn.lastError.ToString(), (int)MessagePopup.MessageType.Error);
    }

    public override void OnStopClient()
    {
        base.OnStopClient();

        UIMainMenu.i.Open();
    }


    public override void OnClientError(NetworkConnection conn, int errorCode)
    {
        base.OnClientError(conn, errorCode);

        string errorMsg = "[NETWORK] Error: " + ((NetworkError)errorCode).ToString() + "(" + conn.lastError + ")";
        MessagePopup.i.AddMessage(errorMsg, (int)MessagePopup.MessageType.Error);
    }

    public override void OnServerError(NetworkConnection conn, int errorCode)
    {
        base.OnServerError(conn, errorCode);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);

        PlayerList.i.Refresh();
    }
}




[System.Serializable]
public enum UnetMessageType
{
    //PlayerInfo = MsgType.Highest + 2,
    Message = MsgType.Highest + 1,
    Connected = MsgType.Connect,
    Disconnected = MsgType.Disconnect,
}




/*[System.Serializable]
public class PlayerInfo : MessageBase
{
    public int connectionId;
    public PlayerData data;

    public PlayerInfo() { }
    public PlayerInfo(int connId, PlayerData data){
        this.connectionId = connId;
        this.data = data;
    }
}*/













[System.Serializable]
public class ChatMsg : MessageBase
{
    public int player;
    public string message;

    public ChatMsg() { }

    public ChatMsg(int player, string msg)
    {
        this.player = player;
        this.message = msg;
    }
}
