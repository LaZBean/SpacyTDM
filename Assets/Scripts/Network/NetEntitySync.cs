using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetEntitySync : NetworkBehaviour {

    public Human entity;
	
	void Awake () {
        if (entity == null) entity = GetComponent<Human>();
	}
	
	
	void Update () {
		
	}

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        print("[NET][ENTITY] Start Local");

        if (isLocalPlayer){
            ControlManager.i.entity = this.entity;
        }
    }




    [SyncVar]Vector3 syncPos;

    [SyncVar]Vector3 aimPos;
    [SyncVar] Vector3 inputDir;

    



    [SerializeField] Transform myTransform;
    [SerializeField] float lerpRate = 15f;


    void FixedUpdate()
    {
        TransmitPos();
        TransmitRot();
        LerpPos();
        LerpRot();
    }


    void LerpPos()
    {
        if (!isLocalPlayer){
            myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime * lerpRate);
        }
    }


    void LerpRot()
    {
        if (!isLocalPlayer)
        {
            entity.aimPos = Vector3.Lerp(entity.aimPos, aimPos, Time.deltaTime * lerpRate);
            entity.inputDir = inputDir;
            
        }
    }

    [ClientRpc]
    public void RpcTeleportTo(Vector3 pos)
    {
        myTransform.position = pos;
    }

    [Command]
    public void CmdTeleportTo(Vector3 pos)
    {
        syncPos = pos;
        RpcTeleportTo(pos);
    }


    [Command(channel = 1)]
    void CmdSendPosToServer(Vector3 pos, bool isRunning)
    {
        syncPos = pos;
        entity.isRunning = isRunning;
    }

    [Command(channel = 1)]
    void CmdSendRotToServer(Vector3 rot, Vector3 inputDir)
    {
        aimPos = rot;
        this.inputDir = inputDir;
    }




    [Client]
    void TransmitPos()
    {
        if (isLocalPlayer)
        {
            CmdSendPosToServer(myTransform.position, entity.isRunning);
        }
    }


    [Client]
    void TransmitRot()
    {
        if (isLocalPlayer)
        {
            CmdSendRotToServer(entity.aimPos, entity.inputDir);
        }
    }


}
