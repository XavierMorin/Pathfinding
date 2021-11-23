using System.Collections.Generic;
using UnityEngine;

public class PathFinder
{
    protected Map map;
    protected Node[,] nodeMap;

    //SnapShots of the progression of the pathfinding algorithm -> used to display on screen 
    protected List<double[,]> snapShots;

    protected class Node
    {
        public Node parent = null;
        public Vector2Int position;
        public bool visited = false;
        public Node() { }
        public Node(Vector2Int position)
        {
            this.position = position;
        }
    }

    public PathFinder()
    {
        map = GameObject.Find("Map").GetComponent<Map>();
    }
    public virtual List<Vector2Int> FindPath(Vector2Int origin, Vector2Int destination) 
    {
        return null;
    }
    public virtual List<double[,]> GetSnapShots()
    {
        return snapShots;
    }

    protected List<Vector2Int> UnpackPath(Node finalNode)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Node curNode = finalNode;
        while (curNode != null)
        {
            path.Insert(0, curNode.position);
            curNode = curNode.parent;
        }
        return path;
    }


    protected virtual void InitializeNodeMap()
    {
        nodeMap = new Node[map.Columns, map.Rows];
        for (int i = 0; i < map.Columns; i++)
            for (int j = 0; j < map.Rows; j++)
                nodeMap[i, j] = new Node(new Vector2Int(i, j));
    }
    protected List<Node> GetUnvisitedNeighbors(Node curNode)
    {
        List<Node> neighbors = new List<Node>();
        int x = curNode.position.x;
        int y = curNode.position.y;

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i != 0 && j != 0)//Skips diagonal neighbors
                    continue;

                Vector2Int neighborPosition = new Vector2Int(i + x, j + y);

                if (map.IsPositionBlocked(neighborPosition) == true)
                    continue;

                if (nodeMap[i + x, j + y].visited == false)
                    neighbors.Add(nodeMap[i + x, j + y]);

            }

        }
        return neighbors;
    }
}



