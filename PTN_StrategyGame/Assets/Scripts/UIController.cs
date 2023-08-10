using UnityEngine;

// Controls the user interface interactions and manages the UIModel and UIView
public class UIController : MonoBehaviour
{
    private UIModel model;
    private UIView view;

    private void Start()
    {
        // Initializes the model and view references
        model = new UIModel();
        view = GetComponent<UIView>();
        model.BuildingName = "Building Name";
        // Hides the production menu and update the initial building UI
        view.HideProductionMenu();
        view.UpdateBuildingUI(model);
    }

    // Handles the selection of a building, updating the model and updating the building UI
    public void HandleBuildingSelection(string buildingName, Sprite buildingImage)
    {
        model.BuildingName = buildingName;
        model.BuildingImage = buildingImage;

        // Updates the building UI to reflect the selected building
        view.UpdateBuildingUI(model);
    }

    // Handles the update of the production image, updating the model and building UI
    public void HandleProductionImageUpdate(Sprite production)
    {
        model.ProductionImage = production;
        view.UpdateBuildingUI(model);
    }

    // Handles the clearing of the production image, updating the model and building UI
    public void HandleClearProductionImage()
    {
        model.ProductionImage = null;

        // Updates the building UI to clear the production image
        view.UpdateBuildingUI(model);
    }
}