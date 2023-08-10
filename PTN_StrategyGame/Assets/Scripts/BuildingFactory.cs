using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingFactory: MonoBehaviour
{
    // Create a building using the provided prefab
    public Building CreateBuilding(GameObject buildingPrefab)
    {
        // Instantiate a new building object at the origin with no rotation
        GameObject buildingObject = Instantiate(buildingPrefab, Vector3.zero, Quaternion.identity);

        // Get the Building component from the instantiated building object
        Building buildingComponent = buildingObject.GetComponent<Building>();

        return buildingComponent;
    }
}
