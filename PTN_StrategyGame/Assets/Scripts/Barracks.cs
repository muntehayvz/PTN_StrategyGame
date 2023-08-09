using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Barracks : IBuilding
{
    [SerializeField] private List<Soldier> soldierTypes;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] protected GameObject productImage;

    protected override void Awake()
    {
        soldierTypes = new List<Soldier>
        {
            new Soldier(10, 10), // Soldier 1
            new Soldier(10, 5),  // Soldier 2
            new Soldier(10, 2)   // Soldier 3
        };
        buildingName = "Barrack";
        healthPoints = MaxHealthPoints = 100;
        base.Awake();
    }

    public override void DisplayInfo()
    {
        Debug.Log("Barracks: HP - " + healthPoints);
    }

    private void OnMouseDown()
    {
        Debug.Log("Bina Adı: " + buildingName);

        UIManager.Instance.UpdateBuildingNameAndImage(buildingName, buildImage.GetComponent<SpriteRenderer>());
        UIManager.Instance.UpdateProductionImage(productImage.GetComponentInChildren<SpriteRenderer>());
        int randomSpawn = Random.Range(0, 3); // 1, 2 veya 3 rastgele dönecek
        SpawnSoldier(randomSpawn);

    }

    public void SpawnSoldier(int soldierTypeIndex)
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
