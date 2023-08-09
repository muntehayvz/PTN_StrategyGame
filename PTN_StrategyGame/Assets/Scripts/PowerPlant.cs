﻿using UnityEngine;

public class PowerPlant : IBuilding
{
    protected override void Awake()
    {
        buildingName = "PowerPlant";
        healthPoints = MaxHealthPoints = 50;
        base.Awake();
    }

    public override void DisplayInfo()
    {
        Debug.Log("PowerPlant: HP - " + healthPoints);
    }

    private void OnMouseDown()
    {
        Debug.Log("Bina Adı: " + buildingName);

        UIManager.Instance.UpdateBuildingNameAndImage(buildingName, buildImage.GetComponent<SpriteRenderer>());
    }
}