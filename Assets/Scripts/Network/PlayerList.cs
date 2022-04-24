using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerList : MonoBehaviour {

    public static PlayerList i;

    public List<Player> players = new List<Player>();



    [Header("[UI]")]
    public RectTransform mainPanel;

    public RectTransform listPanel;
    public RectTransform listPanelTeam1;
    public RectTransform listPanelTeam2;
    public GameObject playerPanelPrefab;



    bool _isOpen;







    void Awake(){
        i = this;
        isOpen = false;
    }

	void Start () {
		
	}


	
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isOpen = !isOpen;
        }
    }





    public Player GetPlayerFromConnectionID(int connId)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].connectionToClient.connectionId == connId)
                return players[i];
        }
        return null;
    }

    public void SetPlayerTeam(int connId, int team)
    {
        Player p = GetPlayerFromConnectionID(connId);
        if (p == null) return;
        p.team = team;
    }

    public void SetDeathPoints(int connId)
    {
        Player p = GetPlayerFromConnectionID(connId);
        if (p == null) return;
        p.death = p.death + 1;
    }


    public void SetKillPoints(int connId, int add)
    {
        Player p = GetPlayerFromConnectionID(connId);
        if (p == null) return;
        p.kills = p.kills + add;
    }









    public bool isOpen
    {
        get { return _isOpen; }
        set
        {
            _isOpen = value;
            mainPanel.gameObject.SetActive(_isOpen);
            Refresh();
        }
    }


    public void Refresh(bool p = true)
    {
        Clear();

        //players = FindObjectsOfType<Player>();//GetComponentsInChildren<Player>();

        for (int i = 0; i < players.Count; i++)
        {
            Player player = players[i];


            RectTransform rect = listPanel;
            if (player.team == 0) rect = listPanelTeam1;
            else if (player.team == 1) rect = listPanelTeam2;


            GameObject panel = (GameObject)Instantiate(playerPanelPrefab, rect);
            panel.SetActive(true);
            UIUnetPlayer uiUnetPlayer = panel.GetComponent<UIUnetPlayer>();
            uiUnetPlayer.player = player;


        }
    }


    public void Clear()
    {
        for (int i = 0; i < listPanelTeam1.childCount; i++)
        {
            Destroy(listPanelTeam1.GetChild(i).gameObject);
        }
        for (int i = 0; i < listPanelTeam2.childCount; i++)
        {
            Destroy(listPanelTeam2.GetChild(i).gameObject);
        }
        for (int i = 0; i < listPanel.childCount; i++)
        {
            Destroy(listPanel.GetChild(i).gameObject);
        }
    }
}
