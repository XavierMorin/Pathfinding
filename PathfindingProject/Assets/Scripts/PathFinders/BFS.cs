using System;
using System.Collections.Generic;
using UnityEngine;

class BFS : PathFinder
{
      
    private Queue<Node> queue;
   
    public override List<Vector2Int> FindPath(Vector2Int origin, Vector2Int destination)
    {
        InitializeNodeMap();
        snapShots = new List<double[,]>();
        queue = new Queue<Node>();
        // Put the start node in the queue
       
        nodeMap[origin.x, origin.y].visited = true;
        queue.Enqueue(nodeMap[origin.x, origin.y]);

        // While there is node to be handled in the queue
        while (queue.Count>0)
        {
            // Handle the node in the front of the lineand get unvisited neighbors
            Node curNode = queue.Dequeue();
            FillMegaPath();
            // Terminate if the goal is reached
            if (curNode.position == destination)
                return UnpackPath(curNode);

            // Take neighbors, set its parent, mark as visited, and add to the queue
            foreach (Node neighbor in GetUnvisitedNeighbors(curNode))
            {
                neighbor.visited = true;
                neighbor.parent = curNode;
                queue.Enqueue(neighbor);
            }
        }
        return new List<Vector2Int>();
    }
  

    private void FillMegaPath()
    {
        double[,] megaPath = new double[map.Columns, map.Rows];

        for (int i = 0; i < map.Columns; i++)
            for (int j = 0; j < map.Rows; j++)
                if (nodeMap[i, j].visited == true)
                    megaPath[i, j] = 0.5;

        snapShots.Add(megaPath);
    }

   

    
}

