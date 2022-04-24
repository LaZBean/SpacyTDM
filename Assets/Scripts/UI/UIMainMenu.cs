using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour {

    public static UIMainMenu i;

    public int startSheet = 1;
    int curSheet = -1;

    public UIMainMenuSheet[] sheets;

    public RectTransform rectTransform;

    [Header("* Animation")]
    public Image background;
    public float backMoveSpeed = 1.0f;

    public float backX;
    public Vector2 backStartSize;

    [Header("* Multiplayer")]
    public InputField ipField;
    public Button hostButton;
    public Button connectButton;
    public Button stopButton;
    public Button disconnectButton;

    [Header("* Settings")]
    public InputField playerNameField;

    public Toggle smoothControlToggle;

    public Slider volumeSlider;

    public Image skinImage;
    public int curSkin = 0;

    public Image cursorImage;
    public int curCursor = 0;


    bool isInitialize = false;


    void Awake(){
        i = this;
    }

    void Start () {

        SetCurrentSheet(startSheet);

        backStartSize = background.rectTransform.sizeDelta;
    }

    public void Init()
    {
        LoadSettings();

        isInitialize = true;
    }



	
	
	void Update () {
        backX = (backX + Time.deltaTime * backMoveSpeed) % (backStartSize.x*1.5f);
        background.rectTransform.sizeDelta = backStartSize + new Vector2(backX, 0);



        bool noConnection = (ANetworkManager.singleton.client == null || 
                            ANetworkManager.singleton.client.connection == null || 
                            ANetworkManager.singleton.client.connection.connectionId == -1);

        connectButton.gameObject.SetActive(noConnection);
        hostButton.interactable = (noConnection);
        stopButton.gameObject.SetActive(!noConnection);

        bool isServer = (ANetworkManager.i.hostIsActive);
        disconnectButton.gameObject.SetActive(isServer);
    }






    void LoadSettings()
    {
        playerNameField.text = GameManager.i.playerData.name;

        curSkin = GameManager.i.playerData.skin-1;
        ChangeSkin();



        volumeSlider.value = GameManager.i.settingsData.volume;
        smoothControlToggle.isOn = GameManager.i.settingsData.smoothControl;

        curCursor = GameManager.i.settingsData.cursorPresetID-1;
        ChangeCursor();

        if(GameManager.i.settingsData.ip != "")
            ipField.text = GameManager.i.settingsData.ip;
    }


    public void GameApplySettings()
    {
        if (!isInitialize) return;

        GameManager.i.playerData.name = playerNameField.text;
        GameManager.i.playerData.skin = curSkin;

        GameManager.i.settingsData.cursorPresetID = curCursor;
        GameManager.i.settingsData.volume = volumeSlider.value;
        GameManager.i.settingsData.smoothControl = smoothControlToggle.isOn;
        GameManager.i.settingsData.ip = ipField.text;


        GameManager.i.SaveSettings();
        GameManager.i.ApplySettings();
    }







    public void ChangeSkin()
    {
        curSkin = ++curSkin % SkinLib.i.skinAvatars.Length;
        skinImage.sprite = SkinLib.i.skinAvatars[curSkin];
    }


    public void ChangeCursor()
    {
        curCursor = ++curCursor % MiscLib.i.cursorPresets.Length;
        cursorImage.sprite = MiscLib.i.cursorPresets[curCursor].normal;
    }
















    public void MPHost()
    {
        GameManager.i.MPHost();
    }

    public void MPConnect()
    {
        GameManager.i.MPConnect(ipField.text);
    }

    public void MPStopClient()
    {
        GameManager.i.MPStopHost();
    }


    

    public void GameQuit()
    {
        GameManager.i.Quit();
    }












    public void SetCurrentSheet(int n)
    {
        curSheet = n;

        for(int i=0; i<sheets.Length; i++)
        {
            if (curSheet == i) sheets[i].isHide = false;
            else sheets[i].isHide = true;
        }
    }


    public void Close()
    {
        rectTransform.gameObject.SetActive(false);
    }

    public void Open()
    {
        rectTransform.gameObject.SetActive(true);
    }
}
