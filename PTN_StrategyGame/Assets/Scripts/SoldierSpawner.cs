using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Creating soldiers and manage spawn points
public class SoldierSpawner
{
    private SoldierFactory soldierFactory;
    private List<Soldier> soldierTypes;
    private List<Transform> spawnPoints;

    // Constructor method initialising the SoldierSpawner class with SoldierFactory, soldier types and spawn points
    public SoldierSpawner(SoldierFactory factory, List<Soldier> soldierTypes, List<Transform> spawnPoints)
    {
        soldierFactory = factory;
        this.soldierTypes = soldierTypes;
        this.spawnPoints = spawnPoints;
    }

    // Method to create a soldier at an empty spawn point by selecting a random soldier type
    public void SpawnRandomSoldier()
    {
        Transform emptySpawnPoint = FindEmptySpawnPoint();
        if (emptySpawnPoint != null)
        {
            Vector3 spawnPosition = emptySpawnPoint.position;
            int randomIndex = Random.Range(0, soldierTypes.Count);
            Soldier soldierType = soldierTypes[randomIndex];
            GameObject newSoldier = soldierFactory.CreateSoldier(spawnPosition, soldierType);
        }
    }

    // Method to find an empty spawn point
    private Transform FindEmptySpawnPoint()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            //Checks for troops at the controlled spawnPoint
            bool isOccupied = IsSpawnPointOccupied(spawnPoint);

            // If spawnPoint is empty, returns it
            if (!isOccupied)
            {
                return spawnPoint;
            }
        }
        // Returns null if no spawnPoint is empty
        return null; 
    }

    // Method to check if there are soldiers at a specific spawn point
    private bool IsSpawnPointOccupied(Transform spawnPoint)
    {
        Collider2D[] colliders = Physics2D.OverlapPointAll(spawnPoint.position);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Soldier")) // Eğer asker varsa true döndürün
            {
                return true;
            }
        }

        return false; // Eğer asker yoksa false döndürün
    }
}