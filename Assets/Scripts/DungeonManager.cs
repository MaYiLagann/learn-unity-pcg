using System;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class DungeonManager : MonoBehaviour
{
    public int minBound = 1;
    public int maxBound = 1;
    public int minChamberSize = 1;
    public int maxChamberSize = 1;

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

            if (!endPath.TryGetRandomPathTile(out var nextPos))
                break;

            var nextPath = new PathTile(TileType.Essential, nextPos, minBound, maxBound, gridPositions);

            if (nextPath.position.x > endPath.position.x
                || (nextPath.position.x >= maxBound - 1 && Random.Range(0, 2) == 1))
                boundTracker++;

            endPath = nextPath;
        }

        if (!gridPositions.ContainsKey(endPath.position))
            gridPositions.Add(endPath.position, TileType.Empty);

        endPosition = endPath.position;
    }

    private void BuildRandomPath()
    {
        var paths = new List<PathTile>();

        foreach (var grid in gridPositions)
        {
            paths.Add(new PathTile(TileType.Random, grid.Key, minBound, maxBound, gridPositions));
        }

        var i = 0;
        while (i < paths.Count)
        {
            var path = paths[i];

            if (Random.Range(0, 5) == 1)
            {
                if (!path.TryGetRandomPathTile(out var nextPos))
                    continue;

                if (gridPositions.ContainsKey(nextPos))
                    continue;

                var nextPath = new PathTile(TileType.Random, nextPos, minBound, maxBound, gridPositions);
                gridPositions.Add(nextPos, TileType.Random);
                paths.Add(nextPath);
            }

            if (Random.Range(0, 10) == 1)
            {
                BuildRandomChamber(path);
            }

            i++;
        }
    }

    private void BuildRandomChamber(PathTile path)
    {
        var chamberSize = new Vector2(Random.Range(minChamberSize, maxChamberSize), Random.Range(minChamberSize, maxChamberSize));

        if (!path.TryGetRandomPathTile(out var chamberOrigin))
            return;

        for (var x = (int)chamberOrigin.x; x < chamberOrigin.x + chamberSize.x; x++)
        {
            for (var y = (int)chamberOrigin.y; y < chamberOrigin.y + chamberSize.y; y++)
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
