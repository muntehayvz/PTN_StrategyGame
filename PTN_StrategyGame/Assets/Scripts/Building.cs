﻿using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Building : MonoBehaviour
{
    public bool Placed { get; private set; }
    public BoundsInt area;

    [SerializeField]
    public string buildingName;

    [SerializeField]
    public GameObject buildImage;

    [SerializeField]
    public GameObject productImage;

    [SerializeField]
    public UnityEngine.Vector2 dimensions;

    [SerializeField]
    public List<Transform> spawnPoints;

    [SerializeField]
    private List<Soldier> soldierTypes;
    IAstarAI ai;


    private void Awake()
    {
       // Initializing soldier types with different attack damages
        soldierTypes = new List<Soldier>
        {
            new Soldier(10, 10), // Soldier 1
            new Soldier(10, 5),  // Soldier 2
            new Soldier(10, 2)   // Soldier 3
        };
    }

    #region Build Methods
    void Start()
    {
        ai = GetComponent<IAstarAI>();

        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = true;
        }
    }
    
    private void OnMouseDown()
    {
        Debug.Log("Bina Adı: " + buildingName);

        UIManager.Instance.UpdateBuildingNameAndImage(buildingName, buildImage.GetComponent<SpriteRenderer>());

        if(buildingName == "Barrack")
        {
            UIManager.Instance.UpdateProductionImage(productImage.GetComponentInChildren<SpriteRenderer>());
            int randomSpawn = Random.Range(0, 3); // 1, 2 veya 3 rastgele dönecek
            SpawnSoldier(randomSpawn);
        }

    }

    public bool CanBePlaced()
    {
        Vector3Int positionInt = GridBuildingSystem.instance.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        if (GridBuildingSystem.instance.CanTakeArea(areaTemp))
        {
            return true;

        }
        return false;
    }
    public void Place()
    {
        Vector3Int positionInt = GridBuildingSystem.instance.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        Placed = true;
        GridBuildingSystem.instance.TakeArea(areaTemp);
        AstarPath.active.Scan();


        BuildingFactory buildingFactory = GetComponent<BuildingFactory>();

        if (buildingName == "Barrack")
        {
            IBuilding barracks = buildingFactory.CreateBuilding(BuildingType.Barracks);
            barracks.DisplayInfo();
        }
        if (buildingName == "Power Plant")
        {
            IBuilding powerPlant = buildingFactory.CreateBuilding(BuildingType.PowerPlant);
            powerPlant.DisplayInfo();
        }

    }
    #endregion

    public void SpawnSoldier(int soldierTypeIndex)
    {
        if (Placed)
        {
            if (buildingName == "Barrack")
            {
                Transform emptySpawnPoint = FindEmptySpawnPoint();
                if (emptySpawnPoint != null)
                {
                    UnityEngine.Vector3 spawnPosition = emptySpawnPoint.position;
                    GameObject soldierPrefab = Resources.Load<GameObject>("SoldierPrefab");

                    if (soldierPrefab != null)
                    {
                        GameObject newSoldier = Instantiate(soldierPrefab, spawnPosition, UnityEngine.Quaternion.identity);
                        Soldier soldierType = soldierTypes[soldierTypeIndex];
                        newSoldier.GetComponent<SoldierController>().Initialize(soldierType);

                    }
                }
            }
        }
        
    }

    private Transform FindEmptySpawnPoint()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            // Kontrol edilen spawnPoint'te asker var mı diye kontrol edin
            bool isOccupied = IsSpawnPointOccupied(spawnPoint);

            // Eğer spawnPoint boşsa, onu döndürün
            if (!isOccupied)
            {
                return spawnPoint;
            }
        }

        return null; // Eğer hiçbir spawnPoint boş değilse null döndürün
    }

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
