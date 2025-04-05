using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;


[Serializable]
struct NoiseLayer
{
    public float scale;
    public float offset;
    public float strength;
}

[Serializable]
struct GroundTiles
{
    public Tile 
}

[Serializable]
struct OreTiles
{
    public Tile center;
    public Tile topEdge;
    public Tile bottomEdge;
    public Tile leftEdge;
    public Tile rightEdge;
    public Tile topLeftCorner;
    public Tile topRightCorner;
    public Tile bottomLeftCorner;
    public Tile bottomRightCorner;
    public Tile topLeftEdge;
    public Tile topRightEdge;
    public Tile bottomLeftEdge;
    public Tile bottomRightEdge;
    public Tile noUpEdge;
    public Tile noDownEdge;
    public Tile noLeftEdge;
    public Tile noRightEdge;
}

public class WorldGenerator : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Tilemap tilemap;
    private Dictionary<int, Tile> tiles;
    
    [SerializeField]
    private List<NoiseLayer> noiseLayers;

    public int GroundHeightAtPosition(int pos)
    {
        float height = 0;
        foreach (var layer in noiseLayers)
        {
            height += Mathf.PerlinNoise((pos + layer.offset) / layer.scale, 0) * layer.strength;
        }
        Debug.Log("Height at:" + pos + "is " + height);
        return (int)height;
    }
    
    void Start()
    {
        
        tiles = new Dictionary<int, Tile>();
        foreach (var tile in Resources.LoadAll<Tile>("Tiles"))
        {
            tiles.Add(int.Parse(tile.name.Split('_')[1]), tile);
        }
        
        for (int x = -10; x < 10; x++)
        {
            int height = GroundHeightAtPosition(x);
            Debug.Log("Height for x:" + x + " is " + height);
            tilemap.SetTile(new Vector3Int(x, height, 0), tiles[5]);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
