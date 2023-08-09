using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingFactory: MonoBehaviour
{
    public Building CreateBuilding(GameObject buildingPrefab)
    {
        GameObject buildingObject = Instantiate(buildingPrefab, Vector3.zero, Quaternion.identity);
        Building buildingComponent = buildingObject.GetComponent<Building>();
        return buildingComponent;
    }
}
