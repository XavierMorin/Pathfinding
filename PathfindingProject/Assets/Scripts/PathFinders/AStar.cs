using System.Collections.Generic;
using UnityEngine;

public class AStar : PathFinder
{
    private class AStarNode : PathFinder.Node
    {
       
        public double f = -1;
        public double g = -1;
        public double h = -1;
       

        public AStarNode(Vector2Int position):base(position) { }
        public AStarNode(double f, double g, double h, Vector2Int position, Node parent)
        {
            this.f = f;
            this.g = g;
            this.h = h;
            this.position = position;
            this.parent = parent;
        }
    }

   
    private PriorityQueue<AStarNode> openList;
    private Heuristic heuristic = new ManhattanDistance();
    public override List<Vector2Int> FindPath(Vector2Int startPosition, Vector2Int endPosition)
    { 
        InitializeNodeMap();
        AStarNode startNode = new AStarNode(0, 0, 0, startPosition, null);
        nodeMap[startPosition.x, startPosition.y] = startNode;
        openList = new PriorityQueue<AStarNode>(startNode, 0);

        snapShots = new List<double[,]>();
        
        while (!openList.isEmpty())
        {
            AStarNode curNode=openList.pop();
            curNode.visited = true;

            foreach(AStarNode neighbor in GetUnvisitedNeighbors(curNode))
            {
                double gNew = curNode.g + 0.1;
                double hNew = heuristic.CalculateH(endPosition, neighbor.position)/2;
                double fNew = gNew + hNew;

                if (neighbor.f == -1 || neighbor.f > fNew)
                {
                    openList.push(neighbor, fNew);
                    neighbor.parent = curNode;
                    neighbor.f = fNew;
                    neighbor.h = hNew;
                    neighbor.g = gNew;
                }

                TakeSnapShot();

                if (neighbor.position == endPosition)
                    return UnpackPath(neighbor);
                
            }            
        }
        return null;
    }


    private void TakeSnapShot()
    {
        snapShots.Add(new double[map.Columns, map.Rows]);
        for (int i = 0; i < map.Columns; i++)
            for (int j = 0; j < map.Rows; j++)
                snapShots[snapShots.Count - 1][i, j] = ((AStarNode)nodeMap[i, j]).f;
    }
   
    protected override void InitializeNodeMap()
    {
        nodeMap = new AStarNode[map.Columns, map.Rows];
        for (int i = 0; i < map.Columns; i++)
            for (int j = 0; j < map.Rows; j++)
                nodeMap[i, j] = new AStarNode(new Vector2Int(i, j));   
    }



}
