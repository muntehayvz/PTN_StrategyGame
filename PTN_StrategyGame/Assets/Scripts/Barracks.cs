using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Barracks : IBuilding
{
    [SerializeField] private List<Soldier> soldierTypes;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] protected GameObject productImage;

    private SoldierFactory soldierFactory;
    private SoldierSpawner soldierSpawner;

    protected override void Awake()
    {
        base.Awake();

        buildingName = "Barrack";
        healthPoints = MaxHealthPoints = 100;

        InitializeSoldierTypes();

        // Creating a factory to produce soldiers using a prefab
        soldierFactory = GetComponent<SoldierFactory>();

        // Initializing the soldier spawner with soldier factory, types, and spawn points
        soldierSpawner = new SoldierSpawner(soldierFactory, soldierTypes, spawnPoints);
    }

    // Initializing the different types of soldiers available in the barracks
    private void InitializeSoldierTypes()
    {
        soldierTypes = new List<Soldier>
        {
            new Soldier(10, 10), // Soldier 1
            new Soldier(10, 5),  // Soldier 2
            new Soldier(10, 2)   // Soldier 3
        };
    }

    // Displaying basic information about the barracks
    public override void DisplayInfo()
    {
        Debug.Log(buildingName + ": HP - " + healthPoints);
    }
    // Handling mouse click on the barracks

    private void OnMouseDown()
    {
        // Handling mouse click on the barracks
        UIManager.Instance.UpdateBuildingNameAndImage(buildingName, buildImage.GetComponent<SpriteRenderer>());
        UIManager.Instance.UpdateProductionImage(productImage.GetComponentInChildren<SpriteRenderer>());

        // Handling mouse click on the barracks
        soldierSpawner.SpawnRandomSoldier();
    }
}
