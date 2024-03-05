using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class TreeGenerator : MonoBehaviour
{
    public Tilemap treeTilemap;
    public TileBase[] treeTiles;
    public int treeCount = 100;

    private void Start()
    {
        SpawnTrees();
    }

    private void SpawnTrees()
    {
        BoundsInt bounds = treeTilemap.cellBounds;

        for (int i = 0; i < treeCount; i++)
        {
            Vector3Int randomCell = new Vector3Int(Random.Range(bounds.x, bounds.x + bounds.size.x), Random.Range(bounds.y, bounds.y + bounds.size.y), 0);
            
            TileBase randomTreeTile = treeTiles[Random.Range(0, treeTiles.Length)];

            treeTilemap.SetTile(randomCell, randomTreeTile);
        }
    }
}
