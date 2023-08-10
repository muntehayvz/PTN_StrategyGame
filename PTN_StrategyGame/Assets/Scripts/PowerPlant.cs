using UnityEngine;

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
        Debug.Log(buildingName+ ": HP - " + healthPoints);
    }

    private void OnMouseDown()
    {
        Debug.Log("Bina Adı: " + buildingName);

        // Update the UI with the building's name and image
        UIController uiController = FindObjectOfType<UIController>();

        uiController.HandleBuildingSelection(buildingName, buildImage.GetComponent<SpriteRenderer>().sprite);
    }
}