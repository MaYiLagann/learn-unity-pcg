using System;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class DungeonManager : MonoBehaviour
{
    public int minBound = 0;
    public int maxBound = 0;

    [NonSerialized]
    public Dictionary<Vector2, TileType> gridPositions = new Dictionary<Vector2, TileType>();
    [NonSerialized]
    public Vector2 startPosition = Vector2.zero;
    [NonSerialized]
    public Vector2 endPosition = Vector2.zero;

    public void StartDungeon()
    {
        gridPositions.Clear();

        BuildEssentialPath();
        BuildRandomPath();
    }

    private void BuildEssentialPath()
    {
        var randomY = Random.Range(0, maxBound + 1);
        var endPath = new PathTile(TileType.Essential, Vector2.up * randomY, minBound, maxBound, gridPositions);

        startPosition = endPath.position;

        var boundTracker = 0;
        while (boundTracker < maxBound)
        {
            gridPositions.Add(endPath.position, TileType.Empty);

            if (endPath.adjacentPathTiles.Count == 0)
                break;

            var randomIndex = Random.Range(0, endPath.adjacentPathTiles.Count);
            var nextPos = endPath.adjacentPathTiles[randomIndex];
            var nextPath = new PathTile(TileType.Essential, nextPos, minBound, maxBound, gridPositions);

            if (nextPath.position.x > endPath.position.x
                || (nextPath.position.x == maxBound - 1 && Random.Range(0, 2) == 1))
                boundTracker++;

            endPath = nextPath;
        }

        if (!gridPositions.ContainsKey(endPath.position))
            gridPositions.Add(endPath.position, TileType.Empty);

        endPosition = endPath.position;
    }

    private void BuildRandomPath()
    {
        var paths = new Queue<PathTile>();

        foreach (var grid in gridPositions)
        {
            paths.Enqueue(new PathTile(TileType.Random, grid.Key, minBound, maxBound, gridPositions));
        }

        foreach (var path in paths)
        {
            if (path.adjacentPathTiles.Count != 0)
            {
                if (Random.Range(0, 5) == 1)
                {
                    BuildRandomChamber(path);
                }
            }
            else if (path.type == TileType.Random && path.adjacentPathTiles.Count > 1)
            {
                if (Random.Range(0, 5) == 1)
                {
                    var randomIndex = Random.Range(0, path.adjacentPathTiles.Count);
                    var nextPos = path.adjacentPathTiles[randomIndex];

                    if (!gridPositions.ContainsKey(nextPos))
                    {
                        var nextPath = new PathTile(TileType.Random, nextPos, minBound, maxBound, gridPositions);
                        gridPositions.Add(nextPos, TileType.Random);
                        paths.Enqueue(nextPath);
                    }
                }
            }
        }
    }

    private void BuildRandomChamber(PathTile path)
    {
        const int chamberSize = 3;

        var randomIndex = Random.Range(0, path.adjacentPathTiles.Count);
        var chamberOrigin = path.adjacentPathTiles[randomIndex];

        for (var x = (int)chamberOrigin.x; x < chamberOrigin.x + chamberSize; x++)
        {
            for (var y = (int)chamberOrigin.y; y < chamberOrigin.y + chamberSize; y++)
            {
                var chamberTilePos = new Vector2(x, y);

                if (!gridPositions.ContainsKey(chamberTilePos)
                    && chamberTilePos.x < maxBound
                    && chamberTilePos.x > 0
                    && chamberTilePos.y < maxBound
                    && chamberTilePos.y > 0)
                {
                    gridPositions.Add(chamberTilePos, TileType.Empty);
                }
            }
        }
    }
}
