using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileFactory : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Sprite> groundSprites;
    public List<Sprite> wallSprites;
    private static List<Sprite> staticGroundSprites;
    private static List<Sprite> staticWallSprites;

    void Awake()
    {
        staticGroundSprites = groundSprites;
        staticWallSprites = wallSprites;
    }

    public static Tile CreateGroundTile()
    {
        Tile tile = ScriptableObject.CreateInstance<Tile>();
        int RandomInt = Random.Range(0, staticGroundSprites.Count);
        tile.sprite = staticGroundSprites[RandomInt];
        return tile;
    }
    public static Tile CreateWallTile()
    {
        Tile tile = ScriptableObject.CreateInstance<Tile>();
        int RandomInt = Random.Range(0, staticWallSprites.Count);
        tile.sprite = staticWallSprites[RandomInt];
        Debug.Log(staticWallSprites[RandomInt].name);
        return tile;
    }
   
}
