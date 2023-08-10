using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierFactory : MonoBehaviour
{
    [SerializeField] private GameObject soldierPrefab;

    // Create a new soldier GameObject at the specified spawn position with the given soldier type
    public GameObject CreateSoldier(Vector3 spawnPosition, Soldier soldierType)
    {
        // Instantiate a new soldier GameObject using the soldierPrefab at the specified position and default rotation
        GameObject newSoldier = Instantiate(soldierPrefab, spawnPosition, Quaternion.identity);

        // Initialize the new soldier's characteristics and behavior based on the provided soldier type
        newSoldier.GetComponent<SoldierController>().Initialize(soldierType);

        return newSoldier; // Return the newly created soldier GameObject
    }
}
