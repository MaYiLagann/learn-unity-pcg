using System;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class BoardPCGManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int minimum, int maximum)
        {
            this.minimum = minimum;
            this.maximum = maximum;
        }
    }

    public int columns = 5;
    public int rows = 5;
    public GameObject exitTile;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] outerWallTiles;

    private Transform boardHolder;
    private List<Vector2> gridPositions = new List<Vector2>();
    private Transform dungeonBoardHolder;
    private List<Vector2> dungeonPositions = new List<Vector2>();

    public void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                gridPositions.Add(new Vector2(x, y));

                var toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                var instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity);

                instance.transform.SetParent(boardHolder);
            }
        }
    }

    public void AddToBoard(int horizontal, int vertical)
    {
        if (horizontal == 1)
        {
            var x = (int)Completed.Player.position.x;
            var sightX = x + 2;
            for (x += 1; x <= sightX; x++)
            {
                var y = (int)Completed.Player.position.y;
                var sightY = y + 1;
                for (y -= 1; y <= sightY; y++)
                {
                    AddTiles(new Vector2(x, y));
                }
            }
        }
        else if (horizontal == -1)
        {
            var x = (int)Completed.Player.position.x;
            var sightX = x - 2;
            for (x -= 1; x >= sightX; x--)
            {
                var y = (int)Completed.Player.position.y;
                var sightY = y + 1;
                for (y -= 1; y <= sightY; y++)
                {
                    AddTiles(new Vector2(x, y));
                }
            }
        }
        else if (vertical == 1)
        {
            var y = (int)Completed.Player.position.y;
            var sightY = y + 2;
            for (y += 1; y <= sightY; y++)
            {
                var x = (int)Completed.Player.position.x;
                var sightX = x + 1;
                for (x -= 1; x <= sightX; x++)
                {
                    AddTiles(new Vector2(x, y));
                }
            }
        }
        else if (vertical == -1)
        {
            var y = (int)Completed.Player.position.y;
            var sightY = y - 2;
            for (y -= 1; y >= sightY; y--)
            {
                var x = (int)Completed.Player.position.x;
                var sightX = x + 1;
                for (x -= 1; x <= sightX; x++)
                {
                    AddTiles(new Vector2(x, y));
                }
            }
        }
    }

    public void SetDungeonBoard(Dictionary<Vector2, TileType> dungeonTiles, int bound, Vector2 endPosition)
    {
        boardHolder.gameObject.SetActive(false);
        dungeonBoardHolder = new GameObject("Dungeon").transform;

        foreach (var tile in dungeonTiles)
        {
            var toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
            var instance = Instantiate(toInstantiate, tile.Key, Quaternion.identity);
            instance.transform.SetParent(dungeonBoardHolder);
        }

        for (var x = -1; x < bound + 1; x++)
        {
            for (var y = -1; y < bound + 1; y++)
            {
                if (dungeonTiles.ContainsKey(new Vector2(x, y)))
                    continue;

                var toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                var instance = Instantiate(toInstantiate, new Vector3(x, y, 0), Quaternion.identity);
                instance.transform.SetParent(dungeonBoardHolder);
            }
        }
    }

    public void SetWorldBoard()
    {
        Destroy(dungeonBoardHolder.gameObject);
        boardHolder.gameObject.SetActive(true);
    }

    private void AddTiles(Vector3 tileToAdd)
    {
        if (!gridPositions.Contains(tileToAdd))
        {
            gridPositions.Add(tileToAdd);

            var toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
            var instance = Instantiate(toInstantiate, tileToAdd, Quaternion.identity);
            instance.transform.SetParent(boardHolder);

            if (Random.Range(0, 3) == 1)
            {
                toInstantiate = wallTiles[Random.Range(0, wallTiles.Length)];
                instance = Instantiate(toInstantiate, tileToAdd, Quaternion.identity);
                instance.transform.SetParent(boardHolder);
            }

            if (Random.Range(0, 100) == 1)
            {
                toInstantiate = exitTile;
                instance = Instantiate(toInstantiate, tileToAdd, Quaternion.identity);
                instance.transform.SetParent(boardHolder);
            }
        }
    }
}
