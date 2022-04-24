using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

    public static Player my;

    [SyncVar]public int id;
    [SyncVar(hook = "OnTeamChanged")]  public int team;
    [SyncVar(hook = "OnKillsChanged")] public int kills;
    [SyncVar(hook = "OnDeathChanged")] public int death;
    [SyncVar]public PlayerData data;




    public override void OnStartLocalPlayer()
    {
        my = this;
        CmdSendPlayerData(GameManager.i.playerData);

        base.OnStartLocalPlayer();
    }

    [Command]
    public void CmdSendPlayerData(PlayerData data)
    {
        this.data = data;
        this.id = connectionToClient.connectionId;
        RpcOnDataChanged();
    }

    [ClientRpc]
    public void RpcOnDataChanged()
    {
        PlayerList.i.Refresh();
    }


    void OnTeamChanged(int team){
        this.team = team;
        PlayerList.i.Refresh();
    }

    void OnKillsChanged(int kills){
        this.kills = kills;
        PlayerList.i.Refresh();
    }

    void OnDeathChanged(int death){
        this.death = death;
        PlayerList.i.Refresh();
    }





    void Awake()
    {
        transform.parent = PlayerList.i.transform;
    }

    void Start () {
        PlayerList.i.players.Add(this);
        PlayerList.i.Refresh();
	}
	
	
	void Update () {
		
	}





    void OnDestroy(){
        PlayerList.i.players.Remove(this);
    }
}
