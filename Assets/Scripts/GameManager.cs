using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour {

    public static GameManager i;

    public UIMainMenu menuUI;

    public PlayerData playerData;

    public SettingsData settingsData;


    void Awake()
    {
        i = this;

        LoadSettings();
        
    }

	void Start () {
        if (!menuUI.gameObject.activeSelf)
            menuUI.gameObject.SetActive(true);


        ApplySettings();
        if (playerData.name == "") playerData.name = "Nameless";

        UIMainMenu.i.Init();
    }
	
	
	void Update () {
		
	}




    public void MPHost()
    {
        ANetworkManager.singleton.StartHost();
        ANetworkDiscovery.i.StartServerBroadcast();
    }

    public void MPConnect(string ip, int port = 7777)
    {
        ANetworkManager.singleton.networkPort = port;
        ANetworkManager.singleton.networkAddress = ip;
        ANetworkManager.singleton.StartClient();
    }

    public void MPStopClient()
    {
        /*ANetworkManager.singleton.client.Disconnect();
        ANetworkManager.singleton.client.Shutdown();
        ANetworkManager.singleton.client = null;
        NetworkTransport.Shutdown();*/

        ANetworkManager.singleton.StopClient();
        
    }

    public void MPStopHost()
    {
        MPStopClient();
        
        ANetworkManager.singleton.StopHost();
        ANetworkDiscovery.i.StartClientBroadcast();
    }









    public void LoadSettings()
    {
        if (IOManager.FileExist("", "settings.dat")){
            settingsData = (SettingsData)IOManager.LoadData("", "settings.dat");
        }

        if (IOManager.FileExist("", "player.dat")){
            playerData = (PlayerData)IOManager.LoadData("", "player.dat");
        }
    }



    public void SaveSettings()
    {
        IOManager.SaveData(settingsData, "", "settings.dat");
        IOManager.SaveData(playerData, "", "player.dat");
    }






    public void ApplySettings()
    {
        CursorManager.i.cursorPreset = MiscLib.i.cursorPresets[settingsData.cursorPresetID];
        AudioListener.volume = settingsData.volume;
        ControlManager.i.smoothInput = settingsData.smoothControl;
    }





    public void Quit()
    {
        Application.Quit();
    }


    void OnApplicationQuit()
    {
        SaveSettings();
    }

}



[System.Serializable]
public class PlayerData
{
    public string name;
    public int skin = 0;
    public int sex = 0;
    public int skinColor = 0;
    public int skinHair= 0;
    public int skinHelmet = 0;

    public PlayerData(){}

}


[System.Serializable]
public class SettingsData
{
    public int cursorPresetID;
    public float volume;
    public bool smoothControl;
    public string ip;
    

    public SettingsData() { }

}
