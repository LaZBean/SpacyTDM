using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
[CreateAssetMenu]
public class ATile : Tile {

    [Header("[ADVANCED TILE]")]
    public Vector3Int pos;


    public int hCost;
    public int gCost;

    public int fCost
    {
        get{return gCost + hCost;}
    }

    public ATile parent;



    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        this.pos = position;
        return base.StartUp(position, tilemap, go);
    }

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        base.RefreshTile(position, tilemap);
    }







    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
    }


    public override bool GetTileAnimationData(Vector3Int position, ITilemap tilemap, ref TileAnimationData tileAnimationData)
    {
        return base.GetTileAnimationData(position, tilemap, ref tileAnimationData);
    }

    
}
