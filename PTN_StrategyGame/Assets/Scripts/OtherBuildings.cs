using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherBuildings : IBuilding
{
    protected override void Awake()
    {
        //healthPoints = MaxHealthPoints = 20;
        base.Awake();
    }

    public override void DisplayInfo()
    {
        Debug.Log(buildingName + "HP - " + healthPoints);
    }

    private void OnMouseDown()
    {
        Debug.Log("Bina Adı: " + buildingName);

        UIManager.Instance.UpdateBuildingNameAndImage(buildingName, buildImage.GetComponent<SpriteRenderer>());
    }
}
