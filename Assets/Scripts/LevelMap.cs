using Photon.Pun;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelMap : MonoBehaviour
{
    private BoundsInt bounds;

    public BoundsInt Bounds
    {
        get { return bounds; }
        set { bounds = value; }
    }

    private TileBase[] mapTiles;

    public TileBase[] MapTiles
    {
        get { return mapTiles; }
        set { mapTiles = value; }
    }

    // Set tiles to be blocked
    public Tile[] blockedTiles;

    // Create a dictionary to keep number of columns per row
    private ConcurrentDictionary<int, int> numColRow;

    public ConcurrentDictionary<int, int> NumColRow
    {
        get { return numColRow; }
        set { numColRow = value; }
    }

    private PhotonView phoView = null;

    /// <summary>
    /// Before program start, form an array with all Tiles from the Tilemap
    /// </summary>
    void Awake()
    {
        phoView = GetComponent<PhotonView>();

        Tilemap tilemap = GetComponentInChildren<Tilemap>();
        tilemap.CompressBounds();

        bounds = tilemap.cellBounds;

        mapTiles = tilemap.GetTilesBlock(bounds);

        for (int y = 0; y < bounds.yMax + Mathf.Abs(bounds.yMin); ++y)
        {
            for (int x = 0; x < bounds.xMax + Mathf.Abs(bounds.xMin); ++x)
            {
                TileBase tile = mapTiles[y + x * bounds.size.y];
            }
        }

        numColRow = new ConcurrentDictionary<int, int>();

        // TODO: Get reference for this.
    }
}
