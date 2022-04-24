using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Entity))]
public class AI : MonoBehaviour {

    Entity entity;

    public Vector3 targetPos;
    public List<Node> pathPoints = new List<Node>();


    AIMap aiMap;

    

    void Start() {
        entity = GetComponent<Entity>();

        aiMap = AIMap.i;
    }

    void Update() {

        Tilemap map = entity.motor.tilemap;

        //print(aiMap.GetNode(map.WorldToCell(GameUtility.MouseWorldPosOnPlane(Camera.main, Vector3.forward, Vector3.zero))));

        //print(map.GetTile(map.WorldToCell(GameUtility.MouseWorldPosOnPlane(Camera.main, Vector3.forward, Vector3.zero))));


        if (Input.GetMouseButtonDown(0))
        {
            targetPos = GameUtility.MouseWorldPosOnPlane(Camera.main, Vector3.forward, Vector3.zero);
            Vector3Int gridPos = map.WorldToCell(transform.position);
            Pathfind(transform.position, targetPos);
        }





        if (pathPoints.Count != 0)
        {
            Vector3 pPos = (Vector3)pathPoints[0].pos * 0.08f + new Vector3(0.04f, 0.04f, 0);
            entity.motor.moveDir = GameUtility.DirNormalize ((pPos - transform.position).normalized);
            
            entity.aimPos = pPos;

            if (Vector2.Distance(transform.position, (Vector2)pPos) < 0.05f)
            {
                pathPoints.RemoveAt(0);
                //OnTileEnter();
            }
        }


    }





    public enum BehaviourType
    {
        None,
        Follow
    }








    void OnDrawGizmos()
    {
        /*Gizmos.color = Color.yellow;
        for (int i = 0; i < pathPoints.Count; i++){
            Gizmos.DrawCube((Vector3)pathPoints[i].pos * 0.08f +new Vector3(0.04f, 0.04f, 0), Vector3.one * 0.04f);
        }


        Gizmos.DrawCube((Vector3)entity.motor.tilemap.WorldToCell(targetPos) * 0.08f, Vector3.one * 0.04f);*/
    }


    //PATHFIND
    public void Pathfind(Vector3 start, Vector3 end)
    {

        Tilemap map = entity.motor.tilemap;
        Grid grid = map.layoutGrid;

        Vector3Int startPos = grid.WorldToCell(start);
        Vector3Int endPos = grid.WorldToCell(end);

        Node startTile = aiMap.GetNode(startPos);
        Node endTile = aiMap.GetNode(endPos);

        if (startTile == null || endTile == null) return;

        List<Node> openSet = new List<Node>();
        HashSet<Node> closeSet = new HashSet<Node>();
        openSet.Add(startTile);

        while (openSet.Count > 0)
        {
            Node curTile = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < curTile.fCost || openSet[i].fCost == curTile.fCost && openSet[i].hCost < curTile.hCost)
                {
                    curTile = openSet[i];
                }
            }

            openSet.Remove(curTile);
            closeSet.Add(curTile);

            if (curTile == endTile)
            {
                //print("path found: ");
                RetracePath(startTile, endTile);
                return;
            }


            foreach (Node neighbour in aiMap.GetNeighbours(map, curTile))
            {
                if (neighbour == null || closeSet.Contains(neighbour))
                {
                    continue;
                }

                int newMoveCostToNeighbour = curTile.gCost + aiMap.GetDistance(curTile, neighbour);
                if (newMoveCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMoveCostToNeighbour;
                    neighbour.hCost = aiMap.GetDistance(neighbour, endTile);
                    neighbour.parent = curTile;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
    }



    void RetracePath(Node start, Node end)
    {
        List<Node> path = new List<Node>();


        Node curTile = end;

        while (curTile != start)
        {
            path.Add(curTile);
            curTile = curTile.parent;
        }

        path.Add(start);
        path.Reverse();

        pathPoints = path;
    }



    
}
