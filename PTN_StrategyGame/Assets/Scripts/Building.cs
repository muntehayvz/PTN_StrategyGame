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


    #region Build Methods
    void Start()
    {
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
            SpawnSoldier();
        }
        else
        {
            UIManager.Instance.ClearProductionImage();
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
        if(buildingName != "Soldier")
        {
            GridBuildingSystem.instance.TakeArea(areaTemp);
            AstarPath.active.Scan();
        }
        else
        {
            GridBuildingSystem.instance.ClearArea();
        }
        
    }
    #endregion

    public void SpawnSoldier()
    {
        if (buildingName == "Barrack")
        {
            Transform emptySpawnPoint = FindEmptySpawnPoint();
            if (emptySpawnPoint != null)
            {
                UnityEngine.Vector3 spawnPosition = emptySpawnPoint.position;
                GameObject soldierPrefab = Resources.Load<GameObject>("SoldierPrefab");
                Instantiate(soldierPrefab, spawnPosition, UnityEngine.Quaternion.identity);
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
