using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class GrassSpawner : MonoBehaviour
{
    public Tilemap grassTilemap;
    public TileBase grassTile;
    public int grassCount = 500;

    private void Start()
    {
        SpawnGrass();
    }

    private void SpawnGrass()
    {
        BoundsInt bounds = grassTilemap.cellBounds;
        TileBase[] tileArray = new TileBase[1];
        tileArray[0] = grassTile;

        for (int i = 0; i < grassCount; i++)
        {
            Vector3Int randomCell = new Vector3Int(Random.Range(bounds.x, bounds.x + bounds.size.x), Random.Range(bounds.y, bounds.y + bounds.size.y), 0);
            grassTilemap.SetTiles(new Vector3Int[] { randomCell }, tileArray);
        }
    }
}

