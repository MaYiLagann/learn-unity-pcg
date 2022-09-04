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
    private Dictionary<Vector2, Vector2> gridPositions = new Dictionary<Vector2, Vector2>();

    public void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                gridPositions.Add(new Vector2(x, y), new Vector2(x, y));

                var toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                var instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity);

                instance.transform.SetParent(boardHolder);
            }
        }
    }
}
