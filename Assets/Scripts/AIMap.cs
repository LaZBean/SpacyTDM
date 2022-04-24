using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AIMap : MonoBehaviour {

    public static AIMap i;

    public Tilemap tilemap;

    Node[,] nodes;

    public Node[] nds;

    void Awake()
    {
        i = this;

        Build();
    }

	void Start () {
		
	}

    void OnDrawGizmosSelected()
    {
        /*Gizmos.color = Color.yellow;
        
        for (int x = 0; x < nodes.GetLength(0); x++)
        {
            for (int y = 0; y < nodes.GetLength(1); y++)
            {
                Node n = GetNode(new Vector3Int(x, y, 0) + map.origin);
                if (n != null) { 
                    Gizmos.DrawCube((Vector3)n.pos * 0.08f + new Vector3(0.04f,0.04f,0), Vector3.one * 0.07f);
                }
                

            }
        }*/
    }

    void Build () {

        nodes = new Node[tilemap.size.x, tilemap.size.y];

        Vector3Int bpos = tilemap.origin;


        for (int x=0; x< tilemap.size.x; x++){
            for (int y = 0; y < tilemap.size.y; y++){
                Tile tile = tilemap.GetTile<Tile>(bpos + new Vector3Int(x, y, 0));
                if (tile != null && tile.colliderType == Tile.ColliderType.None)
                {
                    nodes[x, y] = new Node(bpos + new Vector3Int(x, y, 0));
                }
                
                
                
            }
        }
	}


    public Node GetNode(Vector3Int pos)
    {
        int x = pos.x - tilemap.origin.x;
        int y = pos.y - tilemap.origin.y;

        if (x < 0 || x >= tilemap.size.x || y < 0 || y >= tilemap.size.y) return null;

        return nodes[x, y];
    }



    public List<Node> GetNeighbours(Tilemap map, Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++){
            for (int y = -1; y <= 1; y++){

                if (x == 0 && y == 0)
                    continue;

                Vector3Int npos = node.pos + new Vector3Int(x, y, 0);

                Node n = GetNode(npos);
                if(n!=null)
                    neighbours.Add(n);
            }
        }
        return neighbours;
    }


    public int GetDistance(Node a, Node b)
    {
        int dx = Mathf.Abs(a.pos.x - b.pos.y);
        int dy = Mathf.Abs(a.pos.y - b.pos.y);

        if (dx > dy)
        {
            return 14 * dy + 10 * (dx - dy);
        }
        return 14 * dx + 10 * (dy - dx);
    }
}



[System.Serializable]
public class Node
{

    public Vector3Int pos;

    public int hCost;
    public int gCost;
    public Node parent;


    public Node() { }
    public Node(Vector3Int pos){
        this.pos = pos;
    }



    public int fCost{ get { return gCost + hCost; } }

    public bool Equal(Node n){
        return (this.pos == n.pos);
    }
}
