using System;
using UnityEngine;

public enum BuildingType
{
    Barracks,
    PowerPlant
}

public class BuildingFactory : MonoBehaviour
{
    public IBuilding CreateBuilding(BuildingType type)
    {
        switch (type)
        {
            case BuildingType.Barracks:
                return new GameObject("Barracks").AddComponent<Barracks>();
            case BuildingType.PowerPlant:
                return new GameObject("PowerPlant").AddComponent<PowerPlant>();
            default:
                throw new ArgumentOutOfRangeException("type", type, "Invalid building type");
        }
    }
}