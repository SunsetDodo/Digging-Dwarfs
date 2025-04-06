using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
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
    public Tile leftRightSides;
    public Tile topBottomSides;
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
    
    [SerializeField] private List<NoiseLayer> heightNoiseLayers;
    [SerializeField] private List<NoiseLayer> oresNoiseLayers;
    
    [SerializeField]
    private List<OreTiles> ores;
    [SerializeField]
    private List<String> oreNames;

    [SerializeField] private GameObject player;


    [SerializeField] private Tile grassTile;
    [SerializeField] private Tile leftSlopeTile;
    [SerializeField] private Tile leftSlopeCornerTile;
    [SerializeField] private Tile rightSlopeTile;
    [SerializeField] private Tile rightSlopeCornerTile;
    [SerializeField] private Tile groundTile;
    [SerializeField] private Tile stoneTopGrassTile;
    [SerializeField] private Tile stoneTopLeftGrassTile;
    [SerializeField] private Tile stoneTopLeftCornerTile;
    [SerializeField] private Tile stoneTopRightGrassTile;
    [SerializeField] private Tile stoneTopRightCornerTile;
    [SerializeField] private Tile stoneTile;

    private Dictionary<int, int> groundHeights  = new Dictionary<int, int>();
    
    private Dictionary<Vector2Int, bool> orePositions = new Dictionary<Vector2Int, bool>();
    
    private int GroundHeightAtPosition(int pos)
    {
        var height = heightNoiseLayers.Sum(layer => (double)Mathf.PerlinNoise((pos + layer.offset) / layer.scale, 0) * layer.strength);
        return (int)height;
    }
    
    private bool IsOre(Vector2Int pos)
    {
        if (orePositions.TryGetValue(pos, out var ore)) return ore;
        var height = heightNoiseLayers.Sum(layer => Mathf.PerlinNoise((pos.x + layer.offset) / layer.scale, (pos.y + layer.offset * 300) / layer.scale) * layer.strength);
        var max = heightNoiseLayers.Max(layer => layer.strength);
        orePositions.Add(pos, height > max * 0.6f);
        
        return orePositions[pos];
    }

    private void CalculateGroundHeights()
    {
        for (var x = -100; x <= 100; x++)
        {
            groundHeights.Add(x, GroundHeightAtPosition(x));
        }
    }
    
    
    private void Start()
    {
        CalculateGroundHeights();
        
        for (var x = -100; x <= 100; x++)
        {
            if (x is -100 or 100)
            {
                tilemap.SetTile(new Vector3Int(x, groundHeights[x] + 3, 0), grassTile);
                tilemap.SetTile(new Vector3Int(x, groundHeights[x] + 2, 0), groundTile);
                tilemap.SetTile(new Vector3Int(x, groundHeights[x] + 1, 0), groundTile);
                tilemap.SetTile(new Vector3Int(x, groundHeights[x], 0), stoneTopGrassTile);
            }
            else
            {
                var height = groundHeights[x];
                var leftHeight = groundHeights[x - 1];
                var rightHeight = groundHeights[x + 1];
                
                
                if (height == leftHeight + 1 && height == rightHeight)
                {
                    tilemap.SetTile(new Vector3Int(x, height + 4, 0), rightSlopeTile);
                    tilemap.SetTile(new Vector3Int(x, height + 3, 0), rightSlopeCornerTile);
                    tilemap.SetTile(new Vector3Int(x, height + 2, 0), groundTile);
                    tilemap.SetTile(new Vector3Int(x, height + 1, 0), stoneTopLeftGrassTile);
                    tilemap.SetTile(new Vector3Int(x, height + 0, 0), stoneTopLeftCornerTile);
                }
                else if (height == leftHeight - 1 && height == rightHeight)
                {
                    tilemap.SetTile(new Vector3Int(x, height + 4, 0), grassTile);
                    tilemap.SetTile(new Vector3Int(x, height + 3, 0), groundTile);
                    tilemap.SetTile(new Vector3Int(x, height + 2, 0), groundTile);
                    tilemap.SetTile(new Vector3Int(x, height + 1, 0), stoneTopGrassTile);
                    tilemap.SetTile(new Vector3Int(x, height + 0, 0), stoneTile);
                }
                else if (height == leftHeight && height == rightHeight + 1)
                {
                    tilemap.SetTile(new Vector3Int(x, height + 4, 0), leftSlopeTile);
                    tilemap.SetTile(new Vector3Int(x, height + 3, 0), leftSlopeCornerTile);
                    tilemap.SetTile(new Vector3Int(x, height + 2, 0), groundTile);
                    tilemap.SetTile(new Vector3Int(x, height + 1, 0), stoneTopRightGrassTile);
                    tilemap.SetTile(new Vector3Int(x, height + 0, 0), stoneTopRightCornerTile);
                }
                else if (height == leftHeight && height == rightHeight - 1)
                {
                    tilemap.SetTile(new Vector3Int(x, height + 4, 0), grassTile);
                    tilemap.SetTile(new Vector3Int(x, height + 3, 0), groundTile);
                    tilemap.SetTile(new Vector3Int(x, height + 2, 0), groundTile);
                    tilemap.SetTile(new Vector3Int(x, height + 1, 0), stoneTopGrassTile);
                    tilemap.SetTile(new Vector3Int(x, height + 0, 0), stoneTile);
                }
                else
                {
                    tilemap.SetTile(new Vector3Int(x, height + 4, 0), grassTile);
                    tilemap.SetTile(new Vector3Int(x, height + 3, 0), groundTile);
                    tilemap.SetTile(new Vector3Int(x, height + 2, 0), groundTile);
                    tilemap.SetTile(new Vector3Int(x, height + 1, 0), stoneTopGrassTile);
                    tilemap.SetTile(new Vector3Int(x, height + 0, 0), stoneTile);
                }

            }
            
            
            for (var y = -5000; y < GroundHeightAtPosition(x); y++)
            {
                if (IsOre(new Vector2Int(x, y)))
                {
                    switch (y)
                    {
                        case >= -50:
                            tilemap.SetTile(new Vector3Int(x, y, 0), ores[0].center);
                            break;
                        case >= -200:
                            tilemap.SetTile(new Vector3Int(x, y, 0), ores[1].center);
                            break;
                        case >= -1000:
                            tilemap.SetTile(new Vector3Int(x, y, 0), ores[2].center);
                            break;
                        case >= -2500:
                            tilemap.SetTile(new Vector3Int(x, y, 0), ores[3].center);
                            break;
                        default:
                            tilemap.SetTile(new Vector3Int(x, y, 0), ores[4].center);
                            break;
                    }
                }
                else
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), stoneTile);
                }
                                
            }
        }
        
        player.transform.position = new Vector3(0, GroundHeightAtPosition(0) + 6, 0);
        player.SetActive(true);
    }

    public int OreIndex(Vector3Int pos)
    {
        var tile = tilemap.GetTile(pos);
        Debug.Log(tile.name);
        for (var i = 0; i < ores.Count; i++)
        {
            Debug.Log(ores[i].center.name == tile.name);
            if (tile.name == ores[i].center.name)
                return i;
        }
        return -1;
    }
}
