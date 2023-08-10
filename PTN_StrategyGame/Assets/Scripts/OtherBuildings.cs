using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherBuildings : IBuilding
{
    protected override void Awake()
    {
        //healthPoints = MaxHealthPoints = 20;
        base.Awake(); // Call the base class's Awake method to initialize common variables
    }

    public override void DisplayInfo()
    {
        Debug.Log(buildingName + "HP - " + healthPoints);
    }

    private void OnMouseDown()
    {
        Debug.Log("Bina Adı: " + buildingName);

        // Update the UI with the building's name and image
        UIController uiController = FindObjectOfType<UIController>();

        uiController.HandleBuildingSelection(buildingName, buildImage.GetComponent<SpriteRenderer>().sprite);
    }
}
