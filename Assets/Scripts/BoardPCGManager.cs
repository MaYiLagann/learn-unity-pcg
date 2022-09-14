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
    public GameObject[] floorTiles;

    private Transform boardHolder;
    private List<Vector2> gridPositions = new List<Vector2>();

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

    private void AddTiles(Vector3 tileToAdd)
    {
        if (!gridPositions.Contains(tileToAdd))
        {
            gridPositions.Add(tileToAdd);
            var toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
            var instance = Instantiate(toInstantiate, new Vector3(tileToAdd.x, tileToAdd.y, 0f), Quaternion.identity);
            instance.transform.SetParent(boardHolder);
        }
    }
}
