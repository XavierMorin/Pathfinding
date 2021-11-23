using UnityEngine;
class MazeGenerator : MonoBehaviour
{
    private Maze maze;
    private Map map;

    private void Start()
    {
        map = GameObject.Find("Map").GetComponent<Map>();
        maze = new Maze();
    }

    public void SetMaze(Maze newMaze)
    {
        maze = newMaze;
    }
    public void GenerateMaze()
    {
        bool [,] obstacles = maze.Generate(map.Columns, map.Rows);
        map.SetObstacles(obstacles);
    }
}

