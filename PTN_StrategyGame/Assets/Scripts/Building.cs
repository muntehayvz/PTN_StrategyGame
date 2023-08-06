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
}
