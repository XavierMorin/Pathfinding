using System;
using System.Collections.Generic;
using UnityEngine;


class Dijkstra : PathFinder
{

    protected class DijkstraNode : PathFinder.Node
    {
        public float distance = Mathf.Infinity;
        public DijkstraNode(Vector2Int position) : base(position) { }
    }
   
    private PriorityQueue<DijkstraNode> queue;
    public override List<Vector2Int> FindPath(Vector2Int origin, Vector2Int destination)
    {
        InitializeNodeMap();
        snapShots = new List<double[,]>();

        DijkstraNode start = (DijkstraNode)nodeMap[origin.x, origin.y];
        // Distance to the root itself is zero
        start.distance = 0;
        queue = new PriorityQueue<DijkstraNode>(start, 0);
        
        while (!queue.isEmpty())
        {
            DijkstraNode curNode = queue.pop();
            
            // Fetch next closest node
            curNode.visited = true;  // Mark as discovered
            TakeSnapShot();

            // Iterate over unvisited neighbors
            foreach (DijkstraNode neighbor in GetUnvisitedNeighbors(curNode))
            {
                // Update minimal distance to neighbor
                // Note: distance between to adjacent node is constant and equal 1 in our grid
                float minDistance = Mathf.Min(neighbor.distance, curNode.distance + 1);
                if (minDistance != neighbor.distance)
                {
                    neighbor.distance = minDistance;  // update mininmal distance
                    neighbor.parent = curNode;        // update best parent

                    // Change queue priority of the neighbor since it have became closer.
                    if (queue.has(neighbor) == true) 
                        queue.setPriority(neighbor, minDistance);
                }

                // Add neighbor to the queue for further visiting.
                if (queue.has(neighbor) == false) 
                    queue.push(neighbor, neighbor.distance);

                if (neighbor.position == destination)
                    return UnpackPath(nodeMap[destination.x, destination.y]);
            }
        }
        return UnpackPath(nodeMap[destination.x,destination.y]);
    }

   

    private void TakeSnapShot()
    {
        double[,] snapShot = new double[map.Columns, map.Rows];
        for (int i = 0; i < map.Columns; i++)
            for (int j = 0; j < map.Rows; j++)
                if (nodeMap[i, j].visited == true)
                    snapShot[i, j] = ((DijkstraNode)nodeMap[i, j]).distance;
        snapShots.Add(snapShot);
    }

   
    protected override void InitializeNodeMap()
    {
        nodeMap = new DijkstraNode[map.Columns, map.Rows];
        for (int i = 0; i < map.Columns; i++)
            for (int j = 0; j < map.Rows; j++)
                nodeMap[i, j] = new DijkstraNode(new Vector2Int(i, j));
    }
   
}

