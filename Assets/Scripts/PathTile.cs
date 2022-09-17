using System;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Empty,
    Essential,
    Random,
}

[Serializable]
public class PathTile
{
    public TileType type;

    public Vector2 position;
    public List<Vector2> adjacentPathTiles;

    public PathTile(TileType type, Vector2 position, int minBound, int maxBound, Dictionary<Vector2, TileType> currentTiles)
    {
        this.type = type;
        this.position = position;
        this.adjacentPathTiles = GetAdjacentPathTiles(minBound, maxBound, currentTiles);
    }

    public List<Vector2> GetAdjacentPathTiles(int minBound, int maxBound, Dictionary<Vector2, TileType> currentTiles)
    {
        var pathTiles = new List<Vector2>();

        var posN = new Vector2(position.x, position.y + 1);
        var posE = new Vector2(position.x + 1, position.y);
        var posS = new Vector2(position.x, position.y - 1);
        var posW = new Vector2(position.x - 1, position.y);

        if (posN.y < maxBound && !currentTiles.ContainsKey(posN))
        {
            pathTiles.Add(posN);
        }
        if (posE.x < maxBound && !currentTiles.ContainsKey(posE))
        {
            pathTiles.Add(posE);
        }
        if (posS.y > minBound && !currentTiles.ContainsKey(posS))
        {
            pathTiles.Add(posS);
        }
        if (posW.x >= minBound && !currentTiles.ContainsKey(posW))
        {
            pathTiles.Add(posW);
        }

        return pathTiles;
    }
}
