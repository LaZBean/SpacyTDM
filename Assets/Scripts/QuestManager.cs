using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class QuestManager : NetworkBehaviour {

    public static QuestManager i;

    [SyncVar] public int score1;
    [SyncVar] public int score2;
    [SyncVar] public float questTimer;

    public Text questInfoText;

    public Button questTeamButton1;
    public Button questTeamButton2;

    public Button questStartButton;
    public Text questSpawnInfo;


    [SyncVar(hook = "OnStageChanged")]public int stage;   //(hook = "OnStageChanged")

    public enum QuestStage
    {
        Begin = 0,
        Play = 1,
        End = 2
    }

   


    void Awake()
    {
        i = this;
    }

	void Start () {

        if (isServer)
        {
            stage = (int)QuestStage.Begin;


            

            
        }
    }





    public override void OnStartClient()
    {
        base.OnStartClient();
        stage = (int)QuestStage.Begin;
    }

    /*public override bool OnSerialize(NetworkWriter writer, bool initialState)
    { 
        return base.OnSerialize(writer, initialState);
    }*/





    void Update () {


        if (stage == (int)QuestStage.End || stage == (int)QuestStage.Play){
            questInfoText.text = "<color=#a52a2aff>" + score1 + "</color>   -   <color=#0000ffff>" + score2 + "</color> " +
                             "\n\n" + Mathf.FloorToInt(questTimer / 60) + ":" + Mathf.FloorToInt(questTimer % 60);
        }
        else{
            questInfoText.text = "";
        }
        



        if (isServer)
        {
            questTimer -= Time.deltaTime;


            if (questTimer <= 0 && stage == (int)QuestStage.Play)
            {
                stage = (int)QuestStage.End;
            }
        }





        questTeamButton1.gameObject.SetActive((stage == (int)QuestStage.Begin || stage == (int)QuestStage.Play) && (Human.my != null && Human.my.isDead));
        questTeamButton2.gameObject.SetActive((stage == (int)QuestStage.Begin || stage == (int)QuestStage.Play) && (Human.my != null && Human.my.isDead));
        questStartButton.gameObject.SetActive(isServer && stage == (int)QuestStage.Begin);

        questSpawnInfo.gameObject.SetActive((stage == (int)QuestStage.Play && (Human.my != null && Human.my.isDead)));
        if (questSpawnInfo.gameObject.active){
            if (Input.GetKeyDown(KeyCode.F))
            {
                RespawnPlayer();
            }
        }
    }

    void Refresh()
    {

    }







    public void RespawnPlayer()
    {
        Human.my.AskForRespawn();
    }


    public void ClientSetTeam(int team)
    {
        ControlManager.i.entity.ClientSetTeam(team);
    }

    
    public void ClientSpawnPlayer()
    {
        if (isClient && stage == (int)QuestStage.Play)
        {
             //CmdSpawnPlayer(ANetworkManager.i.client.connection.connectionId);
        }
    }

    



    public void StartMatch()
    {
        stage = (int)QuestStage.Play;
        StartQuest();
    }

    public void OnStageChanged(int stage)
    {
        this.stage = stage;

        PlayerList.i.isOpen = (stage == (int)QuestStage.Begin || (stage == (int)QuestStage.End));
    }




    public void StartQuest()
    {
        questTimer = 10 * 60;
        score1 = 50;
        score2 = 50;
    }

    public void EndQuest()
    {

    }


    void IEEndQuest()
    {

    }



    //[ClientRpc] on clients with seerver`s vars
    //[Command] on server with client vars

}
