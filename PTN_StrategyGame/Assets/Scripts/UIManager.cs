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

    public void UpdateBuildingNameAndImage(string buildingName, SpriteRenderer buildingImage)
    {
        if (buildingName != null && buildingImage != null)
        {
            buildingNameText.text = buildingName;
            buildingImageRenderer.sprite = buildingImage.sprite;
        }

        if(buildingName == "Barrack")
        {
            productionMenu.SetActive(true);
        }
        else
        {
            productionMenu.SetActive(false);
        }
    }

    public void UpdateProductionImage(SpriteRenderer production)
    {
        if (productionRenderer != null)
        {
            productionRenderer.sprite = production.sprite;
        }
    }
    public void ClearProductionImage()
    {
        productionRenderer.sprite = null;
    }
}
