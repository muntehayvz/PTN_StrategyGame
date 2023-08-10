using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField]
    private int width, height;

    [SerializeField]
    private GameObject tilePrefab;

    private Transform gridContainer;

    private void Start()
    {
        gridContainer = new GameObject("Grid Container").transform; // Create a container for the grid tiles
        gridContainer.parent = transform;

        GenerateGrid(); // Generate the grid of tiles
    }

    // Generates a grid of tiles based on specified dimensions and prefab
    void GenerateGrid()
    {
        float halfTileSize = 0.27f;

        for (int i = 0; i < width * 2; i++)
        {
            for (int j = 0; j < height * 2; j++)
            {
                var position = new Vector3(i * halfTileSize, j * halfTileSize); // Calculate tile position
                var spawnedTile = Instantiate(tilePrefab, position, Quaternion.identity); // Instantiate a tile
                spawnedTile.name = $"Tile {i} {j}"; // Set tile name
                spawnedTile.transform.parent = gridContainer; // Set tile as a child of the container
            }
        }
    }
}
