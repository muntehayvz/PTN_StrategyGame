using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Manages the visual representation of the user interface based on UIModel data
public class UIView : MonoBehaviour
{
    public TextMeshProUGUI buildingNameText;
    public GameObject productionMenu;
    public SpriteRenderer buildingImageRenderer;
    public SpriteRenderer productionRenderer;

    // Updates the building-related UI elements based on the given UIModel
    public void UpdateBuildingUI(UIModel model)
    {
        buildingNameText.text = model.BuildingName;
        buildingImageRenderer.sprite = model.BuildingImage;

        // If there is a production image, shows the production menu and update the production image.
        if (model.ProductionImage != null)
        {
            ShowProductionMenu();
            productionRenderer.sprite = model.ProductionImage;
        }

        // If the building is not a "Barrack," hides the production menu and clear the production image.
        if (model.BuildingName != "Barrack")
        {
            HideProductionMenu();
            ClearProductionImage();
        }
    }

    // Shows the production menu
    public void ShowProductionMenu()
    {
        productionMenu.SetActive(true);
    }

    // Hides the production menu
    public void HideProductionMenu()
    {
        productionMenu.SetActive(false);
    }

    // Clears the displayed production image
    public void ClearProductionImage()
    {
        productionRenderer.sprite = null;
    }
}