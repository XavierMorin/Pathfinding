using System;
using System.Collections.Generic;
using UnityEngine;




public class DFS : PathFinder
{
   
    private Stack<Node> stack;
   
    public override List<Vector2Int> FindPath(Vector2Int origin, Vector2Int destination)
    {
        InitializeNodeMap();
        snapShots = new List<double[,]>();
        
        stack = new Stack<Node>();
       
        nodeMap[origin.x, origin.y].visited=true;
        stack.Push(nodeMap[origin.x, origin.y]);

        while (stack.Count>0)
        {
            TakeSnapShot();

            Node curNode = stack.Pop();

            if (curNode.position == destination)
                return UnpackPath(curNode);

            // Take unvisited neighbors in order (eg. Noth, East, South, West),
            foreach (Node neighbor in GetUnvisitedNeighbors(curNode))
            {
                neighbor.visited = true;
                neighbor.parent = curNode;
                stack.Push(neighbor);
            }
        }

        return new List<Vector2Int>();
        
    }
    private void TakeSnapShot()
    {
        double[,] megaPath = new double[map.Columns, map.Rows];

        for (int i = 0; i < map.Columns; i++)
            for (int j = 0; j < map.Rows; j++)
                if(nodeMap[i,j].visited==true)
                    megaPath[i, j] = 0.5;
        
        snapShots.Add(megaPath);

    }
  
   

    
}

