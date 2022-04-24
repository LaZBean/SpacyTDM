using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class EntityMotor : MonoBehaviour {

    public Vector3Int gridPos;
    [Header("[MOVEMENT]")]
    public float speed;
    public Vector3 moveDir;

    public Vector3 velocity;

    [Header("[COLLISION]")]
    public Tilemap tilemap;



    void Awake() {

        //tilemap = AIMap.i.tilemap;
    }
	
	
	void Update () {
        Move();


        //gridPos = tilemap.WorldToCell(transform.position);
    }


    void Move()
    {
        //CheckCollision();

        velocity = moveDir * speed * Time.deltaTime;
        transform.position += velocity;
    }


    bool CheckCollision()
    {
        Vector3Int gridPos = tilemap.WorldToCell(transform.position);


        Vector3Int nextPos = tilemap.WorldToCell(transform.position + velocity);

        if (tilemap.GetTile(nextPos) == null) {
            
            Vector3Int normal = gridPos - nextPos;

            return true;
        }

        return false;
    }
}
