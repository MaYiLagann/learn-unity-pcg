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
    }

    private void BuildEssentialPath()
    {
        var randomY = Random.Range(0, maxBound + 1);
        var ePath = new PathTile(TileType.Essential, Vector2.up * randomY, minBound, maxBound, gridPositions);

        startPosition = ePath.position;

        var boundTracker = 0;
        while (boundTracker < maxBound)
        {
            gridPositions.Add(ePath.position, TileType.Empty);

            var adjacentTileCount = ePath.adjacentPathTiles.Count;
            var randomIndex = Random.Range(0, adjacentTileCount);
            var nextEPathPos = Vector2.zero;

            if (adjacentTileCount > 0)
                nextEPathPos = ePath.adjacentPathTiles[randomIndex];
            else
                break;

            var nextEPath = new PathTile(TileType.Essential, nextEPathPos, minBound, maxBound, gridPositions);

            if (nextEPath.position.x > ePath.position.x || (nextEPath.position.x == maxBound - 1 && Random.Range(0, 2) == 1))
            {
                boundTracker++;
            }

            ePath = nextEPath;
        }

        if (!gridPositions.ContainsKey(ePath.position))
            gridPositions.Add(ePath.position, TileType.Empty);

        endPosition = ePath.position;
    }
}
