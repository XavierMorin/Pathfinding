using System;
using UnityEngine;

class ManhattanDistance : Heuristic
{
    public override double CalculateH(Vector2Int origin, Vector2Int destination)
    {
        int xDistance = Math.Abs(origin.x - destination.x);
        int yDistance = Math.Abs(origin.y - destination.y);

        return xDistance + yDistance;

    }
}

