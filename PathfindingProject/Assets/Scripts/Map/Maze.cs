using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Direction { N, S, E,W}
public class Maze
{

    private int width;
    private int height;
    private int Columns;
    private int Rows;
    private Direction[,] grid;
    private System.Random random;

    private Dictionary<Direction, int> DX = new Dictionary<Direction, int>() { { Direction.E, 1 }, {Direction.W, -1 },{ Direction.N, 0 },{ Direction.S, 0 } } ;
    private Dictionary<Direction, int> DY = new Dictionary<Direction, int>() { { Direction.E, 0 }, { Direction.W, 0 }, { Direction.N, -1 }, { Direction.S, 1 } };
    private Dictionary<Direction, Direction> Opposite = new Dictionary<Direction, Direction>() { { Direction.E, Direction.W }, { Direction.W, Direction.E }, { Direction.N, Direction.S }, { Direction.S, Direction.N } };
   

    public bool [,] Generate(int Columns,int Rows)
    {
        this.Columns = Columns;
        this.Rows = Rows;
        this.width = (int)Math.Ceiling(Columns / 2.0f);
        this.height = (int)Math.Ceiling(Rows / 2.0f);
        
        random = new System.Random();
        grid = new Direction[width, height];
        CarvePassagesFrom(0, 0);

        return ConvertFormat();
    }

    private bool [,] ConvertFormat()
    {
        bool [,] newMazeFormat = new bool[Columns, Rows];



        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {

                if (grid[i, j] == Direction.S)
                {

                    if (i + 1 < width && grid[i + 1, j] != Direction.W)
                        newMazeFormat[i * 2 + 1, j * 2] = true;
                    if (i + 1 < width && j + 1 < height)
                        newMazeFormat[i * 2 + 1, j * 2 + 1] = true;
                    if (i - 1 >= 0 && j + 1 < height)
                        newMazeFormat[i * 2 - 1, j * 2 + 1] = true;
                }
                else if (j + 1 < height)
                {
                    newMazeFormat[i * 2, j * 2 + 1] = true;
                    if (i + 1 < width && grid[i + 1, j] != Direction.S)
                        newMazeFormat[i * 2 + 1, j * 2 + 1] = true;
                }

            }

        }

        return newMazeFormat;
    }

    private void CarvePassagesFrom(int cx, int cy)
    {
        Direction[] dirs = { Direction.N, Direction.S, Direction.W, Direction.E };
       
        dirs = dirs.OrderBy(x => random.Next()).ToArray();

        foreach( Direction dir in dirs)
        {
            int nx = cx + DX[dir];
            int ny = cy + DY[dir];

            if(ny>=0 && ny< this.height && nx >=0 && nx < this.width && grid[nx, ny] == 0)
            {
                grid[cx, cy] = dir;
                grid[nx, ny] = Opposite[dir];
                CarvePassagesFrom(nx, ny);

            }

        }

    }




}

