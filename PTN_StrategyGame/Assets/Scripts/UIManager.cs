using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI buildingNameText;
    public SpriteRenderer buildingImageRenderer;
    public SpriteRenderer productionRenderer;
    [SerializeField] private GameObject productionMenu;

    private static UIManager instance;
    public static UIManager Instance
    {
        // Get the UIManager instance if not assigned
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }

    private void Start()
    {
        productionMenu.SetActive(false);
        buildingNameText.text = "Building Name";
        buildingImageRenderer.sprite = null;
        productionRenderer.sprite = null;
    }

    // Update the building name and image in the UI
    public void UpdateBuildingNameAndImage(string buildingName, SpriteRenderer buildingImage)
    {
        // Update building name and image if provided
        if (buildingName != null && buildingImage != null)
        {
            buildingNameText.text = buildingName;
            buildingImageRenderer.sprite = buildingImage.sprite;
        }

        // Activate production menu if the building is a "Barrack," otherwise deactivate it
        if (buildingName == "Barrack")
        {
            productionMenu.SetActive(true);
        }
        else
        {
            productionMenu.SetActive(false);
        }
    }

    // Update the production image in the UI
    public void UpdateProductionImage(SpriteRenderer production)
    {
        if (productionRenderer != null)
        {
            productionRenderer.sprite = production.sprite;
        }
    }

    // Clear the production image in the UI
    public void ClearProductionImage()
    {
        productionRenderer.sprite = null;
    }

    // Update the UI with soldier information
    public void UpdateSoldierInfo(string soldierName)
    {
        buildingNameText.text = soldierName;
        buildingImageRenderer.sprite = null;
    }

}
