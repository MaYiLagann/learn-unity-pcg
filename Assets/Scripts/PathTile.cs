using System;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public enum TileType
{
    Empty,
    Essential,
    Random,
    Chest,
}

public enum DirectionType
{
    None = 0,
    North = 1,
    East = 2,
    South = 4,
    West = 8,
    All = 15,
}

[Serializable]
public class PathTile
{
    public TileType type;
    public Vector2 position;

    private List<Vector2> adjacentPathTiles;

    public PathTile(TileType type, Vector2 position, int minBound, int maxBound, Dictionary<Vector2, TileType> currentTiles, DirectionType direction = DirectionType.All)
    {
        this.type = type;
        this.position = position;
        this.adjacentPathTiles = CreateAdjacentPathTiles(minBound, maxBound, currentTiles, direction);
    }

    private List<Vector2> CreateAdjacentPathTiles(int minBound, int maxBound, Dictionary<Vector2, TileType> currentTiles, DirectionType direction)
    {
        var pathTiles = new List<Vector2>();

        var posN = new Vector2(position.x, position.y + 1);
        var posE = new Vector2(position.x + 1, position.y);
        var posS = new Vector2(position.x, position.y - 1);
        var posW = new Vector2(position.x - 1, position.y);

        if (direction.HasFlag(DirectionType.North) && posN.y < maxBound && !currentTiles.ContainsKey(posN))
        {
            pathTiles.Add(posN);
        }
        if (direction.HasFlag(DirectionType.East) && posE.x < maxBound && !currentTiles.ContainsKey(posE))
        {
            pathTiles.Add(posE);
        }
        if (direction.HasFlag(DirectionType.South) && posS.y > minBound && !currentTiles.ContainsKey(posS))
        {
            pathTiles.Add(posS);
        }
        if (direction.HasFlag(DirectionType.West) && posW.x >= minBound && !currentTiles.ContainsKey(posW))
        {
            pathTiles.Add(posW);
        }

        return pathTiles;
    }

    public bool TryGetRandomPathTile(out Vector2 position)
    {
        position = Vector2.zero;

        if (adjacentPathTiles.Count == 0)
            return false;

        position = adjacentPathTiles[Random.Range(0, adjacentPathTiles.Count)];

        return true;
    }
}
