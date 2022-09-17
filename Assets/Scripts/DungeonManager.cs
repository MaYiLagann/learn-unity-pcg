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
}
