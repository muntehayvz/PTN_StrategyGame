using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    [SerializeField]
    private int width, height;

    [SerializeField]
    private Tile tilePrefab;

    private Transform gridContainer;

    private void Start()
    {
        gridContainer = new GameObject("Grid Container").transform;
        gridContainer.parent = transform; // GridManager'ın alt nesnesi olarak ayarlanır.

        GenerateGrid();
    }

    void GenerateGrid()
    {
        float halfTileSize = 0.5f;

        for (int i = 0; i < width * 2; i++)
        {
            for (int j = 0; j < height * 2; j++)
            {
                var spawnedTile = Instantiate(tilePrefab, new Vector3(i * halfTileSize, j * halfTileSize), Quaternion.identity);
                spawnedTile.name = $"Tile {i} {j}";
                spawnedTile.transform.parent = gridContainer; // Oluşturulan Tile nesnesi, alt nesne olarak ayarlanır.

                var isOffset = (i % 2 == 0 && j % 2 != 0) || (i % 2 != 0 && j % 2 == 0);
                //spawnedTile.Init(isOffset);
            }
        }

        float targetOrthoSize = Mathf.Max((float)width, (float)height) * halfTileSize;
        float targetX = ((float)width - 1) * halfTileSize;
        float targetY = ((float)height - 1) * halfTileSize;
    }
}
