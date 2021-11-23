using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Map : MonoBehaviour
{
    // Start is called before the first frame update

    public int Rows;
    public int Columns;

    public Vector2Int origin;
    public Vector2Int destination;
   
    

    [SerializeField]
    private Sprite sprite;
    private Tilemap tilemap;
    [SerializeField]
    private float cellGap;


    private bool[,] obstacles;
    private Bug bug;
    private GameObject fruit;

    
    

    public Tilemap obstaclesMap;
   

    public void Awake()
    { 
        tilemap = GetComponent<Tilemap>();
        bug = GameObject.Find("Bug").GetComponent<Bug>();
        fruit = GameObject.Find("Fruit");
        AdjustCamera();
       
    }
    public void Start()
    {
        InitializeMap();
        SetOrigin(origin);
        SetDestination(destination);      
    }
     

    public void InitializeMap()
    {
        obstacles = new bool[Columns, Rows];
        for (int i = 0; i < Columns; i++)
            for (int j = 0; j < Rows; j++)
                tilemap.SetTile(new Vector3Int(i, j, 1), TileFactory.CreateGroundTile());
    }

    public void LightUpPoints(List<Vector2Int> points, Color color)
    {
        Tile tile = CreateTile(color);
        foreach (Vector2Int point in points)
            tilemap.SetTile(new Vector3Int(point.x, point.y, 1), tile);        
    }

    public void ColorGradeMap(double [,] colorGrid, Gradient colorGradient)
    {
        for (int i = 0; i < Columns; i++)
        {
            for (int j = 0; j < Rows; j++)
            {
                if (colorGrid[i, j] > 0)
                {
                    Color color = colorGradient.Evaluate((float)colorGrid[i, j]);
                    tilemap.SetTileFlags(new Vector3Int(i, j, 1), TileFlags.None);
                    tilemap.SetColor(new Vector3Int(i, j, 1), color);
                }
            }
        }

    }
   

    public void AdjustCamera()
    {
        float lenght = Columns *20 + (Columns - 1) * cellGap;
        float height = Rows *20 + (Rows - 1) * cellGap;
        Camera.main.transform.position = new Vector3(lenght / 2.0f, height / 2.0f, -10);
        Camera.main.orthographicSize = lenght /2;
    }

    public void SetObstacles(bool [,] obstacles)
    {
        Clear();

        this.obstacles = obstacles;
        SetOrigin(RelocatePoint(origin));
        SetDestination(RelocatePoint(destination));
      
        for (int i = 0; i < Columns; i++)
            for (int j = 0; j < Rows; j++)
                if (obstacles[i, j] == true)
                    obstaclesMap.SetTile(new Vector3Int(i, j, 1), TileFactory.CreateWallTile());
    }
   

    public void Clean()
    {
        for (int i = 0; i < Columns; i++)
        {
            for (int j = 0; j < Rows; j++)
            {
                if (obstacles[i, j] == false)
                {
                    tilemap.SetTileFlags(new Vector3Int(i, j, 1), TileFlags.None);
                    tilemap.SetColor(new Vector3Int(i, j, 1), Color.white);
                    obstaclesMap.SetTile(new Vector3Int(i, j, 1), null);
                }
                
            }
        }
    }

    public void Clear()
    { 
        obstacles = new bool[Columns, Rows];
        Clean();
    }
    public bool IsPositionValid(Vector2Int position)
    {
        if ((position.x >= 0 && position.x < Columns) && (position.y >= 0 && position.y <Rows))
            return true;
        return false;
    }

    public bool IsPositionBlocked(Vector2Int position)
    {
        if(IsPositionValid(position)==true)
            return obstacles[position.x, position.y];
        return true;
    }

    public void SetOrigin(Vector2Int newStart)
    {
        if (obstacles[newStart.x, newStart.y] == true)
            return;

        origin = newStart;
        bug.SetPosition(CellToWorld(origin));
    }

    public void SetDestination(Vector2Int newDestination)
    {
        if (obstacles[newDestination.x, newDestination.y] == true)
            newDestination = RelocatePoint(newDestination);
        destination = newDestination;
        fruit.transform.position = CellToWorld(newDestination);
    }

    public void SetWall(Vector2Int position)
    {
        if (position == origin || position == destination || obstacles[position.x,position.y]==true)
            return;
       
        obstacles[position.x, position.y] = true;
        obstaclesMap.SetTile(new Vector3Int(position.x, position.y, 1), TileFactory.CreateWallTile());
    }

    public void SetFloor(Vector2Int position)
    {
        obstacles[position.x, position.y] = false;
        obstaclesMap.SetTile(new Vector3Int(position.x, position.y, 1), null);
    }
    public Vector2Int WorldToCell(Vector3 position)
    {
        GridLayout gridLayout = transform.GetComponent<GridLayout>();
        Vector3Int pos = gridLayout.WorldToCell(position);
        return new Vector2Int(pos.x,pos.y);
    }

    public Vector3 CellToWorld(Vector2Int position)
    {
        GridLayout gridLayout = transform.GetComponent<GridLayout>();
        Vector3 pos = gridLayout.CellToWorld(new Vector3Int(position.x,position.y,0));
        return pos;
    }
    private Vector2Int RelocatePoint(Vector2Int point)
    {
        if (obstacles[point.x, point.y])
            for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
                    if (obstacles[point.x + i, point.y + j] == false && IsPositionValid(new Vector2Int(point.x+i,point.y+j)))
                        return new Vector2Int(point.x + 1, point.y + j);
        return point;
    }
    private Tile CreateTile(Color color)
    {
        Tile tile = ScriptableObject.CreateInstance<Tile>();
        tile.sprite = sprite;
        tile.color = color;
        return tile;
    }
}
