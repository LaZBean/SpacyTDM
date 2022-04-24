using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UIUnetPlayer : MonoBehaviour {

    Player _player;

    public Text nameText;
    public Image iconImg;
    public Text scoreText;
    public Text pingText;
    public Image colorImg;
    public Image teamColorImg;

    void Start () {
		
	}

    void Update()
    {
        if(_player != null && _player.name != "")
        {
            /*if (!ANetworkManager.i.isNetworkActive) return;
            byte error;
            int ping = NetworkTransport.GetCurrentRTT(0, player.id, out error);
            pingText.text = ping + " ms";*/

        }
        else{
            PlayerList.i.Refresh();
        }
    }
	
	
	public void Refresh () {
        if (player == null) return;

        nameText.text = "[" + player.id + "] " + player.data.name;
        scoreText.text = player.kills + " / " + player.death;

        iconImg.sprite = SkinLib.i.skinIcons[player.data.skin];




        teamColorImg.color = (player.team == 0) ? new Color(1.0f, 0.5f, 0.5f, 0.5f) : new Color(0.5f, 0.5f, 1.0f, 0.5f);

        pingText.text = "- ms";
    }


    public Player player
    {
        get { return _player; }
        set { _player = value;
            Refresh();
        }
    }
}
