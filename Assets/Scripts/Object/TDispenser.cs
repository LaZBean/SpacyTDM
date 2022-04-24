using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TDispenser : TObject {

    public Sprite[] sprites;

    public new SpriteRenderer renderer;

    public GameObject spawnablePrefab;

    public float startSpawnTime = 0f;
    public float spawnTime = 5f;

    public TPickable item;
    TPickable _lastItem;

    [SyncVar]float spawnTimer;



    void Start () {
        spawnTimer = startSpawnTime;
    }
	
	
	void Update () {

        int frame = Mathf.RoundToInt(Mathf.Clamp((spawnTimer / spawnTime) * (sprites.Length-1), 0, sprites.Length-1));
        renderer.sprite = sprites[frame];

        if (!isServer) return;

        if (_lastItem != item){
            _lastItem = item;
            spawnTimer = spawnTime;
        }


        if (isSpawned)
        {

        }
        else
        {
            spawnTimer -= Time.deltaTime;
            if(spawnTimer <= 0)
            {
                //spawnTimer = spawnTime;
                CmdSpawn();
            }
        }

        
        
	}





    public bool isSpawned
    {
        get { return item!=null; }
    }





    [Command]
    public void CmdSpawn()
    {
        GameObject obj = (GameObject)Instantiate(spawnablePrefab, transform.position, transform.rotation);
        item = obj.GetComponent<TPickable>();
        NetworkServer.Spawn(obj);
    }
}
